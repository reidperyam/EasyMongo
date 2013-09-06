using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver.Builders;
using MongoDB.Driver;

namespace EasyMongo.Test
{
    [TestFixture]
    public class WriterTest : TestBase
    {
        /// <summary>
        /// Synchronously writes a MongoTestEntry to a MongoDB and verifies that it was retrieved 
        /// and that the persisted properties are persisted as expected
        /// </summary>
        [Test]
        public void Simple_AddTest()
        {
            string entryMessage = "This is a test";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);

            _results.AddRange(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "Message", entryMessage));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual(entryMessage, _results[0].Message);
        }

        /// <summary>
        /// Synchronously writes a MongoTestEntry to a MongoDB and verifies that it was retrieved 
        /// and that the persisted properties are persisted as expected
        /// </summary>
        [Test]
        public void Add_TwoTest()
        {
            string entryMessage = "This is a test";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            _results.AddRange(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual(entryMessage, _results[0].Message);

            _results.Clear();

            string entryMessage2 = "This is a test as well";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);
            _results.AddRange(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, _results.Count());
            Assert.AreEqual(entryMessage2, _results[1].Message);
        }

        //[Test]
        //public void RemoveTest1()
        //{
        //    string entryMessage1 = "entry 1";
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    string entryMessage2 = "entry 2";
        //    AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

        //    List<TestEntry> results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(2, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //    Assert.AreEqual(entryMessage2, results[1].Message);
    
        //    var searchQuery = Query.NE("Message", entryMessage1);

        //    // remove entries with Message != entryMessage1
        //    _mongoWriter.Remove(MONGO_COLLECTION_1_NAME, searchQuery);

        //    results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(1, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //}

        //[Test]
        //public void RemoveTest2()
        //{
        //    #region RemoveFlags.Single
        //    string entryMessage1 = "entry 1";
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    string entryMessage2 = "entry 2";
        //    AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

        //    List<TestEntry> results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(3, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //    Assert.AreEqual(entryMessage1, results[1].Message);
        //    Assert.AreEqual(entryMessage2, results[2].Message);

        //    var searchQuery = Query.NE("Message", entryMessage2);

        //    // remove entries with Message != entryMessage1
        //    // RemoveFlags.Single means only one occurance matching searchQuery will be removed
        //    _mongoWriter.Remove(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.Single);

        //    results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(2, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //    Assert.AreEqual(entryMessage2, results[1].Message);
        //    #endregion RemoveFlags.Single

        //    // clear the collection before trying different RemoveFlags value...
        //    _mongoDatabaseConnection.ClearCollection(MONGO_COLLECTION_1_NAME);

        //    #region RemoveFlags.None
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

        //    results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(3, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //    Assert.AreEqual(entryMessage1, results[1].Message);
        //    Assert.AreEqual(entryMessage2, results[2].Message);

        //    searchQuery = Query.NE("Message", entryMessage2);

        //    // remove entries with Message != entryMessage1
        //    // RemoveFlags.None means every occurance matching searchQuery will be removed
        //    _mongoWriter.Remove(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.None);

        //    results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(1, results.Count());
        //    Assert.AreEqual(entryMessage2, results[0].Message);
        //    #endregion RemoveFlags.None
        //}

        //[Test]
        //public void RemoveTest3()
        //{
        //    string entryMessage1 = "entry 1";
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    string entryMessage2 = "entry 2";
        //    AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

        //    List<TestEntry> results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(2, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //    Assert.AreEqual(entryMessage2, results[1].Message);

        //    var searchQuery = Query.NE("Message", entryMessage1);

        //    // remove entries with Message != entryMessage1
        //    _mongoWriter.Remove(MONGO_COLLECTION_1_NAME, searchQuery, _writeConcern);

        //    results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(1, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //}

        //[Test]
        //public void RemoveTest4()
        //{
        //    #region RemoveFlags.Single
        //    string entryMessage1 = "entry 1";
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    string entryMessage2 = "entry 2";
        //    AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

        //    List<TestEntry> results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(3, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //    Assert.AreEqual(entryMessage1, results[1].Message);
        //    Assert.AreEqual(entryMessage2, results[2].Message);

        //    var searchQuery = Query.NE("Message", entryMessage2);

        //    // remove entries with Message != entryMessage1
        //    // RemoveFlags.Single means only one occurance matching searchQuery will be removed
        //    _mongoWriter.Remove(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.Single, _writeConcern);

        //    results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(2, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //    Assert.AreEqual(entryMessage2, results[1].Message);
        //    #endregion RemoveFlags.Single

        //    // clear the collection before trying different RemoveFlags value...
        //    _mongoDatabaseConnection.ClearCollection(MONGO_COLLECTION_1_NAME);

        //    #region RemoveFlags.None
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
        //    AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

        //    results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(3, results.Count());
        //    Assert.AreEqual(entryMessage1, results[0].Message);
        //    Assert.AreEqual(entryMessage1, results[1].Message);
        //    Assert.AreEqual(entryMessage2, results[2].Message);

        //    searchQuery = Query.NE("Message", entryMessage2);

        //    // remove entries with Message != entryMessage1
        //    // RemoveFlags.None means every occurance matching searchQuery will be removed
        //    _mongoWriter.Remove(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.None, _writeConcern);

        //    results = new List<TestEntry>(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
        //    Assert.AreEqual(1, results.Count());
        //    Assert.AreEqual(entryMessage2, results[0].Message);
        //    #endregion RemoveFlags.None
        //}
    }
}
