using System;
using System.Collections.Generic;
using System.Text;

namespace EasyMongo
{
    public class MongoServerConnectionException : Exception
    {
        public MongoServerConnectionException(string message)
            : base(message)
        {
        }

        public MongoServerConnectionException(string message, Exception ex)
            : base(message, ex)
        {
        }
    } 
}
