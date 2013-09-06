using System;
using System.Collections.Generic;
using System.Text;

namespace EasyMongo
{
    public class InvalidSearchException : Exception
    {
        public InvalidSearchException(string message)
            : base(message)
        {
        }
    }
}
