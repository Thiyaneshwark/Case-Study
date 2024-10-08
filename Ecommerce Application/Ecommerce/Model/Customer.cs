namespace Ecommerce.Model
{
    public class Customer
    {
        public int CustomerId { get; set; } // Identity column
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Customer() { }

        public Customer(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}
