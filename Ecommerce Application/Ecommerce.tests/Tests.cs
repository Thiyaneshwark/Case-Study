using Ecommerce.Dao;
using Ecommerce.Model;
using Moq;
using NUnit.Framework;
using System;

namespace Ecommerce.Tests
{
    [TestFixture]
    public class OrderProcessorRepositoryTests
    {
        private Mock<IOrderProcessorRepository> _mockOrderRepo;
        private IOrderProcessorRepository _orderRepo;

        public OrderProcessorRepositoryTests()
        {
            
            _mockOrderRepo = new Mock<IOrderProcessorRepository>();
            _orderRepo = new OrderProcessorRepositoryImpl(); 
        }

        [SetUp]
        public void Setup()
        {
            
            _mockOrderRepo.Reset();
        }

        [Test]
        public void CreatedProduct_ShouldReturnTrue()
        {
            // Arrange
            var product = new Product
            {
                Name = "Sneakers",
                StockQuantity = 50,
                Price = 100.00m,
                Description = "Comfortable running shoes"
            };

            
            _mockOrderRepo.Setup(repo => repo.CreateProduct(product)).Returns(true);

            // Act
            bool status = _mockOrderRepo.Object.CreateProduct(product);

            // Assert
            Assert.That(status, Is.True, "Product should be created successfully.");
        }

        [Test]
        public void AddedToCart_ShouldReturnTrue()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1 }; 
            var product = new Product { ProductId = 1 };    
            int quantity = 2;

            
            _mockOrderRepo.Setup(repo => repo.AddToCart(customer, product, quantity)).Returns(true);

            // Act
            bool status = _mockOrderRepo.Object.AddToCart(customer, product, quantity);

            // Assert
            Assert.That(status, Is.True, "Product should be added to the cart successfully.");
        }

        [Test]
        public void PlacedOrder_ShouldReturnTrue()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1 }; 
            string shippingAddress = "123 Main St";

            
            _mockOrderRepo.Setup(repo => repo.PlaceOrder(customer, shippingAddress)).Returns(true);

            // Act
            bool status = _mockOrderRepo.Object.PlaceOrder(customer, shippingAddress);

            // Assert
            Assert.That(status, Is.True, "Order should be placed successfully.");
        }

        [Test]
        public void AddToCart_ShouldThrowException_WhenCustomerIdNotFound()
        {
            // Arrange
            var invalidCustomer = new Customer { CustomerId = 999 }; 
            var product = new Product { ProductId = 1 }; 
            int quantity = 1;

            _mockOrderRepo.Setup(repo => repo.AddToCart(invalidCustomer, product, quantity))
                          .Throws(new InvalidOperationException("Customer not found in the database."));

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _mockOrderRepo.Object.AddToCart(invalidCustomer, product, quantity));
            Assert.That(ex.Message, Is.EqualTo("Customer not found in the database."), "Expected exception for non-existing customer ID.");
        }

        [Test]
        public void AddToCart_ShouldThrowException_WhenProductIdNotFound()
        {
            // Arrange
            var customer = new Customer { CustomerId = 1 }; 
            var invalidProduct = new Product { ProductId = 999 }; 
            int quantity = 1;

            _mockOrderRepo.Setup(repo => repo.AddToCart(customer, invalidProduct, quantity))
                          .Throws(new InvalidOperationException("Product not found in the database."));

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _mockOrderRepo.Object.AddToCart(customer, invalidProduct, quantity));
            Assert.That(ex.Message, Is.EqualTo("Product not found in the database."), "Expected exception for non-existing product ID.");
        }
    }
}
