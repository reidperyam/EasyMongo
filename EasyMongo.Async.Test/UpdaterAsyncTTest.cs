using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Threading;
using EasyMongo.Test.Base;
using EasyMongo.Contract;

namespace EasyMongo.Async.Test
{
    [TestFixture]
    public class UpdaterAsyncTTest : IntegrationTestFixture
    {
        [Test]
        public void RemoveTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            _updaterAsyncT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery);
            _updaterAutoResetEvent.WaitOne();

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void RemoveTest2()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            _updaterAsyncT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.Single);
            _updaterAutoResetEvent.WaitOne();

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection<Entry>(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            _updaterAsyncT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.None);
            _updaterAutoResetEvent.WaitOne();

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public void RemoveTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            _updaterAsyncT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, _writeConcern);
            _updaterAutoResetEvent.WaitOne();

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void RemoveTest4()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            _updaterAsyncT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.Single, _writeConcern);
            _updaterAutoResetEvent.WaitOne();

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection<Entry>(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            _updaterAsyncT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.None, _writeConcern);
            _updaterAutoResetEvent.WaitOne();

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public void FindAndModifyTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            _updaterAsyncT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, searchQuery, sortBy, update);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndModifyTest2()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            _updaterAsyncT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, searchQuery, sortBy, update, true);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);

            _updaterAsyncT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, searchQuery, sortBy, update, false);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNull(_findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndModifyTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", "some message that won't be found by the query");

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            _updaterAsyncT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, searchQuery, sortBy, update, true, true);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);/*This should be populated as per the last argument to FindAndModify...*/

            _updaterAsyncT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, searchQuery, sortBy, update, true, false);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNull(_findAndModifyResult.ModifiedDocument);/*This should be populated as per the last argument to FindAndModify...*/

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void FindAndModifyTest4()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Include("TimeStamp");
            _updaterAsyncT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, searchQuery, sortBy, update, fields, true, true);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndRemoveTest()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Null;
            FindAndRemoveArgs findAndRemoveArgs = new FindAndRemoveArgs();
            findAndRemoveArgs.Query = searchQuery;
            findAndRemoveArgs.SortBy = sortBy;
            _updaterAsyncT.FindAndRemoveAsync(MONGO_COLLECTION_1_NAME, findAndRemoveArgs);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(0, results.Count());/*we deleted the entry via FindAndRemove...*/
        }
    }
}
