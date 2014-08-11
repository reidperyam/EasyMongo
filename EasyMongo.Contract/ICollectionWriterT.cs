using System;
using MongoDB.Driver;
using EasyMongo.Contract.Delegates;

namespace EasyMongo.Contract
{
    public interface ICollectionWriter<T>
    {
        event WriteCompletedEvent AsyncWriteCompleted;

        void Write(T entry);
        void WriteAsync(T entry);
    }
}
