using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IUpdater
    {
        WriteConcernResult Remove<T>(string collectionName, IMongoQuery query);
        WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern);
        WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags);
        WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        FindAndModifyResult FindAndModify<T>(string collectionName, FindAndModifyArgs findAndModifyArgs);
 
        FindAndModifyResult FindAndRemove<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs);
    }
}
