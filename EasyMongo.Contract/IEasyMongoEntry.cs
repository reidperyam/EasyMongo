using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace EasyMongo.Contract
{
    public interface IEasyMongoEntry
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        BsonObjectId ID
        {
            get;
            set;
        }
    }
}
