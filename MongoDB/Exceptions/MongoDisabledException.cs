using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB
{
    public class MongoDisabledException : Exception
    {
        public MongoDisabledException(string message)
            : base(message)
        {
        }
    } 
}
