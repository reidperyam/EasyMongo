using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface ICollectionWriter
    {
        void Write<T>(T entry);
        Task WriteAsync<T>(T entry);
    }
}
