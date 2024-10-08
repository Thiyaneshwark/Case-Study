using System;

namespace Ecommerce.Exceptions
{
    public class ProductNotFoundException : System.Exception
    {
        public ProductNotFoundException(string message) : base(message) { }
    }
}
