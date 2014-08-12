using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using EasyMongo.Contract.Delegates;

namespace EasyMongo.Contract
{
    public interface ICollectionWriter<T>
    {
        void Write(T entry);
        Task WriteAsync(T entry);
    }
}
