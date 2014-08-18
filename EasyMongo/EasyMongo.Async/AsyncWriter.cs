using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Async
{
    public class AsyncWriter : IAsyncWriter
    {
        IWriter _writer;

        public AsyncWriter(IWriter writer)
        {
            _writer = writer;
        }

        public Task WriteAsync<T>(string collectionName, T entry)
        {
            return Task.Run(() => { _writer.Write<T>(collectionName, entry); });
        }
    }

    public class WriterTask<T> : IAsyncWriter<T>
    {
        IAsyncWriter _writerTask;

        public WriterTask(IAsyncWriter writerTask)
        {
            _writerTask = writerTask;
        }

        public Task WriteAsync(string collectionName, T entry)
        {
            return Task.Run(() => { _writerTask.WriteAsync<T>(collectionName, entry); });
        }
    }
}
