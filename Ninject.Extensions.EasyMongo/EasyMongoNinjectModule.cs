using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo;
using EasyMongo.Async;
using EasyMongo.Async.Delegates;
using EasyMongo.Contract;
using EasyMongo.Database;
using EasyMongo.Collection;
using Ninject;
using Ninject.Modules;
using Ninject.Syntax;
using System.Configuration;

namespace Ninject.Extensions.EasyMongo
{
    /// <summary>
    /// This class defines 
    /// </summary>
    public class EasyMongoNinjectModule : NinjectModule
    {
        private readonly string CONNECTION_STRING;
        private readonly string DATABASE_NAME;
        private readonly string COLLECTION_NAME;
        private readonly string YOUR_VALUE_HERE = "YOUR_VALUE_HERE";

        private readonly string CONFIGURATION_CONNECTION_STRING = "EasyMongo:NinjectBindings:MongoDBServerConnStr";
        private readonly string CONFIGURATION_DATABASE_NAME     = "EasyMongo:NinjectBindings:DatabaseName";
        private readonly string CONFIGURATION_COLLECTION_NAME   = "EasyMongo:NinjectBindings:CollectionName";

        public EasyMongoNinjectModule()
        {
             //read required parameters from application configuration
             //these settings are required at runtime to retrieve Ninject type bindings for the application automatically
             //without them auto-binding will fail...
             //users will still be able to manually define their own bindings but this will circumvent the entire point of
             //"EasyMongo"
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            #region    Create Missing Configuration Stubs if Missing
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(CONFIGURATION_CONNECTION_STRING))
            {
                config.AppSettings.Settings.Add(CONFIGURATION_CONNECTION_STRING, YOUR_VALUE_HERE);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("AppSettings");
            }

            if (!ConfigurationManager.AppSettings.AllKeys.Contains(CONFIGURATION_DATABASE_NAME))
            {
                config.AppSettings.Settings.Add(CONFIGURATION_DATABASE_NAME, YOUR_VALUE_HERE);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("AppSettings");
            }

            if (!ConfigurationManager.AppSettings.AllKeys.Contains(CONFIGURATION_COLLECTION_NAME))
            {
                config.AppSettings.Settings.Add(CONFIGURATION_COLLECTION_NAME, YOUR_VALUE_HERE);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("AppSettings");
            }
            #endregion Create Missing Configuration Stubs if Missing

            CONNECTION_STRING = ConfigurationManager.AppSettings[CONFIGURATION_CONNECTION_STRING];
            DATABASE_NAME     = ConfigurationManager.AppSettings[CONFIGURATION_DATABASE_NAME];
            COLLECTION_NAME   = ConfigurationManager.AppSettings[CONFIGURATION_COLLECTION_NAME];

            #region    Error on Missing
            if (string.IsNullOrWhiteSpace(CONNECTION_STRING))
            {
                string format = string.Format("Could not parse argument '{0}' value from Configuration AppSettings.",
                    CONFIGURATION_CONNECTION_STRING);
                throw new ConfigurationErrorsException(format);
            }

            if (string.IsNullOrWhiteSpace(DATABASE_NAME))
            {
                string format = string.Format("Could not parse argument '{0}' value from Configuration AppSettings.",
                    CONFIGURATION_DATABASE_NAME);
                throw new ConfigurationErrorsException(format);
            }

            if (string.IsNullOrWhiteSpace(COLLECTION_NAME))
            {
                string format = string.Format("Could not parse argument '{0}' value from Configuration AppSettings.",
                    CONFIGURATION_COLLECTION_NAME);
                throw new ConfigurationErrorsException(format);
            }
            #endregion    Error on Missing      
        }

        /// <summary>
        /// Contains bindings and constructor arguments used by the IntegrationTest TestFixture
        /// </summary>
        public override void Load()
        {
            Bind<IServerConnection>().To<ServerConnection>().InSingletonScope().WithConstructorArgument("connectionString", CONNECTION_STRING);
            Bind(typeof(IDatabaseConnection)).To<DatabaseConnection>().InSingletonScope().WithConstructorArgument("databaseName", DATABASE_NAME);

            Bind(typeof(IReader)).To(typeof(Reader));
            Bind(typeof(IWriter)).To(typeof(Writer));
            Bind(typeof(IUpdater)).To(typeof(Updater));

            Bind(typeof(IReader<>)).To(typeof(Reader<>));
            Bind(typeof(IWriter<>)).To(typeof(Writer<>));
            Bind(typeof(IUpdater<>)).To(typeof(Updater<>));

            Bind(typeof(IAsyncDelegateReader)).To(typeof(AsyncDelegateReader));
            Bind(typeof(IAsyncDelegateWriter)).To(typeof(AsyncDelegateWriter));
            Bind(typeof(IAsyncDelegateUpdater)).To(typeof(AsyncDelegateUpdater));

            Bind(typeof(IAsyncReader)).To(typeof(AsyncReader));
            Bind(typeof(IAsyncWriter)).To(typeof(AsyncWriter));
            Bind(typeof(IAsyncUpdater)).To(typeof(AsyncUpdater));

            Bind(typeof(IAsyncDelegateReader<>)).To(typeof(AsyncDelegateReader<>));
            Bind(typeof(IAsyncDelegateWriter<>)).To(typeof(AsyncDelegateWriter<>));
            Bind(typeof(IAsyncDelegateUpdater<>)).To(typeof(AsyncDelegateUpdater<>));

            Bind(typeof(IAsyncReader<>)).To(typeof(AsyncReader<>));
            Bind(typeof(IAsyncWriter<>)).To(typeof(AsyncWriter<>));
            Bind(typeof(IAsyncUpdater<>)).To(typeof(AsyncUpdater<>));

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

            Bind(typeof(ICollectionReader<>)).To(typeof(CollectionReader<>));
            Bind(typeof(ICollectionWriter<>)).To(typeof(CollectionWriter<>));
            Bind(typeof(ICollectionUpdater<>)).To(typeof(CollectionUpdater<>));
        }
    }
}
