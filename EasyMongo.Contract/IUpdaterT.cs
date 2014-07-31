using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IUpdater<T>
    {
        WriteConcernResult Remove(string collectionName, IMongoQuery query);
        WriteConcernResult Remove(string collectionName, IMongoQuery query, WriteConcern writeConcern);
        WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags);
        WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        FindAndModifyResult FindAndModify(string collectionName, FindAndModifyArgs findAndModifyArgs);

        FindAndModifyResult FindAndRemove(string collectionName, FindAndRemoveArgs findAndRemoveArgs);
    }
}
