namespace Ecommerce.Model
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; } 
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public OrderItem() { }

        public OrderItem(int orderItemId, int orderId, Product product, int quantity)
        {
            OrderItemId = orderItemId;
            OrderId = orderId; 
            Product = product;
            Quantity = quantity;
        }
    }
}
