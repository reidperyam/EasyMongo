using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo;
using EasyMongo.Async;
using EasyMongo.Contract;
using EasyMongo.Database;
using EasyMongo.Collection;
using Ninject;
using Ninject.Modules;
using Ninject.Syntax;

using EasyMongo.Test.Model;

namespace Ninject.Extensions.EasyMongo
{
    class EasyMongoNinjectIntegrationTestModule : NinjectModule
    {
        /// <summary>
        /// Contains bindings and constructor arguments used by the IntegrationTest TestFixture
        /// </summary>
        public override void Load()
        {
            const string CONNECTION_STRING = "mongodb://localhost";// "mongodb://67.190.39.219";//"mongodb://localhost";
            const string DATABASE_NAME = "TEST_DB_1";
            const string COLLECTION_NAME = "MONGO_READER_TESTS";

            Bind<IEasyMongoEntry>().To<TestEntry>();



            Bind(typeof(IDatabaseConnection<IEasyMongoEntry>)).To(typeof(DatabaseConnection<TestEntry>)).WithConstructorArgument("databaseName", DATABASE_NAME);
           // Bind(typeof(IDatabaseConnection<>)).To(typeof(DatabaseConnection<>)).WithConstructorArgument("databaseName", DATABASE_NAME);
            //Bind(typeof(IDatabaseConnection<>)).To<DatabaseConnection<TestEntry>>().WithConstructorArgument("databaseName", DATABASE_NAME);
            //Bind(typeof(IDatabaseConnection<>)).To<DatabaseConnection<IEasyMongoEntry>>().WithConstructorArgument("databaseName", DATABASE_NAME);
            //Bind(typeof(IDatabaseConnection<>)).To(typeof(DatabaseConnection<TestEntry>)).WithConstructorArgument("databaseName", DATABASE_NAME);
            //Bind(typeof(IDatabaseConnection<>)).To(typeof(DatabaseConnection<IEasyMongoEntry>)).WithConstructorArgument("databaseName", DATABASE_NAME);
            //Bind<IDatabaseConnection<IEasyMongoEntry>>().To<DatabaseConnection<IEasyMongoEntry>>().WithConstructorArgument("databaseName", DATABASE_NAME);

            // TODO - reference these static variables from the TestFixture to which they relate...
            Bind<IServerConnection>().ToConstant<ServerConnection>(new ServerConnection(CONNECTION_STRING));
           // Bind<IDatabaseConnection<IEasyMongoEntry>>().To(typeof(DatabaseConnection<IEasyMongoEntry>)).WithConstructorArgument("databaseName", DATABASE_NAME);

            Bind(typeof(IReader<>)).To(typeof(global::EasyMongo.Reader<>));
            Bind(typeof(IWriter<>)).To(typeof(global::EasyMongo.Writer<>));
            Bind(typeof(IUpdater<>)).To(typeof(global::EasyMongo.Updater<>));

            Bind(typeof(IReaderAsync<>)).To(typeof(global::EasyMongo.Async.ReaderAsync<>));
            Bind(typeof(IWriterAsync<>)).To(typeof(global::EasyMongo.Async.WriterAsync<>));
            Bind(typeof(IUpdaterAsync<>)).To(typeof(global::EasyMongo.Async.UpdaterAsync<>));

            // bind our database r/w/u to an implementation pointing to our test server and database
            Bind(typeof(IDatabaseReader<>)).To(typeof(global::EasyMongo.Database.Reader<>)).WithConstructorArgument("connectionString", CONNECTION_STRING).WithConstructorArgument("databaseName", DATABASE_NAME);
            Bind(typeof(IDatabaseWriter<>)).To(typeof(global::EasyMongo.Database.Writer<>)).WithConstructorArgument("connectionString", CONNECTION_STRING).WithConstructorArgument("databaseName", DATABASE_NAME);
            Bind(typeof(IDatabaseUpdater<>)).To(typeof(global::EasyMongo.Database.Updater<>)).WithConstructorArgument("connectionString", CONNECTION_STRING).WithConstructorArgument("databaseName", DATABASE_NAME);

            // bind our collection r/w/u our test collection
            Bind(typeof(ICollectionReader<>)).To(typeof(global::EasyMongo.Collection.Reader<>)).WithConstructorArgument("collectionName", COLLECTION_NAME);
            Bind(typeof(ICollectionWriter<>)).To(typeof(global::EasyMongo.Collection.Writer<>)).WithConstructorArgument("collectionName", COLLECTION_NAME);
            Bind(typeof(ICollectionUpdater<>)).To(typeof(global::EasyMongo.Collection.Updater<>)).WithConstructorArgument("collectionName", COLLECTION_NAME);
        }
    }
}
