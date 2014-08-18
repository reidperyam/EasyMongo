using System;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IDatabaseUpdater<T> : IUpdater<T>, IAsyncUpdater<T>
    {
    }
}
