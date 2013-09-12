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
        public event WriteCompletedEvent AsyncWriteCompleted;
        IDatabaseWriter<T> _mongoDBWriter;
        string _collectionName;
        //WriteConcern 

        public Writer(IDatabaseWriter<T> mongoDatabaseWriter, string collectionName)
        {
            _mongoDBWriter = mongoDatabaseWriter;
            _collectionName = collectionName;
            _mongoDBWriter.AsyncWriteCompleted += new WriteCompletedEvent(_mongoDBWriter_AsyncWriteCompleted);
        }

        void _mongoDBWriter_AsyncWriteCompleted(object sender)
        {
            if (AsyncWriteCompleted != null)
                AsyncWriteCompleted(sender);
        }

        public void Write(T entry)
        {
            _mongoDBWriter.Write(_collectionName, entry);
        }

        public void WriteAsync(T entry)
        {
            _mongoDBWriter.Write(_collectionName, entry);
        }
    }
}
