using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Ecommerce.Dao
{
    public class OrderProcessorRepositoryImpl : IOrderProcessorRepository
    {
        private string _connectionString = Utility.DbConnUtil.GetConnString();

        // Product Management
        public bool CreateProduct(Product product)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Products (name, price, description, stockQuantity) VALUES (@Name, @Price, @Description, @StockQuantity)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = product.Name;
                        cmd.Parameters.Add("@Price", System.Data.SqlDbType.Decimal).Value = product.Price;
                        cmd.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = product.Description;
                        cmd.Parameters.Add("@StockQuantity", System.Data.SqlDbType.Int).Value = product.StockQuantity;
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public bool DeleteProduct(int productId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Products WHERE product_id = @ProductId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = productId;
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Products";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product
                                {
                                    ProductId = Convert.ToInt32(reader["product_id"]),
                                    Name = reader["name"].ToString(),
                                    Price = Convert.ToDecimal(reader["price"]),
                                    Description = reader["description"].ToString(),
                                    StockQuantity = Convert.ToInt32(reader["stockQuantity"])
                                };
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return products;
        }

        public Product GetProductById(int productId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Products WHERE product_id = @ProductId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = productId;
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Product
                                {
                                    ProductId = Convert.ToInt32(reader["product_id"]),
                                    Name = reader["name"].ToString(),
                                    Price = Convert.ToDecimal(reader["price"]),
                                    Description = reader["description"].ToString(),
                                    StockQuantity = Convert.ToInt32(reader["stockQuantity"])
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return null;
        }

        // Customer Management
        public bool CreateCustomer(Customer customer)
        {
            try
            {
                string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

                if (!Regex.IsMatch(customer.Password, passwordPattern))
                {
                    throw new ArgumentException("Password must contain at least 8 characters, including one uppercase letter, one lowercase letter, one digit, and one special character.");
                }

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Customers (name, email, password) VALUES (@Name, @Email, @Password)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = customer.Name;
                        cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar).Value = customer.Email;
                        cmd.Parameters.Add("@Password", System.Data.SqlDbType.NVarChar).Value = customer.Password;

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            Console.WriteLine("Customer created successfully.");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Failed to create the customer.");
                            return false;
                        }
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid Input: {ex.Message}");
                return false;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }


        public bool DeleteCustomer(int customerId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Customers WHERE customer_id = @CustomerId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@CustomerId", System.Data.SqlDbType.Int).Value = customerId;
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public Customer GetCustomerByEmail(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Customers WHERE email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar).Value = email;
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Customer
                                {
                                    CustomerId = Convert.ToInt32(reader["customer_id"]),
                                    Name = reader["name"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Password = reader["password"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return null;
        }

        // Cart Management
        public bool AddToCart(Customer customer, Product product, int quantity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Cart (customer_id, product_id, quantity) VALUES (@CustomerId, @ProductId, @Quantity)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@CustomerId", System.Data.SqlDbType.Int).Value = customer.CustomerId;
                        cmd.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = product.ProductId;
                        cmd.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = quantity;
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public bool RemoveFromCart(Customer customer, Product product)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Cart WHERE customer_id = @CustomerId AND product_id = @ProductId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@CustomerId", System.Data.SqlDbType.Int).Value = customer.CustomerId;
                        cmd.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = product.ProductId;
                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public List<Cart> GetAllFromCart(Customer customer)
        {
            List<Cart> cartItems = new List<Cart>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT cart.cart_id, cart.quantity, products.* FROM Cart INNER JOIN Products ON cart.product_id = products.product_id WHERE cart.customer_id = @CustomerId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@CustomerId", System.Data.SqlDbType.Int).Value = customer.CustomerId;
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cart cart = new Cart
                                {
                                    CartId = Convert.ToInt32(reader["cart_id"]),
                                    Customer = customer,
                                    Product = new Product
                                    {
                                        ProductId = Convert.ToInt32(reader["product_id"]),
                                        Name = reader["name"].ToString(),
                                        Price = Convert.ToDecimal(reader["price"]),
                                        Description = reader["description"].ToString(),
                                        StockQuantity = Convert.ToInt32(reader["stockQuantity"])
                                    },
                                    Quantity = Convert.ToInt32(reader["quantity"])
                                };
                                cartItems.Add(cart);
                            }
                        }
                    }
                }
                return cartItems;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return cartItems;
        }

        // Order Management
        public bool PlaceOrder(Customer customer, string shippingAddress)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {

                        string orderQuery = "INSERT INTO Orders (customer_id, order_date, total_price, shipping_address) OUTPUT INSERTED.order_id VALUES (@CustomerId, @OrderDate, @TotalPrice, @ShippingAddress)";
                        using (SqlCommand orderCmd = new SqlCommand(orderQuery, conn, transaction))
                        {
                            orderCmd.Parameters.Add("@CustomerId", System.Data.SqlDbType.Int).Value = customer.CustomerId;
                            orderCmd.Parameters.Add("@OrderDate", System.Data.SqlDbType.DateTime).Value = DateTime.Now;

                            decimal totalPrice = 0;
                            List<Cart> cartItems = GetAllFromCart(customer);

                            // Checking stock availability and calculate total price
                            foreach (var item in cartItems)
                            {
                                if (item.Quantity <= 0)
                                {
                                    throw new InvalidOperationException("Quantity must be greater than zero.");
                                }

                                // Calculating total price
                                totalPrice += item.Product.Price * item.Quantity;

                                // Ensuring stock is sufficient
                                if (item.Product.StockQuantity < item.Quantity)
                                {
                                    throw new InvalidOperationException($"Not enough stock for product {item.Product.Name}. Required: {item.Quantity}, Available: {item.Product.StockQuantity}");
                                }
                            }

                            // Seting the parameters for order insert
                            orderCmd.Parameters.Add("@TotalPrice", System.Data.SqlDbType.Decimal).Value = totalPrice;
                            orderCmd.Parameters.Add("@ShippingAddress", System.Data.SqlDbType.NVarChar).Value = shippingAddress;
                            int orderId = (int)orderCmd.ExecuteScalar();


                            foreach (var item in cartItems)
                            {

                                string orderItemQuery = "INSERT INTO Order_Items (order_id, product_id, quantity) VALUES (@OrderId, @ProductId, @Quantity)";
                                using (SqlCommand orderItemCmd = new SqlCommand(orderItemQuery, conn, transaction))
                                {
                                    orderItemCmd.Parameters.Add("@OrderId", System.Data.SqlDbType.Int).Value = orderId;
                                    orderItemCmd.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = item.Product.ProductId;
                                    orderItemCmd.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = item.Quantity;
                                    orderItemCmd.ExecuteNonQuery();
                                }


                                item.Product.StockQuantity -= item.Quantity;


                                UpdateProductStock(item.Product, transaction);
                            }


                            string clearCartQuery = "DELETE FROM Cart WHERE customer_id = @CustomerId";
                            using (SqlCommand clearCartCmd = new SqlCommand(clearCartQuery, conn, transaction))
                            {
                                clearCartCmd.Parameters.Add("@CustomerId", System.Data.SqlDbType.Int).Value = customer.CustomerId;
                                clearCartCmd.ExecuteNonQuery();
                            }


                            transaction.Commit();
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error while placing order: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        // Method to update product stock in the database
        private void UpdateProductStock(Product product, SqlTransaction transaction)
        {
            try
            {
                string updateStockQuery = "UPDATE Products SET stockQuantity = @StockQuantity WHERE product_id = @ProductId";

                using (var cmd = new SqlCommand(updateStockQuery, transaction.Connection, transaction))
                {
                    cmd.Parameters.Add("@StockQuantity", System.Data.SqlDbType.Int).Value = product.StockQuantity;
                    cmd.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = product.ProductId;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
                throw; // Re-throwing the exception in case further handling is needed by the calling method
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                throw;
            }
        }




        public List<Order> GetOrders(Customer customer)
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Orders WHERE customer_id = @CustomerId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@CustomerId", System.Data.SqlDbType.Int).Value = customer.CustomerId;
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orders.Add(new Order
                                {
                                    OrderId = Convert.ToInt32(reader["order_id"]),
                                    Customer = customer,
                                    OrderDate = Convert.ToDateTime(reader["order_date"]),
                                    TotalPrice = Convert.ToDecimal(reader["total_price"]),
                                    ShippingAddress = reader["shipping_address"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return orders;
        }


        public List<Order> GetOrdersByCustomer(int customerId)
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM Orders WHERE customer_id = @CustomerId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@CustomerId", System.Data.SqlDbType.Int).Value = customerId;
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                orders.Add(new Order
                                {
                                    OrderId = Convert.ToInt32(reader["order_id"]),
                                    OrderDate = Convert.ToDateTime(reader["order_date"]),
                                    TotalPrice = Convert.ToDecimal(reader["total_price"]),
                                    ShippingAddress = reader["shipping_address"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return orders;
        }

        public void DecreaseProductQuantity(int productId, int quantity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE Products SET stockQuantity = stockQuantity - @Quantity WHERE product_id = @ProductId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = productId;
                        cmd.Parameters.Add("@Quantity", System.Data.SqlDbType.Int).Value = quantity;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public bool RemoveProduct(int productId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Products WHERE product_id = @ProductId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@ProductId", System.Data.SqlDbType.Int).Value = productId;
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Return true if a row was deleted
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Database Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}