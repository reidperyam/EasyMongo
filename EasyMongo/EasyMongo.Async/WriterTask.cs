using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Async
{
    public class WriterTask : IWriterTask
    {
        IWriter _writer;

        public WriterTask(IWriter writer)
        {
            _writer = writer;
        }

        public Task WriteAsync<T>(string collectionName, T entry)
        {
            return Task.Run(() => { _writer.Write<T>(collectionName, entry); });
        }
    }

    public class WriterTask<T> : IWriterTask<T>
    {
        IWriterTask _writerTask;

        public WriterTask(IWriterTask writerTask)
        {
            _writerTask = writerTask;
        }

        public Task WriteAsync(string collectionName, T entry)
        {
            return Task.Run(() => { _writerTask.WriteAsync<T>(collectionName, entry); });
        }
    }
}
