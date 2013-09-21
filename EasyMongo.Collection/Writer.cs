using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo.Database;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Collection
{
    public class Writer : ICollectionWriter
    {
        public event WriteCompletedEvent AsyncWriteCompleted;

        IDatabaseWriter _mongoDBWriter;
        string _collectionName; 

        public Writer(IDatabaseWriter mongoDatabaseWriter, string collectionName)
        {
            _mongoDBWriter = mongoDatabaseWriter;
            _collectionName = collectionName;

            _mongoDBWriter.AsyncWriteCompleted += new WriteCompletedEvent(_mongoDBWriter_AsyncWriteCompleted);
        }

        #region    Synchronous
        public void Write<T>(T entry)
        {
            _mongoDBWriter.Write(_collectionName, entry);
        }
        #endregion Synchronous

        #region    Asynchronous
        public void WriteAsync<T>(T entry)
        {
            _mongoDBWriter.Write<T>(_collectionName, entry);
        }
        #endregion Asynchronous

        void _mongoDBWriter_AsyncWriteCompleted(object sender)
        {
            if (AsyncWriteCompleted != null)
                AsyncWriteCompleted(sender);
        }
    }
}
