using System;
using System.Collections.Generic;
using EasyMongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IDatabaseReader<T> : IReader<T>, IReaderAsync<T>
    {
    }
}
