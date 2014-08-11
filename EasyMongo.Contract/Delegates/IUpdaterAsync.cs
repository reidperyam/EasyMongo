using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Contract.Delegates
{
    public delegate void FindAndRemoveCompletedEvent(WriteConcernResult result);
    public delegate void FindAndModifyCompletedEvent(FindAndModifyResult result);

    [Obsolete("This interface is obselete")]
    public interface IUpdaterAsync
    {
        event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        void RemoveAsync<T>(string collectionName, IMongoQuery query);
        void RemoveAsync<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern);
        void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags);
        void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        void FindAndModifyAsync<T>(string collectionName, FindAndModifyArgs findAndModifyArgs);

        void FindAndRemoveAsync<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs);
    }
}
