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
    class EasyMongoExampleNinjectModule : NinjectModule
    {
        /// <summary>
        /// Contains bindings and constructor arguments used by the IntegrationTest TestFixture
        /// </summary>
        public override void Load()
        {
            const string CONNECTION_STRING = "mongodb://localhost";// "mongodb://67.190.39.219";//"mongodb://localhost";
            const string DATABASE_NAME     = "TEST_DB_1";
            const string COLLECTION_NAME   = "MONGO_READER_TESTS";

            Bind<IEasyMongoEntry>().To<TestEntry>();

            Bind<IServerConnection>().To<ServerConnection>().InSingletonScope().WithConstructorArgument("connectionString", CONNECTION_STRING);
            Bind(typeof(IDatabaseConnection)).To<DatabaseConnection>().InSingletonScope().WithConstructorArgument("databaseName", DATABASE_NAME);

            Bind(typeof(IReader)).To(typeof(Reader));
            Bind(typeof(IWriter)).To(typeof(Writer));
            Bind(typeof(IUpdater)).To(typeof(Updater));

            Bind(typeof(IReader<>)).To(typeof(Reader<>));
            Bind(typeof(IWriter<>)).To(typeof(Writer<>));
            Bind(typeof(IUpdater<>)).To(typeof(Updater<>));

            Bind(typeof(IReaderAsync)).To(typeof(ReaderAsync));
            Bind(typeof(IWriterAsync)).To(typeof(WriterAsync));
            Bind(typeof(IUpdaterAsync)).To(typeof(UpdaterAsync));

            Bind(typeof(IReaderAsync<>)).To(typeof(ReaderAsync<>));
            Bind(typeof(IWriterAsync<>)).To(typeof(WriterAsync<>));
            Bind(typeof(IUpdaterAsync<>)).To(typeof(UpdaterAsync<>));

            // bind our database r/w/u to an implementation pointing to our test server and database
            Bind(typeof(IDatabaseReader)).To(typeof(DatabaseReader));
            Bind(typeof(IDatabaseWriter)).To(typeof(DatabaseWriter));
            Bind(typeof(IDatabaseUpdater)).To(typeof(DatabaseUpdater));

            Bind(typeof(IDatabaseReader<>)).To(typeof(DatabaseReader<>));
            Bind(typeof(IDatabaseWriter<>)).To(typeof(DatabaseWriter<>));
            Bind(typeof(IDatabaseUpdater<>)).To(typeof(DatabaseUpdater<>));

            // bind our collection r/w/u our test collection
            Bind(typeof(ICollectionReader)).To(typeof(CollectionReader)).WithConstructorArgument("collectionName", COLLECTION_NAME);
            Bind(typeof(ICollectionWriter)).To(typeof(CollectionWriter)).WithConstructorArgument("collectionName", COLLECTION_NAME);
            Bind(typeof(ICollectionUpdater)).To(typeof(CollectionUpdater)).WithConstructorArgument("collectionName", COLLECTION_NAME);

            //Bind(typeof(ICollectionReader<>)).To(typeof(CollectionReader<>));
            //Bind(typeof(ICollectionWriter<>)).To(typeof(CollectionWriter<>));
            //Bind(typeof(ICollectionUpdater<>)).To(typeof(CollectionUpdater<>));
        }
    }
}
