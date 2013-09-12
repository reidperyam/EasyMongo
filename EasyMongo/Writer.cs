
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo
{
    public class Writer<T> : IWriter<T> where T : IEasyMongoEntry
    {
        private IDatabaseConnection<T> _databaseConnection;

        public Writer(IDatabaseConnection<T> databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public void Write(string collectionName, T entry)
        {
            var collection = _databaseConnection.GetCollection(collectionName);
            collection.Save(entry);
        }

        public IWriter<T> Create(IDatabaseConnection<T> databaseConnection)
        {
            return new Writer<T>(databaseConnection);
        }

        private MongoCollection<T> GetCollection(string collectionName)
        {
            return _databaseConnection.GetCollection(collectionName);
        }
    }
}
