using System;
using System.Collections.Generic;
using System.Text;

namespace EasyMongo
{
    public class MongoDisabledException : Exception
    {
        public MongoDisabledException(string message)
            : base(message)
        {
        }
    } 
}
