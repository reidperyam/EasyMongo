using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EasyMongo.Contract;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using EasyMongo.Base.Test;

namespace EasyMongo.Collection.Test
{
    [TestFixture]
    public class UpdaterTest : TestBase
    {
        [Test]
        public void RemoveTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            _collectionUpdater.Remove(searchQuery);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void RemoveTest2()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            _collectionUpdater.Remove(searchQuery, RemoveFlags.Single);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            _collectionUpdater.Remove(searchQuery, RemoveFlags.None);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public void RemoveTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            _collectionUpdater.Remove( searchQuery, _writeConcern);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void RemoveTest4()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            _collectionUpdater.Remove(searchQuery, RemoveFlags.Single, _writeConcern);

            results = new List<TestEntry>(_collectionReader.Read( "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            _collectionUpdater.Remove(searchQuery, RemoveFlags.None, _writeConcern);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public void FindAndModifyTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            var findAndModifyResult = _collectionUpdater.FindAndModify(searchQuery, sortBy, update);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndModifyTest2()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            var findAndModifyResult = _collectionUpdater.FindAndModify(searchQuery, sortBy, update, true);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            findAndModifyResult = _collectionUpdater.FindAndModify(searchQuery, sortBy, update, false);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNull(findAndModifyResult.ModifiedDocument);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndModifyTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", "some message that won't be found by the query");

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            var findAndModifyResult = _collectionUpdater.FindAndModify(searchQuery, sortBy, update, true, true);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);/*This should be populated as per the last argument to FindAndModify...*/

            findAndModifyResult = _collectionUpdater.FindAndModify(searchQuery, sortBy, update, true, false);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNull(findAndModifyResult.ModifiedDocument);/*This should be populated as per the last argument to FindAndModify...*/


            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void FindAndModifyTest4()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Include("TimeStamp");
            var findAndModifyResult = _collectionUpdater.FindAndModify(searchQuery, sortBy, update, fields, true, true);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndRemoveTest()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Null;
            var findAndModifyResult = _collectionUpdater.FindAndRemove(searchQuery, sortBy);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(0, results.Count());/*we deleted the entry via FindAndRemove...*/
        }

        #region Async
        [Test]
        public void RemoveAsyncTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            _collectionUpdater.RemoveAsync(searchQuery);
            _updaterAutoResetEvent.WaitOne();

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void RemoveAsyncTest2()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            _collectionUpdater.RemoveAsync(searchQuery, RemoveFlags.Single);
            _updaterAutoResetEvent.WaitOne();

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            _collectionUpdater.RemoveAsync(searchQuery, RemoveFlags.None);
            _updaterAutoResetEvent.WaitOne();

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public void RemoveAsyncTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            _collectionUpdater.RemoveAsync(searchQuery, _writeConcern);
            _updaterAutoResetEvent.WaitOne();

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void RemoveAsyncTest4()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            _collectionUpdater.RemoveAsync(searchQuery, RemoveFlags.Single, _writeConcern);
            _updaterAutoResetEvent.WaitOne();

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsync(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            _collectionUpdater.RemoveAsync(searchQuery, RemoveFlags.None, _writeConcern);
            _updaterAutoResetEvent.WaitOne();

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public void FindAndModifyAsyncTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            _collectionUpdater.FindAndModifyAsync(searchQuery, sortBy, update);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndModifyAsyncTest2()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            _collectionUpdater.FindAndModifyAsync(searchQuery, sortBy, update, true);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);

            _collectionUpdater.FindAndModifyAsync(searchQuery, sortBy, update, false);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNull(_findAndModifyResult.ModifiedDocument);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndModifyAsyncTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", "some message that won't be found by the query");

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            _collectionUpdater.FindAndModifyAsync(searchQuery, sortBy, update, true, true);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);/*This should be populated as per the last argument to FindAndModify...*/

            _collectionUpdater.FindAndModifyAsync(searchQuery, sortBy, update, true, false);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNull(_findAndModifyResult.ModifiedDocument);/*This should be populated as per the last argument to FindAndModify...*/

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void FindAndModifyAsyncTest4()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Include("TimeStamp");
            _collectionUpdater.FindAndModifyAsync(searchQuery, sortBy, update, fields, true, true);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndRemoveAsyncTest()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<TestEntry> results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Null;
            _collectionUpdater.FindAndRemoveAsync(searchQuery, sortBy);
            _updaterAutoResetEvent.WaitOne();

            Assert.IsTrue(_findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(_findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(_findAndModifyResult.ModifiedDocument);

            results = new List<TestEntry>(_collectionReader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(0, results.Count());/*we deleted the entry via FindAndRemove...*/
        }
        #endregion Async
    }
}
