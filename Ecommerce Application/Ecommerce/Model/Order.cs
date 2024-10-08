namespace Ecommerce.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; }

        public Order() { }

        public Order(int orderId, Customer customer, DateTime orderDate, decimal totalPrice, string shippingAddress)
        {
            OrderId = orderId;
            Customer = customer;
            OrderDate = orderDate;
            TotalPrice = totalPrice;
            ShippingAddress = shippingAddress;
        }
    }
}