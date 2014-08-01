using System;
using MongoDB.Driver;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Contract
{
    public interface IDatabaseUpdater : IUpdater, IUpdaterAsync
    {
    }
}
