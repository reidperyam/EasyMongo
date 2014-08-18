using System;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IDatabaseWriter: IWriter, IWriterTask
    {
    }
}
