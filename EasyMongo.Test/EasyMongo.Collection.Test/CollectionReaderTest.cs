﻿using System;
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

        #region    Distinct T
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
        #endregion Distinct T
    }
}