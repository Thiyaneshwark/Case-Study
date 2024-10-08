namespace Ecommerce.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        // public object Id { get; internal set; }

        public Product() { }

        public Product(int productId, string name, decimal price, string description, int stockQuantity)
        {
            ProductId = productId;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
        }
        public List<Product> GetProducts()
        {
            // Creating a list of products. You would typically retrieve this from a database.
            List<Product> products = new List<Product>
            {

            };

            // Return the list of products
            return products;
        }
    }
}