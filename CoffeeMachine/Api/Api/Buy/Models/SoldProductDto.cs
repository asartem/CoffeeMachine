namespace Cm.Api.Api.Buy.Models
{
    /// <summary>
    /// Product and qty
    /// </summary>
    public class SoldProductDto
    {
        /// <summary>
        /// Product id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Price of single item
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// Number of sold products
        /// </summary>
        public int SoldQuantity { get; set; }

        /// <summary>
        /// Total cost
        /// </summary>
        public int Cost => Price * SoldQuantity;

        /// <summary>
        /// Create instance of the class
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="soldQuantity"></param>
        public SoldProductDto(int id, string name, int price, int soldQuantity)
        {
            Id = id;
            Name = name;
            Price = price;
            SoldQuantity = soldQuantity;
        }
    }
}