using System;
using System.Collections.Generic;
using System.Text;

namespace EasyMongo
{
    public class MongoWriteException : Exception
    {
        public MongoWriteException(string message)
            : base(message)
        {
        }

        public MongoWriteException(string message, Exception ex)
            : base(message, ex)
        {
        }
    } 
}
