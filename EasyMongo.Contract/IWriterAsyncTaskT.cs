using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IWriterAsyncTask<T>
    {
        void WriteAsync(string collectionName, T entry);
    }
}
