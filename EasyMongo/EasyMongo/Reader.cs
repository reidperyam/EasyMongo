using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using EasyMongo;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;
using EasyMongo.Contract;

namespace EasyMongo
{
    public class Reader : IReader
    {
        IDatabaseConnection _databaseConnection;

        public Reader(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        #region    Methods
        #region   Read Against a Collection
        public IEnumerable<T> Read<T>(string collectionName)
        {
            List<T> results;
            Find(collectionName, out results);
            return results;
        }
        public IEnumerable<T> Read<T>(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            List<T> results;
            Find(collectionName, dateTimeFieldName, start, end, out results);
            return results;
        }
        public IEnumerable<T> Read<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            List<T> results;
            Find(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end, out results);
            return results;
        }
        public IEnumerable<T> Read<T>(string collectionName, string fieldName, string regexPattern)
        {
            List<T> results;
            Find(collectionName, fieldName, regexPattern, out results);
            return results;
        }
        #endregion Read Against a Collection
        #region   Read Against Multiple Collections
        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames)
        {
            List<T> results;
            Find(collectionNames, out results);
            return results;
        }
        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            List<T> results;
            Find(collectionNames, dateTimeFieldName, start, end, out results);
            return results;
        }
        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            List<T> results;
            Find(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end, out results);
            return results;
        }
        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            List<T> results;
            Find(collectionNames, fieldName, regexPattern, out results);
            return results;
        }
        #endregion Read Against Multiple Collections

        #region Read Implementation methods
        private void Find<T>(string collectionName, out List<T> results)
        {
            results = new List<T>();
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            results.AddRange(collection.FindAll());
        }
        private void Find<T>(string collectionName, string fieldName, string regexPattern, out List<T> results)
        {
            results = new List<T>();
            var searchQuery = Query.Matches(fieldName, new BsonRegularExpression(regexPattern));
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            results.AddRange(collection.Find(searchQuery));
        }
        private void Find<T>(string collectionName, string dateTimeFieldName, DateTime start, DateTime end, out List<T> results)
        {
            results = new List<T>();
            var searchQuery = Query.And(Query.GTE(dateTimeFieldName, start), Query.LTE(dateTimeFieldName, end));
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            results.AddRange(collection.Find(searchQuery));
        }
        private void Find<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end, out List<T> results)
        {
            results = new List<T>();
            var searchQuery = Query.And(Query.GTE(dateTimeFieldName, start), Query.LTE(dateTimeFieldName, end), Query.Matches(fieldName, new BsonRegularExpression(regexPattern)));
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            results.AddRange(collection.Find(searchQuery));
        }
        private void Find<T>(IEnumerable<string> collectionNames, out List<T> results)
        {
            results = new List<T>();
            List<T> resultsForCollection;

            foreach (string collectionName in collectionNames)
            {
                Find(collectionName, out resultsForCollection);
                results.AddRange(resultsForCollection);
                resultsForCollection.Clear();
            }
        }
        private void Find<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, out List<T> results)
        {
            results = new List<T>();
            List<T> resultsForCollection;

            foreach (string collectionName in collectionNames)
            {
                Find(collectionName, fieldName, regexPattern, out resultsForCollection);
                results.AddRange(resultsForCollection);
                resultsForCollection.Clear();
            }
        }
        private void Find<T>(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end, out List<T> results)
        {
            results = new List<T>();
            List<T> resultsForCollection;

            foreach (string collectionName in collectionNames)
            {
                Find(collectionName, dateTimeFieldName, start, end, out resultsForCollection);
                results.AddRange(resultsForCollection);
                resultsForCollection.Clear();
            }
        }
        private void Find<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end, out List<T> results)
        {
            results = new List<T>();
            List<T> resultsForCollection;

            foreach (string collectionName in collectionNames)
            {
                Find(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end, out resultsForCollection);
                results.AddRange(resultsForCollection);
                resultsForCollection.Clear();
            }
        }
        #endregion Read Implementation methods

        #region Distinct Across Collection T
        public IEnumerable<T> Distinct<T>(string collectionName, string fieldName)
        {
            List<T> results;
            Distinct<T>(collectionName, fieldName, out results);
            return results;
        }
        public IEnumerable<T> Distinct<T>(string collectionName, string fieldName, IMongoQuery query)
        {
            List<T> results;
            Distinct<T>(collectionName, fieldName, query, out results);
            return results;
        }
        #endregion Distinct Across Collection T
        #region Distinct Across Multiple Collections T
        public IEnumerable<T> Distinct<T>(IEnumerable<string> collectionNames, string fieldName)
        {
            List<T> results;
            Distinct<T>(collectionNames, fieldName, out results);
            return results;
        }
        public IEnumerable<T> Distinct<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            List<T> results;
            Distinct<T>(collectionNames, fieldName, query, out results);
            return results;
        }
        #endregion Distinct Across Multiple Collections T
        #region    Distinct Implementation
        #region    T
        private void Distinct<T>(string collectionName, string fieldName, out List<T> results)
        {
            results = new List<T>();
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            results.AddRange(collection.Distinct<T>(fieldName));
        }
        private void Distinct<T>(string collectionName, string fieldName, IMongoQuery query, out List<T> results)
        {
            results = new List<T>();
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            results.AddRange(collection.Distinct<T>(fieldName, query));
        }
        private void Distinct<T>(IEnumerable<string> collectionNames, string fieldName, out List<T> results)
        {
            results = new List<T>();
            List<T> resultsForCollection;

            foreach (string collectionName in collectionNames)
            {
                Distinct<T>(collectionName, fieldName, out resultsForCollection);

                // since this method returns distinct values and we are searching across collections
                // we need to cull values returned from other collections
                foreach (T result in resultsForCollection)
                {
                    if (!results.Contains(result))
                        results.Add(result);
                }

                resultsForCollection.Clear();
            }
        }
        private void Distinct<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query, out List<T> results)
        {
            results = new List<T>();
            List<T> resultsForCollection;

            foreach (string collectionName in collectionNames)
            {
                Distinct<T>(collectionName, fieldName, query, out resultsForCollection);

                // since this method returns distinct values and we are searching across collections
                // we need to cull values returned from other collections
                foreach (T result in resultsForCollection)
                {
                    if (!results.Contains(result))
                        results.Add(result);
                }
                resultsForCollection.Clear();
            }
        }
        #endregion T
        #endregion Distinct Implementation

        #region    Execute 
        public IEnumerable<T> Execute<T>(string collectionName, IMongoQuery mongoQuery)
        {
            List<T> results;
            Execute(collectionName, mongoQuery, out results);
            return results;
        }
        public IEnumerable<T> ExecuteAnds<T>(string collectionName, IEnumerable<IMongoQuery> mongoQueries)
        {
            List<T> results;
            ExecuteAnds(collectionName, mongoQueries, out results);
            return results;
        }
        public IEnumerable<T> ExecuteOrs<T>(string collectionName, IEnumerable<IMongoQuery> mongoQueries)
        {
            List<T> results;
            ExecuteOrs(collectionName, mongoQueries, out results);
            return results;
        }
        #endregion Execute

        #region    Execute Implementation Methods
        private void Execute<T>(string collectionName, IMongoQuery mongoQuery, out List<T> results)
        {
            results = new List<T>();
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            results.AddRange(collection.Find(mongoQuery));
        }
        private void ExecuteAnds<T>(string collectionName, IEnumerable<IMongoQuery> mongoQueries, out List<T> results)
        {
            results = new List<T>();
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            results.AddRange(collection.Find(Query.And(mongoQueries)));
        }
        private void ExecuteOrs<T>(string collectionName, IEnumerable<IMongoQuery> mongoQueries, out List<T> results)
        {
            results = new List<T>();
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            results.AddRange(collection.Find(Query.Or(mongoQueries)));
        }
        #endregion Execute Implementation Methods
        #endregion Methods
    }

    public class Reader<T> : IReader<T>
    {
        private IReader _reader;

        public Reader(IReader reader)
        {
            _reader = reader;
        }

        public IEnumerable<T> Read(string collectionName)
        {
            return _reader.Read<T>(collectionName);
        }

        public IEnumerable<T> Read(string collectionName, string fieldName, string regexPattern)
        {
            return _reader.Read<T>(collectionName, fieldName, regexPattern);
        }

        public IEnumerable<T> Read(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _reader.Read<T>(collectionName, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _reader.Read<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames)
        {
            return _reader.Read<T>(collectionNames);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return _reader.Read<T>(collectionNames, fieldName, regexPattern);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end)
        {
            return _reader.Read<T>(collectionNames, fieldName, start, end);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _reader.Read<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<Y> Distinct<Y>(string collectionName, string fieldName)
        {
            return _reader.Distinct<Y>(collectionName, fieldName);
        }

        public IEnumerable<Y> Distinct<Y>(string collectionName, string fieldName, IMongoQuery query)
        {
            return _reader.Distinct<Y>(collectionName, fieldName, query);
        }

        public IEnumerable<Y> Distinct<Y>(IEnumerable<string> collectionNames, string fieldName)
        {
            return _reader.Distinct<Y>(collectionNames, fieldName);
        }

        public IEnumerable<Y> Distinct<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return _reader.Distinct<Y>(collectionNames, fieldName, query);
        }

        public IEnumerable<T> Execute(string collectionName, IMongoQuery mongoQuery)
        {
            return _reader.Execute<T>(collectionName, mongoQuery);
        }

        public IEnumerable<T> ExecuteAnds(string collectionName, IEnumerable<IMongoQuery> mongoQueries)
        {
            return _reader.ExecuteAnds<T>(collectionName, mongoQueries);
        }

        public IEnumerable<T> ExecuteOrs(string collectionName, IEnumerable<IMongoQuery> mongoQueries)
        {
            return _reader.ExecuteOrs<T>(collectionName, mongoQueries);
        }
    }
}
