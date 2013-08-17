using System;
using MongoDB.Driver;

namespace MongoDB.Contract
{
    public interface IDatabaseUpdater<T> : IUpdater<T>, IUpdaterAsync<T>
    {
    }
}
