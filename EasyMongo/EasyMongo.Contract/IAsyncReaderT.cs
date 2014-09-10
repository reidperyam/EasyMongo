using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IAsyncReader<T>
    {
        /// <summary>
        /// Synchronously searches against multiple MongoDB collections
        /// </summary>
        /// <param name="collectionNames">The MongoDB Collections to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern);
        /// <summary>
        /// Asynchronously searches against a MongoDB collection
        /// </summary>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        Task<IEnumerable<T>> ReadAsync(string collectionName, string dateTimeFieldName, DateTime start, DateTime end);
        /// <summary>
        /// Asynchronously searches against a MongoDB collection
        /// </summary>
        /// <param name="collectionName">>The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>       
        Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        /// <summary>
        /// Asynchronously searches against multiple MongoDB collections
        /// </summary>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern);
        /// <summary>
        /// Asynchronously searches against multiple MongoDB collections
        /// </summary>
        /// <param name="collectionNames">The MongoDB Collectionw to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end);
        /// <summary>
        /// Asynchronously searches against multiple MongoDB collections
        /// </summary>
        /// <param name="collectionNames">The MongoDB Collections to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>      
        Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        /// <summary>
        /// Synchronously searches against a collection and returns distinct set of argument fieldName values
        /// </summary>
        /// <typeparam name="Y">The type of the record (corresponding to argument fieldName) that will be returned</typeparam>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <returns>IEnumerable of destinct values for the argument fieldName</returns>
        Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName);
        /// <summary>
        /// Synchronously searches against a collection and returns distinct set of argument fieldName values for the corresponding query
        /// </summary>
        /// <typeparam name="Y">The type of the record (corresponding to argument fieldName) that will be returned</typeparam>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="query">The MongoDB query to be used against the argument field (see QueryDocument and QueryBuilder)</param>
        /// <returns>IEnumerable of destinct values for the argument fieldName and query</returns>
        Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName, IMongoQuery query);
        /// <summary>
        /// Synchronously searches against multiple collections and returns distinct set of argument fieldName values
        /// </summary>
        /// <typeparam name="Y">The type of the record (corresponding to argument fieldName) that will be returned</typeparam>
        /// <param name="collectionNames">The MongoDB Collections to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <returns>IEnumerable of destinct values for the argument fieldName</returns>
        Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName);
        /// <summary>
        /// Synchronously searches against multiple collections and returns distinct set of argument fieldName values for the corresponding query
        /// </summary>
        /// <typeparam name="Y">The type of the record (corresponding to argument fieldName) that will be returned</typeparam>
        /// <param name="collectionNames">The MongoDB Collections to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="query">The MongoDB query to be used against the argument field (see QueryDocument and QueryBuilder)</param>
        /// <returns>IEnumerable of destinct values for the argument fieldName and query</returns>
        Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query);
    }
}
