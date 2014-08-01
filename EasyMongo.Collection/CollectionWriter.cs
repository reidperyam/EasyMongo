using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo.Contract;
using EasyMongo.Contract.Deprecated;
using MongoDB.Driver;

namespace EasyMongo.Collection
{
    public class CollectionWriter : ICollectionWriter
    {
        public event WriteCompletedEvent AsyncWriteCompleted;

        IDatabaseWriter _databaseWriter;
        string _collectionName; 

        public CollectionWriter(IDatabaseWriter databaseWriter, string collectionName)
        {
            _databaseWriter = databaseWriter;
            _collectionName = collectionName;

            _databaseWriter.AsyncWriteCompleted += new WriteCompletedEvent(_mongoDBWriter_AsyncWriteCompleted);
        }

        #region    Synchronous
        public void Write<T>(T entry)
        {
            _databaseWriter.Write(_collectionName, entry);
        }
        #endregion Synchronous

        #region    Asynchronous
        public void WriteAsync<T>(T entry)
        {
            _databaseWriter.WriteAsync<T>(_collectionName, entry);
        }
        #endregion Asynchronous

        void _mongoDBWriter_AsyncWriteCompleted(object sender)
        {
            if (AsyncWriteCompleted != null)
                AsyncWriteCompleted(sender);
        }
    }

    public class CollectionWriter<T> : ICollectionWriter<T>
    {
        public event WriteCompletedEvent AsyncWriteCompleted
        {
            add
            {
                lock (_collectionWriter)
                {
                    _collectionWriter.AsyncWriteCompleted += value;
                }
            }
            remove
            {
                lock (_collectionWriter)
                {
                    _collectionWriter.AsyncWriteCompleted -= value;
                }
            }
        }

        private ICollectionWriter _collectionWriter;

        public CollectionWriter(ICollectionWriter collectionWriter)
        {
            _collectionWriter = collectionWriter;
        }

        public void Write(T entry)
        {
            _collectionWriter.Write<T>(entry);
        }

        public void WriteAsync(T entry)
        {
            _collectionWriter.WriteAsync<T>(entry);
        }
    }
}
