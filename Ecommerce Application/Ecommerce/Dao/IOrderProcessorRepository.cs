using System.Collections.Generic;
using Ecommerce.Model;

namespace Ecommerce.Dao
{
    public interface IOrderProcessorRepository
    {
        // Product Management
        bool CreateProduct(Product product); 
        bool DeleteProduct(int productId); 
        List<Product> GetProducts(); 
        Product GetProductById(int productId); 

        // Customer Management
        bool CreateCustomer(Customer customer); 
        bool DeleteCustomer(int customerId); 
        Customer GetCustomerByEmail(string email); 

        // Cart Management
        bool AddToCart(Customer customer, Product product, int quantity); 
        bool RemoveFromCart(Customer customer, Product product); 
        List<Cart> GetAllFromCart(Customer customer); 

        // Order Management
        bool PlaceOrder(Customer customer, string shippingAddress); 
        List<Order> GetOrdersByCustomer(int customerId); 
        void DecreaseProductQuantity(int productId, int quantity);
        bool RemoveProduct(int productId);
    }
}
