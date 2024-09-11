using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandwritingCompressor.Exceptions
{
    public class NotActivatedProductException : Exception
    {
        public NotActivatedProductException(string message) : base(message) { }
        public NotActivatedProductException() : base(string.Empty) { }

    }
}
