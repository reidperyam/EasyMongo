using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo.Database;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Collection
{
    public class Writer<T> : ICollectionWriter<T> where T : IEasyMongoEntry
    {
        IDatabaseWriter<T> _mongoDBWriter;
        string _collectionName; 

        public Writer(IDatabaseWriter<T> mongoDatabaseWriter, string collectionName)
        {
            _mongoDBWriter = mongoDatabaseWriter;
            _collectionName = collectionName;
        }

        #region    Synchronous
        public void Write(T entry)
        {
            _mongoDBWriter.Write(_collectionName, entry);
        }
        #endregion Synchronous

        #region    Asynchronous
        public void WriteAsync(T entry)
        {
            _mongoDBWriter.Write(_collectionName, entry);
        }
        #endregion Asynchronous
    }
}
