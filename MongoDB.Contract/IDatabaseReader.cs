using System;
using System.Collections.Generic;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB.Contract
{
    public interface IDatabaseReader<T> : IReader<T>, IReaderAsync<T>
    {
    }
}
