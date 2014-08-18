using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface ICollectionUpdater<T>
    {
        // Are the WriteConcern overloads required or are they defined in embedded objects?
        WriteConcernResult Remove(IMongoQuery query);
        WriteConcernResult Remove(IMongoQuery query, WriteConcern writeConcern);
        WriteConcernResult Remove(IMongoQuery query, RemoveFlags removeFlags);
        WriteConcernResult Remove(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        Task<WriteConcernResult> RemoveAsync(IMongoQuery query);
        Task<WriteConcernResult> RemoveAsync(IMongoQuery query, WriteConcern writeConcern);
        Task<WriteConcernResult> RemoveAsync(IMongoQuery query, RemoveFlags removeFlags);
        Task<WriteConcernResult> RemoveAsync(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        FindAndModifyResult FindAndModify(FindAndModifyArgs findAndModifyArgs);

        FindAndModifyResult FindAndRemove(FindAndRemoveArgs findAndRemoveArgs);

        Task<FindAndModifyResult> FindAndModifyAsync(FindAndModifyArgs findAndModifyArgs);

        Task<FindAndModifyResult> FindAndRemoveAsync(FindAndRemoveArgs findAndRemoveArgs);
    }
}
