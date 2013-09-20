using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using EasyMongo.Contract;

namespace EasyMongo.Test.Base
{
    public interface ITestEntry : IEasyMongoEntry
    {
        /// <summary>
        /// A field representing the time the Log Entry was initialized
        /// </summary>
        DateTime TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// A description of activity to be logged
        /// </summary>
        [BsonIgnoreIfNull]
        string Message
        {
            get;
            set;
        }
    }
}
