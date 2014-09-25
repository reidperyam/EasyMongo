using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Runtime.InteropServices;

namespace EasyMongo.Contract
{
    public interface IReader
    {
        /// <summary>
        /// Synchronously returns all records in a MongoDB collection
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        IEnumerable<T> Read<T>(string collectionName);
        /// <summary>
        /// Synchronously reads from a MongoDB collection
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionName">>The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        IEnumerable<T> Read<T>(string collectionName, string fieldName, string regexPattern);
        /// <summary>
        /// Synchronously reads from a MongoDB collection
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        IEnumerable<T> Read<T>(string collectionName, string dateTimeFieldName, DateTime start, DateTime end);
        /// <summary>
        /// Synchronously reads from a MongoDB collection
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        IEnumerable<T> Read<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        /// <summary>
        /// Synchronously returns all records in multiple MongoDB collections
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionNames">The MongoDB Collections to be read from</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        IEnumerable<T> Read<T>(IEnumerable<string> collectionNames);
        /// <summary>
        /// Synchronously reads from multiple MongoDB collections
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionNames">The MongoDB Collections to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern);
        /// <summary>
        /// Synchronously reads from multiple MongoDB collections
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionNames">The MongoDB Collections to be read from</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end);
        /// <summary>
        /// Synchronously reads from multiple MongoDB collections
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="regexPattern">A string representing text to search a fieldName for. An objected with an associated match will be returned as a result</param>
        /// <param name="dateTimeFieldName">The name of the field (property) of the persisted object associated with a DateTime object</param>
        /// <param name="start">The time at which search should begin</param>
        /// <param name="end">The time at which search should end</param>
        /// <returns>IEnumerable of read results from the MongoDB</returns>
        IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        /// <summary>
        /// Synchronously reads from a collection and returns distinct set of argument fieldName values
        /// </summary>
        /// <typeparam name="T">The type of the record (corresponding to argument fieldName) that will be returned</typeparam>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <returns>IEnumerable of destinct values for the argument fieldName</returns>
        IEnumerable<T> Distinct<T>(string collectionName, string fieldName);
        /// <summary>
        /// Synchronously reads from a collection and returns distinct set of argument fieldName values for the corresponding query
        /// </summary>
        /// <typeparam name="T">The type of the record (corresponding to argument fieldName) that will be returned</typeparam>
        /// <param name="collectionName">The MongoDB Collection to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="query">The MongoDB query to be used against the argument field (see QueryDocument and QueryBuilder)</param>
        /// <returns>IEnumerable of destinct values for the argument fieldName and query</returns>
        IEnumerable<T> Distinct<T>(string collectionName, string fieldName, IMongoQuery query);
        /// <summary>
        /// Synchronously reads from multiple collections and returns distinct set of argument fieldName values
        /// </summary>
        /// <typeparam name="T">The type of the record (corresponding to argument fieldName) that will be returned</typeparam>
        /// <param name="collectionNames">The MongoDB Collections to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <returns>IEnumerable of destinct values for the argument fieldName</returns>
        IEnumerable<T> Distinct<T>(IEnumerable<string> collectionNames, string fieldName);
        /// <summary>
        /// Synchronously reads from multiple collections and returns distinct set of argument fieldName values for the corresponding query
        /// </summary>
        /// <typeparam name="T">The type of the record (corresponding to argument fieldName) that will be returned</typeparam>
        /// <param name="collectionNames">The MongoDB Collections to be read from</param>
        /// <param name="fieldName">The name of the field (property) of the persisted object that will be searched for a matching regexPattern</param>
        /// <param name="query">The MongoDB query to be used against the argument field (see QueryDocument and QueryBuilder)</param>
        /// <returns>IEnumerable of destinct values for the argument fieldName and query</returns>
        IEnumerable<T> Distinct<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query);

        /// <summary>
        /// Synchronously executes the argument IMongoQuery against the collection
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionName">The collection to query</param>
        /// <param name="mongoQuery">The IMongoQuery to execute</param>
        /// <returns>IEnumberable<T> of read results from the MongoDB</returns>
        IEnumerable<T> Execute<T>(string collectionName, IMongoQuery mongoQuery);
        /// <summary>
        /// Synchronously executes the argument IMongoQuerys against the collection as "AND"s
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionName">The collection to query</param>
        /// <param name="mongoQueries">An enumerator of IMongoQuerys to execute</param>
        /// <returns>IEnumberable<T> of read results from the MongoDB</returns>
        IEnumerable<T> ExecuteAnds<T>(string collectionName, IEnumerable<IMongoQuery> mongoQueries);
        /// <summary>
        /// Synchronously executes the argument IMongoQuerys against the collection as "OR"s
        /// </summary>
        /// <typeparam name="T">The type of the record that will be returned</typeparam>
        /// <param name="collectionName">The collection to query</param>
        /// <param name="mongoQueries">An enumerator of IMongoQuerys to execute</param>
        /// <returns>IEnumberable<T> of read results from the MongoDB</returns>
        IEnumerable<T> ExecuteOrs<T>(string collectionName, IEnumerable<IMongoQuery> mongoQueries);
    }
}
