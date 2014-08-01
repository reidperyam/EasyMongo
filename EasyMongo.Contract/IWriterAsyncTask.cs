using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IWriterAsyncTask
    {
        void WriteAsync<T>(string collectionName, T entry);
    }
}
