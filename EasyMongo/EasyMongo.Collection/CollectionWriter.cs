using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Collection
{
    public class CollectionWriter : ICollectionWriter
    {
        IDatabaseWriter _databaseWriter;
        string _collectionName; 

        public CollectionWriter(IDatabaseWriter databaseWriter, string collectionName)
        {
            _databaseWriter = databaseWriter;
            _collectionName = collectionName;
        }

        #region    Synchronous
        public void Write<T>(T entry)
        {
            _databaseWriter.Write(_collectionName, entry);
        }
        #endregion Synchronous

        #region    Asynchronous
        public Task WriteAsync<T>(T entry)
        {
            return _databaseWriter.WriteAsync<T>(_collectionName, entry);
        }
        #endregion Asynchronous
    }

    public class CollectionWriter<T> : ICollectionWriter<T>
    {
        private ICollectionWriter _collectionWriter;

        public CollectionWriter(ICollectionWriter collectionWriter)
        {
            _collectionWriter = collectionWriter;
        }

        public void Write(T entry)
        {
            _collectionWriter.Write<T>(entry);
        }

        public Task WriteAsync(T entry)
        {
            return _collectionWriter.WriteAsync<T>(entry);
        }
    }
}
