using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public delegate void WriteCompletedEvent(object sender);

    public interface IWriterAsync
    {
        event WriteCompletedEvent AsyncWriteCompleted;

        void WriteAsync<T>(string collectionName, T entry);
    }
}
