using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface ICollectionUpdater
    {
        // Are the WriteConcern overloads required or are they defined in embedded objects?
        WriteConcernResult Remove<T>(IMongoQuery query);
        WriteConcernResult Remove<T>(IMongoQuery query, WriteConcern writeConcern);
        WriteConcernResult Remove<T>(IMongoQuery query, RemoveFlags removeFlags);
        WriteConcernResult Remove<T>(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        Task<WriteConcernResult> RemoveAsync<T>(IMongoQuery query);
        Task<WriteConcernResult> RemoveAsync<T>(IMongoQuery query, WriteConcern writeConcern);
        Task<WriteConcernResult> RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags);
        Task<WriteConcernResult> RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        FindAndModifyResult FindAndModify<T>(FindAndModifyArgs findAndModifyArgs);
        FindAndModifyResult FindAndRemove<T>(FindAndRemoveArgs findAndRemoveArgs);

        Task<FindAndModifyResult> FindAndModifyAsync<T>(FindAndModifyArgs findAndModifyArgs);

        Task<FindAndModifyResult> FindAndRemoveAsync<T>(FindAndRemoveArgs findAndRemoveArgs);
    }
}
