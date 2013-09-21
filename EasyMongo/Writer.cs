
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

        public IWriter Create(IDatabaseConnection databaseConnection)
        {
            return new Writer(databaseConnection);
        }

        private MongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _databaseConnection.GetCollection<T>(collectionName);
        }
    }
}
