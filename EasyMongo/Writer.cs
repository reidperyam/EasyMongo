
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo
{
    public class Writer : IWriter
    {
        private IDatabaseConnection _databaseConnection;

        public Writer(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public void Write<T>(string collectionName, T entry)
        {
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            collection.Save(entry);
        }

        private MongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _databaseConnection.GetCollection<T>(collectionName);
        }
    }

    public class Writer<T> : IWriter<T>
    {
        private IDatabaseConnection _databaseConnection;

        public Writer(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public void Write(string collectionName, T entry)
        {
            var collection = _databaseConnection.GetCollection<T>(collectionName);
            collection.Save(entry);
        }

        private MongoCollection<T> GetCollection(string collectionName)
        {
            return _databaseConnection.GetCollection<T>(collectionName);
        }
    }
}
