using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Contract.Delegates;

namespace EasyMongo.Database
{
    public class DatabaseWriter : IDatabaseWriter
    {
        protected IWriter _mongoWriter;

        protected IWriterTask _mongoWriterTask;

        public DatabaseWriter(IWriter      writer,
                              IWriterTask writerTask)
        {
            _mongoWriter = writer;
            _mongoWriterTask = writerTask;
        }

        public void Write<T>(string collectionName, T entry)
        {
            _mongoWriter.Write<T>(collectionName, entry);
        }

        public Task WriteAsync<T>(string collectionName, T entry)
        {
            return _mongoWriterTask.WriteAsync<T>(collectionName, entry);
        }
    }

    public class DatabaseWriter<T> : IDatabaseWriter<T>
    {
        IDatabaseWriter _databaseWriter;

        public DatabaseWriter(IDatabaseWriter databaseWriter)
        {
            _databaseWriter = databaseWriter;
        }

        public void Write(string collectionName, T entry)
        {
            _databaseWriter.Write<T>(collectionName, entry);
        }

        public Task WriteAsync(string collectionName, T entry)
        {
            return _databaseWriter.WriteAsync<T>(collectionName, entry);
        }
    }
}
