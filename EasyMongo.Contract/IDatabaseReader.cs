using System;
using System.Collections.Generic;
using EasyMongo;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IDatabaseReader : IReader, IReaderAsync
    {
    }
}
