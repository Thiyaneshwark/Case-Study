namespace Ecommerce.Model
{
    public class Cart
    {
        public int CartId { get; set; }
        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public Cart() { }

        public Cart(int cartId, Customer customer, Product product, int quantity)
        {
            CartId = cartId;
            Customer = customer;
            Product = product;
            Quantity = quantity;
        }
    }
}