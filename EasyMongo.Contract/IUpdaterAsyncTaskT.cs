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
    public interface IUpdaterAsyncTask<T>
    {
        Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query);
        Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern);
        Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags);
        Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        Task<FindAndModifyResult> FindAndModifyAsync(string collectionName, FindAndModifyArgs findAndModifyArgs);

        Task<FindAndModifyResult> FindAndRemoveAsync(string collectionName, FindAndRemoveArgs findAndRemoveArgs);
    }
}
