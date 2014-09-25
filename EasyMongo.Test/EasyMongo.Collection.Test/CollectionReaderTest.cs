using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EasyMongo.Contract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using EasyMongo.Test.Base;

namespace EasyMongo.Collection.Test
{
    [TestFixture]
    public class CollectionReaderTest : IntegrationTestFixture
    {        
        [Test]
        public void ConstructorTest()
        {
            // construct a collection reader from a database reader and a collection name
            _collectionReader = new Collection.CollectionReader(_databaseReader, MONGO_COLLECTION_1_NAME);
            Assert.IsNotNull(_collectionReader);
        }

        #region    Read
        [Test]
        public void ReadTest1()
        {
            AddMongoEntry();

            _results.AddRange(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest2()
        {
            AddMongoEntry();

            _results.AddRange(_collectionReader.Read<Entry>("Message", "Hello"));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest3()
        {
            AddMongoEntry();

            _results.AddRange(_collectionReader.Read<Entry>("Message","Hello","TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest4()
        {
            AddMongoEntry("Hello World 1", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Hello World 2", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Hello World 3", MONGO_COLLECTION_1_NAME);

            _results.AddRange(_collectionReader.Read<Entry>());
            Assert.AreEqual(3, _results.Count());
            Assert.AreEqual("Hello World 1", _results[0].Message);
            Assert.AreEqual("Hello World 2", _results[1].Message);
            Assert.AreEqual("Hello World 3", _results[2].Message);
        }

        #region    Async
        [Test]
        public async void ReadAsyncTest1()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsyncDelegate(message: entryMessage);
            IEnumerable<Entry> results = await _collectionReader.ReadAsync<Entry>("TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);
        }

        [Test]
        public async void ReadAsyncTest2()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsyncDelegate(message: entryMessage);
            IEnumerable<Entry> results = await _collectionReader.ReadAsync<Entry>("Message", entryMessage);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);
        }

        [Test]
        public async void ReadAsyncTest3()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsyncDelegate(message: entryMessage);
            IEnumerable<Entry> results = await _collectionReader.ReadAsync<Entry>("Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);
        }

        [Test]
        public async void ReadAsyncTest4()
        {
            AddMongoEntry("Hello World 1", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Hello World 2", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Hello World 3", MONGO_COLLECTION_1_NAME);

            IEnumerable<Entry> results = await _collectionReader.ReadAsync<Entry>();
            Assert.AreEqual(3, results.Count());

            Assert.AreEqual("Hello World 1", results.ElementAt(0).Message);
            Assert.AreEqual("Hello World 2", results.ElementAt(1).Message);
            Assert.AreEqual("Hello World 3", results.ElementAt(2).Message);
        }
        #endregion Async
        #endregion Read

        #region    Distinct 
        [Test]
        public void DistinctTest1()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            List<string> list = new List<string>(_collectionReader.Distinct<string>("Message"));

            Assert.AreEqual(3, list.Count());
            Assert.AreEqual("One", list[0]);
            Assert.AreEqual("Two", list[1]);
            Assert.AreEqual("Three", list[2]);
        }

        [Test]
        public void DistinctTest2()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            List<string> list = new List<string>(_collectionReader.Distinct<string>("Message", searchQuery));

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual("One", list[0]);
        }

        #region    Async
        [Test]
        public async void DistinctAsyncTest1()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            IEnumerable<string> results = await _collectionReader.DistinctAsync<string>("Message");

            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
            Assert.AreEqual("Two", results.ElementAt(1));
            Assert.AreEqual("Three", results.ElementAt(2));
        }

        [Test]
        public async void DistinctAsyncTest2()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            IEnumerable<string> results = await _collectionReader.DistinctAsync<string>("Message", searchQuery);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
        }
        #endregion Async
        #endregion Distinct

        #region    Execute
        [Test]
        public void ExecuteTest1()
        {
            AddMongoEntry("Hello World 1");

            IMongoQuery query = Query.Matches("Message", new BsonRegularExpression("WORLD", "i"));

            _results.AddRange(_collectionReader.Execute<Entry>(query));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual("Hello World 1", _results[0].Message);
        }

        [Test]
        public void ExecuteAndsTest1()
        {
            AddMongoEntry("Hello World 1");
            AddMongoEntry("Hello World 2");

            IList<IMongoQuery> queries = new List<IMongoQuery>();

            queries.Add(Query.Matches("Message", new BsonRegularExpression("WORLD", "i")));
            queries.Add(Query.Matches("Message", new BsonRegularExpression("1")));

            _results.AddRange(_collectionReader.ExecuteAnds<Entry>(queries));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual("Hello World 1", _results[0].Message);
        }

        [Test]
        public void ExecuteOrsTest1()
        {
            AddMongoEntry("Hello World 1");
            AddMongoEntry("Goodbye Yellow Brick Road");

            IList<IMongoQuery> queries = new List<IMongoQuery>();

            queries.Add(Query.Matches("Message", new BsonRegularExpression("WORLD", "i")));
            queries.Add(Query.Matches("Message", new BsonRegularExpression("Road")));

            _results.AddRange(_collectionReader.ExecuteOrs<Entry>(queries));
            Assert.AreEqual(2, _results.Count());
            Assert.AreEqual("Hello World 1", _results[0].Message);
            Assert.AreEqual("Goodbye Yellow Brick Road", _results[1].Message);
        }

        #region    Async
        [Test]
        public async void ExecuteAsyncTest1()
        {
            AddMongoEntry("Hello World 1");

            IMongoQuery query = Query.Matches("Message", new BsonRegularExpression("WORLD", "i"));

            _results.AddRange(await _collectionReader.ExecuteAsync<Entry>( query));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual("Hello World 1", _results[0].Message);
        }

        [Test]
        public async void ExecuteAndsAsyncTest1()
        {
            AddMongoEntry("Hello World 1");
            AddMongoEntry("Hello World 2");

            IList<IMongoQuery> queries = new List<IMongoQuery>();

            queries.Add(Query.Matches("Message", new BsonRegularExpression("WORLD", "i")));
            queries.Add(Query.Matches("Message", new BsonRegularExpression("1")));

            _results.AddRange(await _collectionReader.ExecuteAndsAsync<Entry>(queries));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual("Hello World 1", _results[0].Message);
        }

        [Test]
        public async void ExecuteOrsAsyncTest1()
        {
            AddMongoEntry("Hello World 1");
            AddMongoEntry("Goodbye Yellow Brick Road");

            IList<IMongoQuery> queries = new List<IMongoQuery>();

            queries.Add(Query.Matches("Message", new BsonRegularExpression("WORLD", "i")));
            queries.Add(Query.Matches("Message", new BsonRegularExpression("Road")));

            _results.AddRange(await _collectionReader.ExecuteOrsAsync<Entry>(queries));
            Assert.AreEqual(2, _results.Count());
            Assert.AreEqual("Hello World 1", _results[0].Message);
            Assert.AreEqual("Goodbye Yellow Brick Road", _results[1].Message);
        }
        #endregion Async
        #endregion Execute
    }
}
