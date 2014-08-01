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

        IWriter _writer;

        public WriterAsync(IWriter writer)
        {
            _writer = writer;
        }

        public void WriteAsync<T>(string collectionName, T entry)
        {
            new Action<string, T>(_writer.Write).BeginInvoke(collectionName, entry, Callback, null);
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
                lock (_writerAsync)
                {
                    _writerAsync.AsyncWriteCompleted += value;
                }
            }
            remove
            {
                lock (_writerAsync)
                {
                    _writerAsync.AsyncWriteCompleted -= value;
                }
            }
        }

        IWriterAsync _writerAsync;

        public WriterAsync(IWriterAsync writerAsync)
        {
            _writerAsync = writerAsync;
        }

        public void WriteAsync(string collectionName, T entry)
        {
            _writerAsync.WriteAsync<T>(collectionName, entry);
        }
    }
}
