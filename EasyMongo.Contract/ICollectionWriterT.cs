using System;
using MongoDB.Driver;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Contract
{
    public interface ICollectionWriter<T>
    {
        event WriteCompletedEvent AsyncWriteCompleted;

        void Write(T entry);
        void WriteAsync(T entry);
    }
}
