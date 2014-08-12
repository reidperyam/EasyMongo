using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IWriterTask<T>
    {
        Task WriteAsync(string collectionName, T entry);
    }
}
