using System;

namespace Ecommerce.Exceptions
{
    public class OrderNotFoundException : System.Exception
    {
        public OrderNotFoundException(string message) : base(message)
        {
        }
    }
}
