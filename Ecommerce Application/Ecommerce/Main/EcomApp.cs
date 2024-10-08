using Ecommerce.Dao;
using Ecommerce.Model;
using Spectre.Console;
namespace Ecommerce.Main
{
    public class EcomApp
    {

        private IOrderProcessorRepository _repository = new OrderProcessorRepositoryImpl();

        public void Start()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            CenterText("Welcome to the eCommerce Application!\n");
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("Are you a User or Admin? (Enter 'User', 'Admin', or 'Exit' to quit): \n");
                Console.ResetColor();
                string userType = Console.ReadLine();

                if (userType.Equals("User", StringComparison.OrdinalIgnoreCase))
                {
                    HandleUserFlow();
                }
                else if (userType.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    HandleAdminLogin();
                }
                else if (userType.Equals("Exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Exiting the E-commerce Application. Goodbye! Have a nice day.");
                    Console.ResetColor();
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter 'User', 'Admin', or 'Exit'.");
                    Console.ResetColor();
                }
            }
        }

        private void HandleUserFlow()
        {
            Console.Write("Are you a new user? (yes/no or type 'exit' to quit): ");
            string isNewUser = Console.ReadLine();

            if (isNewUser.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Customer customer = null;

            if (isNewUser.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                customer = RegisterCustomer();
                if (customer == null) return;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You have successfully registered!");
                Console.ResetColor();
            }
            else
            {
                customer = AuthenticateCustomer();
                if (customer == null) return;
            }

            while (true)
            {
                Console.WriteLine("\n[1] View Products");
                Console.WriteLine("[2] View Cart");
                Console.WriteLine("[3] Make a Purchase");
                Console.WriteLine("[4] Exit");
                Console.Write("Select an option: ");
                string userChoice = Console.ReadLine();

                switch (userChoice)
                {
                    case "1":
                        ViewProducts();
                        break;
                    case "2":
                        ViewCart(customer);
                        break;
                    case "3":
                        UserPurchaseFlow(customer);
                        break;
                    case "4":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Exiting. Goodbye!\n");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.\n");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private Customer AuthenticateCustomer()
        {
            Console.Write("Enter your email: ");
            string email = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            Customer customer = _repository.GetCustomerByEmail(email);

            if (customer != null && customer.Password == password)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Login Successful!");
                Console.ResetColor();
                return customer;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid email or password.\n");
                Console.ResetColor();
                return null;
            }
        }

        private Customer RegisterCustomer()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            Customer customer = new Customer
            {
                Name = name,
                Email = email,
                Password = password
            };

            if (_repository.CreateCustomer(customer))
            {
                return _repository.GetCustomerByEmail(email);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to register customer.");
                Console.ResetColor();
                return null;
            }
        }

        private void UserPurchaseFlow(Customer customer)
        {
            while (true)
            {
                Console.Write("Would you like to make a purchase? (yes/no): ");
                string purchaseChoice = Console.ReadLine();

                if (purchaseChoice.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    ViewProducts();
                    Console.Write("Enter Product ID: ");
                    int productId = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter Quantity: ");
                    int quantity = Convert.ToInt32(Console.ReadLine());

                    Product product = _repository.GetProductById(productId);

                    if (product != null)
                    {
                        bool success = _repository.AddToCart(customer, product, quantity);

                        if (success)
                        {
                            ViewCart(customer);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Product added to cart successfully!\n");
                            Console.ResetColor();

                            
                            Console.Write("Would you like to add more products to the cart? (yes/no): ");
                            string addMore = Console.ReadLine();

                            if (addMore.Equals("no", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.Write("Would you like to place an order? (yes/no): ");
                                string placeOrderChoice = Console.ReadLine();

                                if (placeOrderChoice.Equals("yes", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.Write("Enter Shipping Address: ");
                                    string shippingAddress = Console.ReadLine();

                                    if (_repository.PlaceOrder(customer, shippingAddress))
                                    {
                                        
                                        _repository.DecreaseProductQuantity(productId, quantity);

                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine("Order placed successfully!\n");
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Failed to place the order.\n");
                                        Console.ResetColor();
                                    }
                                }
                                return;
                            }
                            else
                            {
                                
                                continue;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Failed to add product to cart.\n");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Product not found.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("You chose not to make a purchase.");
                    Console.ResetColor();
                    return;
                }
            }
        }



        private void ViewCart(Customer customer)
        {
            List<Cart> cartItems = _repository.GetAllFromCart(customer);

            if (cartItems.Count == 0)
            {
                AnsiConsole.Markup("[yellow]Your cart is empty.[/]\n");
                return;
            }

            var table = new Table();
            table.Border(TableBorder.Rounded);

            table.AddColumn("Product ID");
            table.AddColumn("Product Name");
            table.AddColumn("Quantity");
            table.AddColumn("Price");

            foreach (var item in cartItems)
            {
                table.AddRow(
                    item.Product.ProductId.ToString(),
                    item.Product.Name,
                    item.Quantity.ToString(),
                    item.Product.Price.ToString("F2")
                );
            }

            AnsiConsole.Write(table); 
        }


        private void HandleAdminLogin()
        {
            Console.Write("Enter Admin Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Admin Password: ");
            string password = Console.ReadLine();

            
            if (email.Equals("thiyanesh@gmail.com", StringComparison.OrdinalIgnoreCase) && password == "thiyanesh@123")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Admin login successful!");
                Console.ResetColor();

                HandleAdminFlow();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Admin email or password.\n");
                Console.ResetColor();
            }
        }

        private void HandleAdminFlow()
        {
            while (true)
            {
                Console.WriteLine("\nAdmin Options:");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View Products");
                Console.WriteLine("3. Remove Product");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                int adminChoice = Convert.ToInt32(Console.ReadLine());

                switch (adminChoice)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        ViewProducts();
                        break;
                    case 3:
                        RemoveProduct(); 
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Exiting admin mode.\n");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option.");
                        Console.ResetColor();
                        break;
                }
            }
        }
        private void RemoveProduct()
        {
            Console.Write("Enter Product ID to remove: ");
            int productId = Convert.ToInt32(Console.ReadLine());

            if (_repository.RemoveProduct(productId))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product removed successfully!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to remove product. It may not exist.");
                Console.ResetColor();
            }
        }



        private void AddProduct()
        {
            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Price: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            Console.Write("Enter Description: ");
            string description = Console.ReadLine();
            Console.Write("Enter Stock Quantity: ");
            int stock = Convert.ToInt32(Console.ReadLine());

            Product product = new Product
            {
                Name = name,
                Price = price,
                Description = description,
                StockQuantity = stock
            };

            if (_repository.CreateProduct(product))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product added successfully!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to add product.");
                Console.ResetColor();
            }
        }

        private void ViewProducts()
        {
            List<Product> products = _repository.GetProducts();

            var table = new Table();
            table.Border(TableBorder.Rounded);

            table.AddColumn("ID");
            table.AddColumn("Name");
            table.AddColumn("Price");
            table.AddColumn("Stock");
            table.AddColumn("Description");

            foreach (var product in products)
            {
                table.AddRow(
                    product.ProductId.ToString(),
                    product.Name,
                    product.Price.ToString("F2"), 
                    product.StockQuantity.ToString(),
                    product.Description
                );
            }

            AnsiConsole.Write(table); 
        }

        private void CenterText(string text)
        {
            int screenWidth = Console.WindowWidth;
            int textWidth = text.Length;
            int spaces = (screenWidth - textWidth) / 2;
            Console.WriteLine(new string(' ', spaces) + text);
        }
    }
}