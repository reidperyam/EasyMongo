using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Async
{
    public class WriterAsync : IWriterAsync
    {
        public event WriteCompletedEvent AsyncWriteCompleted;

        IWriter _mongoWriter;

        public WriterAsync(IWriter mongoWriter)
        {
            _mongoWriter = mongoWriter;
        }

        public void WriteAsync<T>(string collectionName, T entry)
        {
            new Action<string, T>(_mongoWriter.Write).BeginInvoke(collectionName, entry, Callback, null);
        }

        private void Callback(IAsyncResult asyncRes)
        {
            // TODO - have invoked async method return a result
            //
            //AsyncResult ares = (AsyncResult)asyncRes;
            //var delg = (dynamic)ares.AsyncDelegate;
            //dynamic result = delg.EndInvoke(asyncRes);

            if (AsyncWriteCompleted != null)
                AsyncWriteCompleted(asyncRes);
        }
    }

    public class WriterAsync<T> : IWriterAsync<T>
    {
        public event WriteCompletedEvent AsyncWriteCompleted
        {
            add
            {
                lock (_mongoWriterAsync)
                {
                    _mongoWriterAsync.AsyncWriteCompleted += value;
                }
            }
            remove
            {
                lock (_mongoWriterAsync)
                {
                    _mongoWriterAsync.AsyncWriteCompleted -= value;
                }
            }
        }

        IWriterAsync _mongoWriterAsync;

        public WriterAsync(IWriterAsync mongoWriterAsync)
        {
            _mongoWriterAsync = mongoWriterAsync;
        }

        public void WriteAsync(string collectionName, T entry)
        {
            _mongoWriterAsync.WriteAsync<T>(collectionName, entry);
        }
    }
}
