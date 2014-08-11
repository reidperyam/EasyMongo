using System;
using MongoDB.Driver;
using EasyMongo.Contract.Delegates;

namespace EasyMongo.Contract
{
    public interface IDatabaseWriter: IWriter, IWriterAsync
    {
    }
}
