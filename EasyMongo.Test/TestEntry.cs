using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using EasyMongo;

namespace EasyMongo.Test
{
    public class TestEntry : EntryBase
    {
        /// <summary>
        /// A description of activity to be logged
        /// </summary>
        [BsonIgnoreIfNull]
        public string Message
        {
            get;
            set;
        }
    }
}
