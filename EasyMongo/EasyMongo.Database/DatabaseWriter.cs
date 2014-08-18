using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Database
{
    public class DatabaseWriter : IDatabaseWriter
    {
        protected IWriter _mongoWriter;

        protected IAsyncWriter _asyncWriter;

        public DatabaseWriter(IWriter      writer,
                              IAsyncWriter asyncWriter)
        {
            _mongoWriter = writer;
            _asyncWriter = asyncWriter;
        }

        public void Write<T>(string collectionName, T entry)
        {
            _mongoWriter.Write<T>(collectionName, entry);
        }

        public Task WriteAsync<T>(string collectionName, T entry)
        {
            return _asyncWriter.WriteAsync<T>(collectionName, entry);
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
