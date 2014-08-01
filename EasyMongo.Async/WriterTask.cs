using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using EasyMongo.Contract;
using MongoDB.Driver;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Async
{
    public class WriterTask : IWriterTask
    {
        IWriter _writer;

        public WriterTask(IWriter writer)
        {
            _writer = writer;
        }

        public async void WriteAsync<T>(string collectionName, T entry)
        {
            await Task.Run(() => { _writer.Write<T>(collectionName, entry); });
        }
    }

    public class WriterTask<T> : IWriterTask<T>
    {
        IWriterAsync _writerAsync;

        public WriterTask(IWriterAsync writerAsync)
        {
            _writerAsync = writerAsync;
        }

        public async void WriteAsync(string collectionName, T entry)
        {
            await Task.Run(() => { _writerAsync.WriteAsync<T>(collectionName, entry); });
        }
    }
}
