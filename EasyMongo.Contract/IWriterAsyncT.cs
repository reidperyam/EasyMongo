using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    [Obsolete("This interface is obselete")]
    public interface IWriterAsync<T>
    {
        event WriteCompletedEvent AsyncWriteCompleted;

        void WriteAsync(string collectionName, T entry);
    }
}
