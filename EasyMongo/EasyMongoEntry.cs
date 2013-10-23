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

namespace EasyMongo
{
    public class _TestEntry : IEasyMongoEntry
    {
        public _TestEntry()
        {
            TimeStamp = DateTime.Now;
        }

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        public BsonObjectId ID
        {
            get;
            set;
        }

        /// <summary>
        /// A description of activity to be logged
        /// </summary>
        [BsonIgnoreIfNull]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// A field representing the time the Log Entry was initialized
        /// </summary>
        public DateTime TimeStamp
        {
            get;
            set;
        }
    }
}
