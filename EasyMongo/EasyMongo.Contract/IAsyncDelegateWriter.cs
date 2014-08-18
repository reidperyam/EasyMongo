using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public delegate void WriteCompletedEvent(object sender);

    public interface IAsyncDelegateWriter
    {
        event WriteCompletedEvent AsyncWriteCompleted;

        void WriteAsync<T>(string collectionName, T entry);
    }
}
