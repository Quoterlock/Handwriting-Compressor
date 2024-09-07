using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandwritingsCompressor.Exceptions
{
    public class InvalidProductKeyException : Exception
    {
        public InvalidProductKeyException(string message) : base(message) { }
    }
}
