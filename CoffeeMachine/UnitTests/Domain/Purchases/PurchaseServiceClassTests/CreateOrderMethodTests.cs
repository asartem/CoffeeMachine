using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cm.Domain.Products;
using Cm.Domain.Purchases;
using Cm.Domain.Purchases.Exceptions;
using Cm.Domain.Users;
using Cm.Domain.Users.Roles;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cm.Tests.Domain.Purchases.PurchaseServiceClassTests
{
    [TestFixture]
    public class CreateOrderMethodTests
    {
        private IPurchaseService service;
        private Mock<IProductsRepository> productsRepositoryMock;
        private Mock<IUsersRepository> usersRepositoryMock;
        private const int BuyerId = 100;
        const int ProductId = 1;
        const int ProductQty = 10;
        const int QtyPurchased = 2;
        private User seller;
        private User buyer;
        const int BuyerDeposit = 200;
        const int Price = 20;

        [SetUp]
        public void Setup()
        {
            seller = new User("Seller", "1234", 0, new UserRole(1, "Seller"));
            buyer = new User("Buyer", "1234", BuyerDeposit, new UserRole(2, "Buyer"));

            productsRepositoryMock = new Mock<IProductsRepository>();
            usersRepositoryMock = new Mock<IUsersRepository>();

            service = new PurchaseService(usersRepositoryMock.Object, productsRepositoryMock.Object);
            usersRepositoryMock.Setup(x => x.GetAsync(It.IsAny<int>())).ReturnsAsync(buyer);
        }

        [Test]
        [Description("Should purchase single item and call all save repository methods")]
        public void SingleProduct_AllSaved()
        {

            IDictionary<int, int> dic1 = new Dictionary<int, int>();
            dic1.Add(1,2);

            var res = JsonConvert.SerializeObject(dic1);


            IList<Product> products = new List<Product>
            {
                new Product(ProductId, "Test", seller, Price, ProductQty)
            };

            productsRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            var dic = new Dictionary<int, int> { { ProductId, QtyPurchased } };
            service.CreateOrderAsync(dic, BuyerId);

            productsRepositoryMock.Verify(x => x.AddToContext(products[0]), Times.Exactly(products.Count));
            usersRepositoryMock.Verify(x => x.AddToContext(buyer), Times.Once);
            usersRepositoryMock.Verify(x => x.SaveContextAsync(), Times.Once);
        }


        [Test]
        [Description("Should purchase multiple item and call all save repository methods")]
        public void MultipleProducts_AllSaved()
        {
            IList<Product> products = CreateDefaultProducts();

            productsRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            var dic = new Dictionary<int, int>
            {
                { products[0].Id, QtyPurchased },
                { products[1].Id, QtyPurchased }
            };
            service.CreateOrderAsync(dic, BuyerId);

            productsRepositoryMock.Verify(x => x.AddToContext(products[0]), Times.Once);
            productsRepositoryMock.Verify(x => x.AddToContext(products[1]), Times.Once);
            usersRepositoryMock.Verify(x => x.AddToContext(buyer), Times.Once);
            usersRepositoryMock.Verify(x => x.SaveContextAsync(), Times.Once);
        }


        [Test]
        [Description("Should reduce number of purchased items")]
        public async Task QtyToPurchased_Reduced()
        {
            IList<Product> products = CreateDefaultProducts();

            productsRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            var dic = new Dictionary<int, int>
            {
                { products[0].Id, QtyPurchased },
                { products[1].Id, QtyPurchased }
            };
            var order = await service.CreateOrderAsync(dic, BuyerId);

            Assert.AreEqual(products[0].Qty, ProductQty - QtyPurchased);
            Assert.AreEqual(products[1].Qty, ProductQty - QtyPurchased);

            Assert.AreEqual(order.ProductsAndQty[products[0]], QtyPurchased);
            Assert.AreEqual(order.ProductsAndQty[products[1]], QtyPurchased);

        }

        [Test]
        [Description("Should calculate total cost as qty * price for each product")]
        public async Task PriceAndQty_SumOfPriceAndQty()
        {
            IList<Product> products = CreateDefaultProducts();

            productsRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            var dic = new Dictionary<int, int>
            {
                { products[0].Id, QtyPurchased },
                { products[1].Id, QtyPurchased }
            };
            var order = await service.CreateOrderAsync(dic, BuyerId);

            var expectedTotal = products.Sum(x => x.Price * QtyPurchased);
            Assert.AreEqual(expectedTotal, order.TotalCost);
        }

        [Test]
        [Description("Should reduce buyer deposit on total amount")]
        public async Task PriceAndQty_DepositReduced()
        {
            IList<Product> products = CreateDefaultProducts();

            productsRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            var dic = new Dictionary<int, int>
            {
                { products[0].Id, QtyPurchased },
                { products[1].Id, QtyPurchased }
            };
            var order = await service.CreateOrderAsync(dic, BuyerId);

            var expected = BuyerDeposit - products.Sum(x => x.Price * QtyPurchased);
            Assert.AreEqual(expected, buyer.Deposit);
            Assert.AreEqual(expected, order.ChangeAmount);
        }

        [Test]
        [Description("Should throw exception if try to buy non existing product")]
        public void WrongProductId_Exception()
        {

            IEnumerable<Product> products = new List<Product>
            {
                new Product(1, "Test", seller, Price, ProductQty)
            };

            productsRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            var dic = new Dictionary<int, int> { { 10, ProductQty } };
            Assert.ThrowsAsync<EntityNotFoundException>(() => service.CreateOrderAsync(dic, BuyerId));

        }

        [Test]
        [Description("Should throw exception if try to buy more then exist")]
        public void NotEnoughQty_Exception()
        {

            IEnumerable<Product> products = new List<Product>
            {
                new Product(1, "Test", seller, Price, ProductQty)
            };

            productsRepositoryMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<Product, bool>>>()))
                .ReturnsAsync(products);

            var dic = new Dictionary<int, int> { { ProductId, ProductQty + 1 } };
            Assert.ThrowsAsync<ProductIsOutOfStockException>(() => service.CreateOrderAsync(dic, BuyerId));

        }



        private IList<Product> CreateDefaultProducts()
        {
            IList<Product> products = new List<Product>
            {
                new Product(ProductId, "Test", seller, Price, ProductQty),
                new Product(ProductId + 1, "Test2", seller, Price * 2, ProductQty)
            };
            return products;
        }
    }
}
