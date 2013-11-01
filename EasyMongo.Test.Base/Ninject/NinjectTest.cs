using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EasyMongo.Contract;
using EasyMongo.Async;
using EasyMongo.Test.Model;

namespace EasyMongo.Test.Base.Ninject
{
    [TestFixture]
    public class NinjectTest : IntegrationTestFixture
    {
        [Test]
        public void IntegrationTestNinjectBindingTest()
        {
            string nullErrorFormatString = "{0} did not bind to implementation class as expected";

            IEasyMongoEntry _iEasyMongoEntry = _configurator.TryGet<IEasyMongoEntry>();
            Assert.IsNotNull(_iEasyMongoEntry, string.Format(nullErrorFormatString, "IEasyMongoEntry"));
            Assert.IsInstanceOf<TestEntry>(_iEasyMongoEntry);

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


            Assert.IsNotNull(_readerT, string.Format(nullErrorFormatString, "IReader<T>"));
            Assert.IsInstanceOf<Reader<TestEntry>>(_readerT);

            Assert.IsNotNull(_writerT, string.Format(nullErrorFormatString, "IWriter<T>"));
            Assert.IsInstanceOf<Writer<TestEntry>>(_writerT);

            Assert.IsNotNull(_updaterT, string.Format(nullErrorFormatString, "IUpdater<T>"));
            Assert.IsInstanceOf<Updater<TestEntry>>(_updaterT);
            #endregion EasyMongo

            #region    EasyMongo.Async
            Assert.IsNotNull(_readerAsync, string.Format(nullErrorFormatString, "IReaderAsync"));
            Assert.IsInstanceOf<ReaderAsync>(_readerAsync);

            Assert.IsNotNull(_writerAsync, string.Format(nullErrorFormatString, "IWriterAsync"));
            Assert.IsInstanceOf<WriterAsync>(_writerAsync);

            Assert.IsNotNull(_updaterAsync, string.Format(nullErrorFormatString, "IUpdaterAsync"));
            Assert.IsInstanceOf<UpdaterAsync>(_updaterAsync);


            Assert.IsNotNull(_readerAsyncT, string.Format(nullErrorFormatString, "IReaderAsync<T>"));
            Assert.IsInstanceOf<ReaderAsync<TestEntry>>(_readerAsyncT);

            Assert.IsNotNull(_writerAsyncT, string.Format(nullErrorFormatString, "IWriterAsync<T>"));
            Assert.IsInstanceOf<WriterAsync<TestEntry>>(_writerAsyncT);

            Assert.IsNotNull(_updaterAsyncT, string.Format(nullErrorFormatString, "IUpdaterAsync<T>"));
            Assert.IsInstanceOf<UpdaterAsync<TestEntry>>(_updaterAsyncT);
            #endregion EasyMongo.Async

            #region    EasyMongo.Database
            Assert.IsNotNull(_databaseReader, string.Format(nullErrorFormatString, "IDatabaseReader"));
            Assert.IsInstanceOf<Database.DatabaseReader>(_databaseReader);

            Assert.IsNotNull(_databaseWriter, string.Format(nullErrorFormatString, "IDatabaseWriter"));
            Assert.IsInstanceOf<Database.DatabaseWriter>(_databaseWriter);

            Assert.IsNotNull(_databaseUpdater, string.Format(nullErrorFormatString, "IDatabaseUpdater"));
            Assert.IsInstanceOf<Database.DatabaseUpdater>(_databaseUpdater);
            #endregion EasyMongo.Database

            #region    EasyMongo.Collection
            Assert.IsNotNull(_collectionReader, string.Format(nullErrorFormatString, " ICollectionReader"));
            Assert.IsInstanceOf<Collection.CollectionReader>(_collectionReader);

            Assert.IsNotNull(_collectionWriter, string.Format(nullErrorFormatString, "ICollectionWriter"));
            Assert.IsInstanceOf<Collection.CollectionWriter>(_collectionWriter);

            Assert.IsNotNull(_collectionUpdater, string.Format(nullErrorFormatString, "ICollectionUpdater"));
            Assert.IsInstanceOf<Collection.CollectionUpdater>(_collectionUpdater);
            #endregion EasyMongo.Collection
        }

        [Test]
        public void NextTest()
        {
            // write Ninject nunit test proving using singletons to create all of our integration testing classes results in async testing problems
        }
    }
}
