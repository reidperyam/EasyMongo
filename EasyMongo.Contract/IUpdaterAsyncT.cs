using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Contract
{
    [Obsolete("This interface is obselete")]
    public interface IUpdaterAsync<T>
    {
        event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        void RemoveAsync(string collectionName, IMongoQuery query);
        void RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern);
        void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags);
        void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        void FindAndModifyAsync(string collectionName, FindAndModifyArgs findAndModifyArgs);

        void FindAndRemoveAsync(string collectionName, FindAndRemoveArgs findAndRemoveArgs);
    }
}
