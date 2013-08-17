using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Database;
using MongoDB.Contract;

namespace MongoDB.Database
{
    public class Writer<T> : Adapter<T>, IDatabaseWriter<T> where T : EntryBase
    {
        public event WriteCompletedEvent AsyncWriteCompleted;

        public Writer(string connectionString, string databaseName)
            : base(new ServerConnection(connectionString), databaseName)
        {
            _mongoWriterAsync.AsyncWriteCompleted += new WriteCompletedEvent(_mongoWriterAsync_WriteCompleted);
        }

        public void Write(string collectionName, T entry)
        {
            _mongoWriter.Write(collectionName, entry);
        }

        public void WriteAsync(string collectionName, T entry)
        {
            _mongoWriterAsync.WriteAsync(collectionName, entry);
        }

        public IWriter<T> Create(IDatabaseConnection<T> databaseConnection)
        {
            return new MongoDB.Writer<T>(databaseConnection);
        }

        public IWriterAsync<T> Create(IWriter<T> writer)
        {
            return new WriterAsync<T>(writer);
        }

        void _mongoWriterAsync_WriteCompleted(object sender)
        {
            if (AsyncWriteCompleted != null)
                AsyncWriteCompleted(sender);
        }
    }
}
