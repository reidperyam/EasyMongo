using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using MongoDB.Contract;
using MongoDB.Driver;

namespace MongoDB
{
    public class WriterAsync<T> : IWriterAsync<T> where T : EntryBase
    {
        public event WriteCompletedEvent AsyncWriteCompleted;

        IWriter<T> _mongoWriter;

        public WriterAsync(IWriter<T> mongoWriter)
        {
            _mongoWriter = mongoWriter;
        }

        public void WriteAsync(string collectionName, T entry)
        {
            new Action<string, T>(_mongoWriter.Write).BeginInvoke(collectionName, entry, Callback, null);
        }

        public IWriterAsync<T> Create(IWriter<T> writer)
        {
            return new WriterAsync<T>(writer);
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
}
