using System;
using MongoDB.Driver;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Contract
{
    public interface IDatabaseWriter<T>: IWriter<T>, IWriterAsync<T>
    {
    }
}
