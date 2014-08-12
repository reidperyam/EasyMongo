using System;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IDatabaseWriter<T>: IWriter<T>, IWriterTask<T>
    {
    }
}
