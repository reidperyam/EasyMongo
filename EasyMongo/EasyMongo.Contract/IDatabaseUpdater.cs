using System;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IDatabaseUpdater : IUpdater, IAsyncUpdater
    {
    }
}
