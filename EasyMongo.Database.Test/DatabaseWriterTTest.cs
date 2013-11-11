using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using EasyMongo.Test.Base;
using EasyMongo.Test.Model;

namespace EasyMongo.Database.Test
{
    [TestFixture]
    public class DatabaseWriterTTest : IntegrationTestFixture
    {
        [Test]
        public void WriteAsyncTest()
        {
            string entryMessage = "This is a test";
            AddMongoEntryAsyncT(entryMessage, MONGO_COLLECTION_1_NAME);
            _results.AddRange(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual(entryMessage, _results[0].Message);

            _results.Clear();

            string entryMessage2 = "This is a test as well";
            AddMongoEntryAsyncT(entryMessage2, MONGO_COLLECTION_1_NAME);
            _results.AddRange(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, _results.Count());
            Assert.AreEqual(entryMessage2, _results[1].Message);
        }
       
        [Test]
        public void WriteTest()
        {
            string entryMessage = "This is a test";
            AddMongoEntryT(entryMessage, MONGO_COLLECTION_1_NAME);
            _results.AddRange(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual(entryMessage, _results[0].Message);

            _results.Clear();

            string entryMessage2 = "This is a test as well";
            AddMongoEntryT(entryMessage2, MONGO_COLLECTION_1_NAME);
            _results.AddRange(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, _results.Count());
            Assert.AreEqual(entryMessage2, _results[1].Message);
        }
    }
}
