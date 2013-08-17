using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDB
{
    public class EntryBase
    {
        public EntryBase()
        {
            TimeStamp = DateTime.Now;
        }

        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        public BsonObjectId MongoId
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
