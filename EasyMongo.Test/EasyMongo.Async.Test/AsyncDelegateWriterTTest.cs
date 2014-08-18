using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using EasyMongo;
using EasyMongo.Test.Base;
using System.Threading;

namespace EasyMongo.Async.Test
{
    [TestFixture]
    public class AsyncDelegateWriterTTest : IntegrationTestFixture
    {
        /// <summary>
        /// Asynchronously writes a MongoTestEntry to a MongoDB and verifies that it was retrieved 
        /// and that the persisted properties are persisted as expected
        /// </summary>
        [Test]
        public void Simple_AddTest()
        {
            string entryMessage = "This is a test";
            AddMongoEntryAsyncDelegateT(entryMessage, MONGO_COLLECTION_1_NAME);
            _results.AddRange(_readerT.Read(MONGO_COLLECTION_1_NAME, "Message", entryMessage));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual(entryMessage, _results[0].Message);
        }

        /// <summary>
        /// Asynchronously writes a MongoTestEntry to a MongoDB and verifies that it was retrieved 
        /// and that the persisted properties are persisted as expected
        /// </summary>
        [Test]
        public void Add_TwoTest()
        {
            string entryMessage = "This is a test";
            AddMongoEntryAsyncDelegateT(entryMessage, MONGO_COLLECTION_1_NAME);
            _results.AddRange(_reader.Read<Entry>(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual(entryMessage, _results[0].Message);

            _results.Clear();

            string entryMessage2 = "This is a test as well";
            AddMongoEntryAsyncDelegateT(entryMessage2, MONGO_COLLECTION_1_NAME);
            _results.AddRange(_reader.Read<Entry>(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, _results.Count());
            Assert.AreEqual(entryMessage2, _results[1].Message);
        }
    }
}
