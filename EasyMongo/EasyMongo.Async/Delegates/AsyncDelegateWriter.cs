using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Async.Delegates
{
    public class AsyncDelegateWriter : IAsyncDelegateWriter
    {
        public event WriteCompletedEvent AsyncWriteCompleted;

        IWriter _writer;

        public AsyncDelegateWriter(IWriter writer)
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

    public class AsyncDelegateWriter<T> : IAsyncDelegateWriter<T>
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

        IAsyncDelegateWriter _writerAsync;

        public AsyncDelegateWriter(IAsyncDelegateWriter writerAsync)
        {
            _writerAsync = writerAsync;
        }

        public void WriteAsync(string collectionName, T entry)
        {
            _writerAsync.WriteAsync<T>(collectionName, entry);
        }
    }
}
