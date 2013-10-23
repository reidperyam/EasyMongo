using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Database
{
    public class DatabaseWriter : IDatabaseWriter
    {
        public event WriteCompletedEvent AsyncWriteCompleted;

        protected IWriter _mongoWriter;

        protected IWriterAsync _mongoWriterAsync;

        public DatabaseWriter(IWriter       writer,
                              IWriterAsync  writerAsync)
        {
            _mongoWriter = writer;
            _mongoWriterAsync = writerAsync;

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
