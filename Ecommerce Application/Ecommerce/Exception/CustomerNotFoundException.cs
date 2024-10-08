using System;

namespace Ecommerce.Exceptions
{
    public class CustomerNotFoundException : System.Exception
    {
        public CustomerNotFoundException(string message) : base(message) { }
    }
}
