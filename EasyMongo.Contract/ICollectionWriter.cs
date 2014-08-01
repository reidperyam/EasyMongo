using System;
using MongoDB.Driver;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Contract
{
    public interface ICollectionWriter
    {
        event WriteCompletedEvent AsyncWriteCompleted;

        void Write<T>(T entry);
        void WriteAsync<T>(T entry);
    }
}
