using System;
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
    public class Reader<T> : IReader<T> where T : IEasyMongoEntry
    {
        IDatabaseConnection<T> _mongoDatabaseConnection;

        public Reader(IDatabaseConnection<T> mongoDatabaseConnection)
        {
            _mongoDatabaseConnection = mongoDatabaseConnection;
            //_mongoDatabaseConnection.ConnectAsyncCompleted += new ConnectAsyncCompletedEvent(_mongoDatabaseConnection_Connected);
        }

        #region    Methods
        #region Read Implementation methods
        private void Find(string collectionName, string dateTimeFieldName, DateTime start, DateTime end, out List<T> results)
        {
            results = new List<T>();
            var searchQuery = Query.And(Query.GTE(dateTimeFieldName, start), Query.LTE(dateTimeFieldName, end));
            var collection = _mongoDatabaseConnection.GetCollection(collectionName);
            results.AddRange(collection.Find(searchQuery));
        }
        private void Find(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end, out List<T> results)
        {
            results = new List<T>();
            var searchQuery = Query.And(Query.GTE(dateTimeFieldName, start), Query.LTE(dateTimeFieldName, end), Query.Matches(fieldName, new BsonRegularExpression(regexPattern)));
            var collection = _mongoDatabaseConnection.GetCollection(collectionName);
            results.AddRange(collection.Find(searchQuery));
        }
        private void Find(string collectionName, string fieldName, string regexPattern, out List<T> results)
        {
            results = new List<T>();
            var searchQuery = Query.Matches(fieldName, new BsonRegularExpression(regexPattern));
            var collection = _mongoDatabaseConnection.GetCollection(collectionName);
            results.AddRange(collection.Find(searchQuery));
        }
        private void Find(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end, out List<T> results)
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
        private void Find(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end, out List<T> results)
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
        private void Find(IEnumerable<string> collectionNames, string fieldName, string regexPattern, out List<T> results)
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
        #endregion Read Implementation methods
        #region   Read Against a Collection
        /// <summary>
        /// Synchronously searches against a MongoDB collection
        /// </summary>
        /// <remarks>Proxy method to private implementation method CollectionDateRangeRead</remarks>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumberable<T> of read results from the MongoDB</returns>
        public IEnumerable<T> Read(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            List<T> results;
            Find(collectionName, dateTimeFieldName, start, end, out results);
            return results;
        }
        /// <summary>
        /// Synchronously searches against a MongoDB collection
        /// </summary>
        /// <remarks>Proxy method to private implementation method CollectionDateRangePropertyRead</remarks>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumberable<T> of read results from the MongoDB</returns>
        public IEnumerable<T> Read(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            List<T> results;
            Find(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end, out results);
            return results;
        }
        /// <summary>
        /// Synchronously searches against a MongoDB collection
        /// </summary>
        /// <remarks>Proxy method to private implementation method CollectionPropertyRead</remarks>
        /// <param name="collectionName">>The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <returns>IEnumberable<T> of read results from the MongoDB</returns>
        public IEnumerable<T> Read(string collectionName, string fieldName, string regexPattern)
        {
            List<T> results;
            Find(collectionName, fieldName, regexPattern, out results);
            return results;
        }
        #endregion Read Against a Collection
        #region   Read Against Multiple Collections
        /// <summary>
        /// Synchronously searches against multiple MongoDB collections
        /// </summary>
        /// <remarks>Proxy method to private implementation method DateRangeRead</remarks>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumberable<T> of read results from the MongoDB</returns>
        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            List<T> results;
            Find(collectionNames, dateTimeFieldName, start, end, out results);
            return results;
        }
        /// <summary>
        /// Synchronously searches against multiple MongoDB collections
        /// </summary>
        /// <remarks>Proxy method to private implementation method DateRangePropertyRead</remarks>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumberable<T> of read results from the MongoDB</returns>
        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            List<T> results;
            Find(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end, out results);
            return results;
        }
        /// <summary>
        /// Synchronously searches against multiple MongoDB collections
        /// </summary>
        /// <remarks>Proxy method to private implementation method PropertyRead</remarks>
        /// <param name="collectionName">>The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <returns>IEnumberable<T> of read results from the MongoDB</returns>
        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            List<T> results;
            Find(collectionNames, fieldName, regexPattern, out results);
            return results;
        }
        #endregion Read Against Multiple Collections

        #region    Distinct Implementation
        private void Distinct(string collectionName, string fieldName, out List<BsonValue> results)
        {
            results = new List<BsonValue>();
            var collection = _mongoDatabaseConnection.GetCollection(collectionName);
            results.AddRange(collection.Distinct(fieldName));
        }
        private void Distinct(string collectionName, string fieldName, IMongoQuery query, out List<BsonValue> results)
        {
            results = new List<BsonValue>();
            var collection = _mongoDatabaseConnection.GetCollection(collectionName);
            results.AddRange(collection.Distinct(fieldName, query));
        }
        private void Distinct(IEnumerable<string> collectionNames, string fieldName, out List<BsonValue> results)
        {
            results = new List<BsonValue>();
            List<BsonValue> resultsForCollection;

            foreach (string collectionName in collectionNames)
            {
                Distinct(collectionName, fieldName, out resultsForCollection);

                // since this method returns distinct values and we are searching across collections
                // we need to cull values returned from other collections
                foreach (string result in resultsForCollection)
                {
                    if(!results.Contains(result))
                        results.Add(result);
                }
                
                resultsForCollection.Clear();
            }
        }
        private void Distinct(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query, out List<BsonValue> results)
        {
            results = new List<BsonValue>();
            List<BsonValue> resultsForCollection;

            foreach (string collectionName in collectionNames)
            {
                Distinct(collectionName, fieldName, query, out resultsForCollection);

                // since this method returns distinct values and we are searching across collections
                // we need to cull values returned from other collections
                foreach (string result in resultsForCollection)
                {
                    if (!results.Contains(result))
                        results.Add(result);
                }
                resultsForCollection.Clear();
            }
        }
        #endregion Distinct Implementation
        #region Distinct Across Collection
        public IEnumerable<BsonValue> Distinct(string collectionName, string fieldName)
        {
            List<BsonValue> results;
            Distinct(collectionName, fieldName, out results);
            return results;
        }
        public IEnumerable<BsonValue> Distinct(string collectionName, string fieldName, IMongoQuery query)
        {
            List<BsonValue> results;
            Distinct(collectionName, fieldName, query, out results);
            return results;
        }
        #endregion Distinct Across Collection
        #region Distinct Across Multiple Collections
        public IEnumerable<BsonValue> Distinct(IEnumerable<string> collectionNames, string fieldName)
        {
            List<BsonValue> results;
            Distinct(collectionNames, fieldName, out results);
            return results;
        }
        public IEnumerable<BsonValue> Distinct(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            List<BsonValue> results;
            Distinct(collectionNames, fieldName, query, out results);
            return results;
        }
        #endregion Distinct Across Multiple Collections
        #endregion Methods

        public IReader<T> Create(IDatabaseConnection<T> databaseConnection)
        {
            return new Reader<T>(databaseConnection);
        }

        // ** TODO - implement this inside a class that is embedded into users
        //    TODO - implment IOC of this class into ctor 

        private System.Threading.AutoResetEvent _resetEvent = new System.Threading.AutoResetEvent(false);

        private void VerifyUnderlyingConnection()
        {
            switch (_mongoDatabaseConnection.ConnectionState)
            {
                case ConnectionState.Connected: break;
                case ConnectionState.Connecting: _resetEvent.WaitOne(); break;
                case ConnectionState.NotConnected: throw new MongoDatabaseConnectionException("Database not connected");
            }

            if (!_mongoDatabaseConnection.CanConnect())
                throw new MongoDatabaseConnectionException("Cannot connect to database");
        }

        private void _mongoDatabaseConnection_Connected(ConnectionResult result)
        {
            // this event gets called when the DatabaseConnection successfully, asynchronously connects to the MongoDB server

            if (result == ConnectionResult.Success)
            {
                // do work
            }
            else
            {
                return;
            }
            _resetEvent.Set();
        }
    }
}
