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

namespace EasyMongo.Test.Model
{
    /// <summary>
    /// This class exists for unit testing purposes to replace an usage instance of an object that will 
    /// be stored/retrieved from the MongoDB. It exists in a separate assembly from the TestFixtures in order
    /// to be bound via a Ninject module to the objects in the TestFixtures
    /// </summary>
    /// TODO - Investigate removal of EasyMongo.Test.Model & TestEntry ?? why is this currently needed after refactoring
    /// of EasyMongo classes to not take a model object at construction time? I think it can be removed...
    public class TestEntry : IEasyMongoEntry
    {
        public TestEntry()
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
