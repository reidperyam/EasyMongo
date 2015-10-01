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
    public class AsyncUpdaterTTest : IntegrationTestFixture
    {
        [Test]
        public async void RemoveTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsyncDelegate(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            WriteConcernResult writeConcernResult = await _asyncUpdaterT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery);
            Assert.IsFalse(writeConcernResult.UpdatedExisting);
            Assert.IsFalse(writeConcernResult.HasLastErrorMessage);
            Assert.IsNull(writeConcernResult.LastErrorMessage);
            Assert.IsNull(writeConcernResult.Upserted);
            Assert.AreEqual(1, writeConcernResult.DocumentsAffected);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public async void RemoveTest2()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsyncDelegate(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            var searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.Single means only one occurance matching searchQuery will be removed
            WriteConcernResult writeConcernResult = await _asyncUpdaterT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.Single);

            Assert.IsFalse(writeConcernResult.UpdatedExisting);
            Assert.IsFalse(writeConcernResult.HasLastErrorMessage);
            Assert.IsNull(writeConcernResult.LastErrorMessage);
            Assert.IsNull(writeConcernResult.Upserted);
            Assert.AreEqual(1, writeConcernResult.DocumentsAffected);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
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

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            writeConcernResult = await _asyncUpdaterT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.None);

            Assert.IsFalse(writeConcernResult.UpdatedExisting);
            Assert.IsFalse(writeConcernResult.HasLastErrorMessage);
            Assert.IsNull(writeConcernResult.LastErrorMessage);
            Assert.IsNull(writeConcernResult.Upserted);
            Assert.AreEqual(2, writeConcernResult.DocumentsAffected);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public async void RemoveTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            string entryMessage2 = "entry 2";
            AddMongoEntryAsyncDelegate(entryMessage2, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage2, results[1].Message);

            var searchQuery = Query.NE("Message", entryMessage1);

            // remove entries with Message != entryMessage1
            WriteConcernResult writeConcernResult = await _asyncUpdaterT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, _writeConcern);

            Assert.IsFalse(writeConcernResult.UpdatedExisting);
            Assert.IsFalse(writeConcernResult.HasLastErrorMessage);
            Assert.IsNull(writeConcernResult.LastErrorMessage);
            Assert.IsNull(writeConcernResult.Upserted);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public async void RemoveTest4()
        {
            #region RemoveFlags.Single
            string entryMessage1 = "entry 1";
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
            AddMongoEntryAsyncDelegate(entryMessage1, MONGO_COLLECTION_1_NAME);
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
            WriteConcernResult writeConcernResult = await _asyncUpdaterT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.Single, _writeConcern);

            Assert.IsFalse(writeConcernResult.UpdatedExisting);
            Assert.IsFalse(writeConcernResult.HasLastErrorMessage);
            Assert.IsNull(writeConcernResult.LastErrorMessage);
            Assert.IsNull(writeConcernResult.Upserted);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
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

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
            Assert.AreEqual(entryMessage1, results[1].Message);
            Assert.AreEqual(entryMessage2, results[2].Message);

            searchQuery = Query.NE("Message", entryMessage2);

            // remove entries with Message != entryMessage1
            // RemoveFlags.None means every occurance matching searchQuery will be removed
            writeConcernResult = await _asyncUpdaterT.RemoveAsync(MONGO_COLLECTION_1_NAME, searchQuery, RemoveFlags.None, _writeConcern);

            Assert.IsFalse(writeConcernResult.UpdatedExisting);
            Assert.IsFalse(writeConcernResult.HasLastErrorMessage);
            Assert.IsNull(writeConcernResult.LastErrorMessage);
            Assert.IsNull(writeConcernResult.Upserted);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage2, results[0].Message);
            #endregion RemoveFlags.None
        }

        [Test]
        public async void FindAndModifyTest1()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
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

            FindAndModifyResult findAndModifyResult = await _asyncUpdaterT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok);
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public async void FindAndModifyTest2()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);

            var searchQuery = Query.EQ("Message", entryMessage1);

            var update = Update.Set("Message", MONGO_EDITED_TEXT);
            var sortBy = SortBy.Descending("TimeStamp");

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = false;

            FindAndModifyResult findAndModifyResult = await _asyncUpdaterT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok);
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument);

            findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = false;

            findAndModifyResult = await _asyncUpdaterT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok);
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNull(findAndModifyResult.ModifiedDocument);

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public async void FindAndModifyTest3()
        {
            string entryMessage1 = "entry 1";
            AddMongoEntry(entryMessage1, MONGO_COLLECTION_1_NAME);

            List<Entry> results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
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

            FindAndModifyResult findAndModifyResult = await _asyncUpdaterT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok);
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument); /* This should be populated as per the last argument to FindAndModify...*/

            findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Upsert = false;

            findAndModifyResult = await _asyncUpdaterT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok);
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNull(findAndModifyResult.ModifiedDocument); /*This should be populated as per the last argument to FindAndModify...*/

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage1, results[0].Message);
        }

        [Test]
        public async void FindAndModifyTest4()
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

            FindAndModifyArgs findAndModifyArgs = new FindAndModifyArgs();
            findAndModifyArgs.Query = searchQuery;
            findAndModifyArgs.SortBy = sortBy;
            findAndModifyArgs.Update = update;
            findAndModifyArgs.Fields = fields;
            findAndModifyArgs.Upsert = true;
            findAndModifyArgs.VersionReturned = FindAndModifyDocumentVersion.Modified;

            FindAndModifyResult findAndModifyResult = await _asyncUpdaterT.FindAndModifyAsync(MONGO_COLLECTION_1_NAME, findAndModifyArgs);

            Assert.IsTrue(findAndModifyResult.Ok);
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument); 

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(MONGO_EDITED_TEXT, results[0].Message);/*This field we modified via FindAndModify...*/
        }

        [Test]
        public async void FindAndRemoveTest()
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

            FindAndModifyResult findAndModifyResult = await _asyncUpdaterT.FindAndRemoveAsync(MONGO_COLLECTION_1_NAME, findAndRemoveArgs);

            Assert.IsTrue(findAndModifyResult.Ok);
            Assert.IsNull(findAndModifyResult.ErrorMessage);
            Assert.IsNotNull(findAndModifyResult.ModifiedDocument); 

            results = new List<Entry>(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(0, results.Count());/*we deleted the entry via FindAndRemove...*/
        }
    }
}
