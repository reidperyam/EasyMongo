using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EasyMongo.Contract;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using EasyMongo.Test.Base;

namespace EasyMongo.Collection.Test
{
    [TestFixture]
    public class CollectionUpdaterTest : IntegrationTestFixture
    {
        [Test]
        public void ConstructorTest()
        {
            _collectionUpdater = new Collection.CollectionUpdater(_databaseUpdater, MONGO_COLLECTION_1_NAME);
            Assert.IsNotNull(_collectionUpdater);
        }

        [Test]
        public void RemoveTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            _collectionUpdater.Remove<Entry>(searchQuery);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
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

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            _collectionUpdater.Remove<Entry>(searchQuery, RemoveFlags.Single);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection<Entry>(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            _collectionUpdater.Remove<Entry>(searchQuery, RemoveFlags.None);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
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

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            _collectionUpdater.Remove<Entry>(searchQuery, _writeConcern);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
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

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            _collectionUpdater.Remove<Entry>(searchQuery, RemoveFlags.Single, _writeConcern);

            results = new List<Entry>(_collectionReader.Read<Entry>( "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection<Entry>(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            _collectionUpdater.Remove<Entry>(searchQuery, RemoveFlags.None, _writeConcern);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public void FindAndModifyTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = true;
            findAndModifyArgs.VersionReturned = FindAndModifyDocumentVersion.Modified;

            var findAndModifyResult = _collectionUpdater.FindAndModify<Entry>(findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndModifyTest2()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = true;
            findAndModifyArgs.VersionReturned = FindAndModifyDocumentVersion.Modified;

            var findAndModifyResult = _collectionUpdater.FindAndModify<Entry>(findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = false;

            findAndModifyResult = _collectionUpdater.FindAndModify<Entry>(findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNull(findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndModifyTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", "some message that won't be found by the query");

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = true;
            findAndModifyArgs.VersionReturned = FindAndModifyDocumentVersion.Modified;

            var findAndModifyResult = _collectionUpdater.FindAndModify<Entry>(findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);/*This should be populated as per the last argument to FindAndModify...*/

            findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = false;

            findAndModifyResult = _collectionUpdater.FindAndModify<Entry>(findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNull(findAndModifyResult.ModifiedDocument);/*This should be populated as per the last argument to FindAndModify...*/


            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public void FindAndModifyTest4()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Include("TimeStamp");

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Fields = fields;
            findAndModifyArgs.Upsert = true;
            findAndModifyArgs.VersionReturned = FindAndModifyDocumentVersion.Modified;

            var findAndModifyResult = _collectionUpdater.FindAndModify<Entry>(findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public void FindAndRemoveTest()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Null;
            FindAndRemoveArgs findAndRemoveArgs = new FindAndRemoveArgs();
            findAndRemoveArgs.Query = searchQuery;
            findAndRemoveArgs.SortBy = sortBy;
            var findAndModifyResult = _collectionUpdater.FindAndRemove<Entry>(findAndRemoveArgs);

            Assert.IsTrue(findAndModifyResult.Ok, "FindAndModifyResult from FindAndModify not OK");
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(0, results.Count());/*we deleted the entry via FindAndRemove...*/
        }

        #region Async
        [Test]
        public async void RemoveAsyncTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsyncDelegate(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            WriteConcernResult result = await _collectionUpdater.RemoveAsync<Entry>(searchQuery);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsFalse(result.HasLastErrorMessage);
            Assert.AreEqual(1, result.DocumentsAffected);
            Assert.IsFalse(result.UpdatedExisting);
            Assert.IsNull(result.Upserted);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public async void RemoveAsyncTest2()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsyncDelegate(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            WriteConcernResult result = await _collectionUpdater.RemoveAsync<Entry>(searchQuery, RemoveFlags.Single);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsFalse(result.HasLastErrorMessage);
            Assert.AreEqual(1, result.DocumentsAffected);
            Assert.IsFalse(result.UpdatedExisting);
            Assert.IsNull(result.Upserted);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection<Entry>(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsyncDelegate(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            result = await _collectionUpdater.RemoveAsync<Entry>(searchQuery, RemoveFlags.None);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsFalse(result.HasLastErrorMessage);
            Assert.AreEqual(2, result.DocumentsAffected);
            Assert.IsFalse(result.UpdatedExisting);
            Assert.IsNull(result.Upserted);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public async void RemoveAsyncTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsyncDelegate(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            WriteConcernResult result = await _collectionUpdater.RemoveAsync<Entry>(searchQuery, _writeConcern);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsFalse(result.HasLastErrorMessage);
            Assert.AreEqual(1, result.DocumentsAffected);
            Assert.IsFalse(result.UpdatedExisting);
            Assert.IsNull(result.Upserted);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public async void RemoveAsyncTest4()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            WriteConcernResult result = await _collectionUpdater.RemoveAsync<Entry>(searchQuery, RemoveFlags.Single, _writeConcern);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsFalse(result.HasLastErrorMessage);
            Assert.AreEqual(1, result.DocumentsAffected);
            Assert.IsFalse(result.UpdatedExisting);
            Assert.IsNull(result.Upserted);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);
            #endregion RemoveFlags.Single

            // clear the collection before trying different RemoveFlags value...
            _mongoDatabaseConnection.ClearCollection<Entry>(MONGO_COLLECTION_1_NAME);

            #region RemoveFlags.None
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsyncDelegate(entryMessage2, MONGO_COLLECTION_1_NAME);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            result = await _collectionUpdater.RemoveAsync<Entry>(searchQuery, RemoveFlags.None, _writeConcern);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsFalse(result.HasLastErrorMessage);
            Assert.AreEqual(2, result.DocumentsAffected);
            Assert.IsFalse(result.UpdatedExisting);
            Assert.IsNull(result.Upserted);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public async void FindAndModifyAsyncTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;

            FindAndModifyResult result = await _collectionUpdater.FindAndModifyAsync<Entry>(findAndModifyArgs);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsNull(result.ErrorMessage);
            Assert.IsNotNull(result.ModifiedDocument);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public async void FindAndModifyAsyncTest2()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = true;

            FindAndModifyResult result = await _collectionUpdater.FindAndModifyAsync<Entry>(findAndModifyArgs);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsNull(result.ErrorMessage);
            Assert.IsNotNull(result.ModifiedDocument);

            findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = false;

            result = await _collectionUpdater.FindAndModifyAsync<Entry>(findAndModifyArgs);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsNull(result.ErrorMessage);
            Assert.IsNull(result.ModifiedDocument);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public async void FindAndModifyAsyncTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", "some message that won't be found by the query");

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = true;
            findAndModifyArgs.VersionReturned = FindAndModifyDocumentVersion.Modified;

            FindAndModifyResult result = await _collectionUpdater.FindAndModifyAsync<Entry>(findAndModifyArgs);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsNull(result.ErrorMessage);
            Assert.IsNotNull(result.ModifiedDocument);

            findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = false;

            result = await _collectionUpdater.FindAndModifyAsync<Entry>(findAndModifyArgs);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsNull(result.ErrorMessage);
            Assert.IsNull(result.ModifiedDocument);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public async void FindAndModifyAsyncTest4()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Include("TimeStamp");

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Fields = fields;
            findAndModifyArgs.Upsert = true;
            findAndModifyArgs.VersionReturned = FindAndModifyDocumentVersion.Modified;

            FindAndModifyResult result = await _collectionUpdater.FindAndModifyAsync<Entry>(findAndModifyArgs);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsNull(result.ErrorMessage);
            Assert.IsNotNull(result.ModifiedDocument);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public async void FindAndRemoveAsyncTest()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");
            IMongoFields fields = Fields.Null;
            FindAndRemoveArgs findAndRemoveArgs = new FindAndRemoveArgs();
            findAndRemoveArgs.Query = searchQuery;
            findAndRemoveArgs.SortBy = sortBy;

            FindAndModifyResult result = await _collectionUpdater.FindAndRemoveAsync<Entry>(findAndRemoveArgs);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Ok);
            Assert.IsNull(result.ErrorMessage);
            Assert.IsNotNull(result.ModifiedDocument);

            results = new List<Entry>(_collectionReader.Read<Entry>("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(0, results.Count());/*we deleted the entry via FindAndRemove...*/
        }
        #endregion Async
    }
}
