using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EasyMongo.Contract;
using EasyMongo.Async;
using EasyMongo.Async.Delegates;
using EasyMongo.Database;
using EasyMongo.Collection;
using Ninject;

namespace EasyMongo.Test.Base.Ninject
{
    [TestFixture]
    public class NinjectTest : IntegrationTestFixture
    {
        [Test]
        public void IntegrationTestNinjectBindingTest()
        {
            string nullErrorFormatString = "{0} did not bind to implementation class as expected";

            Assert.IsNotNull(_mongoServerConnection, string.Format(nullErrorFormatString, "IServerConnection"));
            Assert.IsInstanceOf<ServerConnection>(_mongoServerConnection);

            Assert.IsNotNull(_mongoDatabaseConnection, string.Format(nullErrorFormatString, "IDatabaseConnection"));
            Assert.IsInstanceOf<DatabaseConnection>(_mongoDatabaseConnection);

            #region    EasyMongo
            Assert.IsNotNull(_reader, string.Format(nullErrorFormatString, "IReader"));
            Assert.IsInstanceOf<Reader>(_reader);

            Assert.IsNotNull(_writer, string.Format(nullErrorFormatString, "IWriter"));
            Assert.IsInstanceOf<Writer>(_writer);

            Assert.IsNotNull(_updater, string.Format(nullErrorFormatString, "IUpdater"));
            Assert.IsInstanceOf<Updater>(_updater);

            #region    Generics
            Assert.IsNotNull(_readerT, string.Format(nullErrorFormatString, "IReader<T>"));
            Assert.IsInstanceOf<Reader<Entry>>(_readerT);

            Assert.IsNotNull(_writerT, string.Format(nullErrorFormatString, "IWriter<T>"));
            Assert.IsInstanceOf<Writer<Entry>>(_writerT);

            Assert.IsNotNull(_updaterT, string.Format(nullErrorFormatString, "IUpdater<T>"));
            Assert.IsInstanceOf<Updater<Entry>>(_updaterT);
            #endregion Generics
            #endregion EasyMongo

            #region    EasyMongo.Async
            Assert.IsNotNull(_asyncDelegateReader, string.Format(nullErrorFormatString, "IReaderAsync"));
            Assert.IsInstanceOf<AsyncDelegateReader>(_asyncDelegateReader);

            Assert.IsNotNull(_asyncDelegateWriter, string.Format(nullErrorFormatString, "IWriterAsync"));
            Assert.IsInstanceOf<AsyncDelegateWriter>(_asyncDelegateWriter);

            Assert.IsNotNull(_asyncDelegateUpdater, string.Format(nullErrorFormatString, "IUpdaterAsync"));
            Assert.IsInstanceOf<AsyncDelegateUpdater>(_asyncDelegateUpdater);


            Assert.IsNotNull(_asyncReader, string.Format(nullErrorFormatString, "IReaderTask"));
            Assert.IsInstanceOf<AsyncReader>(_asyncReader);

            Assert.IsNotNull(_asyncWriter, string.Format(nullErrorFormatString, "IWriterTask"));
            Assert.IsInstanceOf<AsyncWriter>(_asyncWriter);

            Assert.IsNotNull(_asyncUpdater, string.Format(nullErrorFormatString, "IUpdaterTask"));
            Assert.IsInstanceOf<AsyncUpdater>(_asyncUpdater);

            #region    Generics
            Assert.IsNotNull(_asyncDelegateReaderT, string.Format(nullErrorFormatString, "IReaderAsync<T>"));
            Assert.IsInstanceOf<AsyncDelegateReader<Entry>>(_asyncDelegateReaderT);

            Assert.IsNotNull(_asyncDelegateWriterT, string.Format(nullErrorFormatString, "IWriterAsync<T>"));
            Assert.IsInstanceOf<AsyncDelegateWriter<Entry>>(_asyncDelegateWriterT);

            Assert.IsNotNull(_AsyncDelegateUpdaterT, string.Format(nullErrorFormatString, "IUpdaterAsync<T>"));
            Assert.IsInstanceOf<AsyncDelegateUpdater<Entry>>(_AsyncDelegateUpdaterT);


            Assert.IsNotNull(_asyncReaderT, string.Format(nullErrorFormatString, "IReaderTask<T>"));
            Assert.IsInstanceOf<AsyncReader<Entry>>(_asyncReaderT);

            Assert.IsNotNull(_asyncWriterT, string.Format(nullErrorFormatString, "IWriterTask<T>"));
            Assert.IsInstanceOf<AsyncWriter<Entry>>(_asyncWriterT);

            Assert.IsNotNull(_asyncUpdaterT, string.Format(nullErrorFormatString, "IUpdaterTask<T>"));
            Assert.IsInstanceOf<AsyncUpdater<Entry>>(_asyncUpdaterT);
            #endregion Generics
            #endregion EasyMongo.Async

            #region    EasyMongo.Database
            Assert.IsNotNull(_databaseReader, string.Format(nullErrorFormatString, "IDatabaseReader"));
            Assert.IsInstanceOf<DatabaseReader>(_databaseReader);

            Assert.IsNotNull(_databaseWriter, string.Format(nullErrorFormatString, "IDatabaseWriter"));
            Assert.IsInstanceOf<DatabaseWriter>(_databaseWriter);

            Assert.IsNotNull(_databaseUpdater, string.Format(nullErrorFormatString, "IDatabaseUpdater"));
            Assert.IsInstanceOf<DatabaseUpdater>(_databaseUpdater);

            #region    Generics
            Assert.IsNotNull(_databaseReaderT, string.Format(nullErrorFormatString, "IDatabaseReader<T>"));
            Assert.IsInstanceOf<DatabaseReader<Entry>>(_databaseReaderT);

            Assert.IsNotNull(_databaseWriterT, string.Format(nullErrorFormatString, "IDatabaseWriter<T>"));
            Assert.IsInstanceOf<DatabaseWriter<Entry>>(_databaseWriterT);

            Assert.IsNotNull(_databaseUpdaterT, string.Format(nullErrorFormatString, "IDatabaseUpdater<T>"));
            Assert.IsInstanceOf<DatabaseUpdater<Entry>>(_databaseUpdaterT);
            #endregion Generics
            #endregion EasyMongo.Database

            #region    EasyMongo.Collection
            Assert.IsNotNull(_collectionReader, string.Format(nullErrorFormatString, "ICollectionReader"));
            Assert.IsInstanceOf<CollectionReader>(_collectionReader);

            Assert.IsNotNull(_collectionWriter, string.Format(nullErrorFormatString, "ICollectionWriter"));
            Assert.IsInstanceOf<CollectionWriter>(_collectionWriter);

            Assert.IsNotNull(_collectionUpdater, string.Format(nullErrorFormatString, "ICollectionUpdater"));
            Assert.IsInstanceOf<CollectionUpdater>(_collectionUpdater);

            #region    Generics
            Assert.IsNotNull(_collectionReaderT, string.Format(nullErrorFormatString, "ICollectionReader<T>"));
            Assert.IsInstanceOf<CollectionReader<Entry>>(_collectionReaderT);

            Assert.IsNotNull(_collectionWriterT, string.Format(nullErrorFormatString, "ICollectionWriter<T>"));
            Assert.IsInstanceOf<CollectionWriter<Entry>>(_collectionWriterT);

            Assert.IsNotNull(_collectionUpdaterT, string.Format(nullErrorFormatString, "ICollectionUpdater<T>"));
            Assert.IsInstanceOf<CollectionUpdater<Entry>>(_collectionUpdaterT);
            #endregion Generics
            #endregion EasyMongo.Collection
        }
    }
}
