using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Async
{
    public class WriterAsyncTask : IWriterAsyncTask
    {
        IWriter _writer;

        public WriterAsyncTask(IWriter writer)
        {
            _writer = writer;
        }

        public async void WriteAsync<T>(string collectionName, T entry)
        {
            await Task.Run(() => { _writer.Write<T>(collectionName, entry); });
        }
    }

    public class WriterAsyncTask<T> : IWriterAsyncTask<T>
    {
        IWriterAsync _writerAsync;

        public WriterAsyncTask(IWriterAsync writerAsync)
        {
            _writerAsync = writerAsync;
        }

        public async void WriteAsync(string collectionName, T entry)
        {
            await Task.Run(() => { _writerAsync.WriteAsync<T>(collectionName, entry); });
        }
    }
}
