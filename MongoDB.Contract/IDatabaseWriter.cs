using System;
using MongoDB.Driver;

namespace MongoDB.Contract
{
    public interface IDatabaseWriter<T> : IWriter<T>, IWriterAsync<T>
    {
    }
}
