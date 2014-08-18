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
    /// <summary>
    /// An class useful for inheriting MongoDB model classes from for development or testing purposes.
    /// </summary>
    public class Entry
    {
        public Entry()
        {
            TimeStamp = DateTime.Now.ToUniversalTime();
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
        /// A field representing the time the Entry was initialized in UTC
        /// </summary>
        public DateTime TimeStamp
        {
            get;
            set;
        }
    }
}
