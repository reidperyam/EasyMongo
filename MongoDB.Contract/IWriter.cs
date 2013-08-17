using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB.Contract
{
    public interface IWriter<T>
    {
        void Write(string collectionName, T entry);

        IWriter<T> Create(IDatabaseConnection<T> databaseConnection);

        //void FindAndReplace();
    }
}
