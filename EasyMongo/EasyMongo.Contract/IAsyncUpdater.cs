using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Contract
{
    public interface IAsyncUpdater
    {
        Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query);
        Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern);
        Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags);
        Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        Task<FindAndModifyResult> FindAndModifyAsync<T>(string collectionName, FindAndModifyArgs findAndModifyArgs);

        Task<FindAndModifyResult> FindAndRemoveAsync<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs);
    }
}
