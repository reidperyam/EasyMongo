using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Async;

namespace EasyMongo.Database
{
    public class DatabaseWriter : Adapter, IDatabaseWriter
    {
        public event WriteCompletedEvent AsyncWriteCompleted;

        public DatabaseWriter(string        connectionString, 
                              string        databaseName,
                              IReader       reader, 
                              IWriter       writer, 
                              IUpdater      updater,
                              IReaderAsync  readerAsync, 
                              IWriterAsync  writerAsync,
                              IUpdaterAsync updaterAsync)
            : base(reader,
                   writer,
                   updater,
                   readerAsync,
                   writerAsync,
                   updaterAsync)
        {
            _mongoWriterAsync.AsyncWriteCompleted += new WriteCompletedEvent(_mongoWriterAsync_WriteCompleted);
        }

        public void Write<T>(string collectionName, T entry)
        {
            _mongoWriter.Write<T>(collectionName, entry);
        }

        public void WriteAsync<T>(string collectionName, T entry)
        {
            _mongoWriterAsync.WriteAsync<T>(collectionName, entry);
        }

        void _mongoWriterAsync_WriteCompleted(object sender)
        {
            if (AsyncWriteCompleted != null)
                AsyncWriteCompleted(sender);
        }
    }
}
