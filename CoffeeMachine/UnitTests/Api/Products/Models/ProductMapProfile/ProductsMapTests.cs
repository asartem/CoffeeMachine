//using System;
//using System.Collections.Generic;
//using System.Text;
//using Api.Products.Models;
//using AutoMapper;
//using Domain.Products;
//using Domain.Users;
//using NUnit.Framework;

//namespace Tests.Api.Products.Models.ProductMapProfile
//{
//    [TestFixture]
//    public class ProductsMapTests
//    { 
//        private IMapper mapper;

//        [Test]
//        public void Product_ToDto()
//        {
//            var role = new UserRole { Id = 1 };
//            var user = new User("Test user", "pass", "test@mail.com", role);
//            var product = new Product("Test", 1) { Id = 1, User = user };

            
//            var productDto = mapper.Map<Product, ProductDto>(product);


//        }
//    }
//}
