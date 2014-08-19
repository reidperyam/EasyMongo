namespace EasyMongo.Readme.Example.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using EasyMongo.Async;
    using EasyMongo.Async.Delegates;
    using EasyMongo.Collection;
    using EasyMongo.Contract;
    using EasyMongo.Database;
    using Ninject;
    using NUnit.Framework;

    /// <summary>
    /// A cute overview of how EasyMongo functions
    /// </summary>
    /// <remarks>Requires localhost MongoDB server running at execution time!</remarks>
    [TestFixture]
    public class ReadmeExampleTestFixture
    {
        readonly string LOCAL_MONGO_SERVER_CONNECTION_STRING = "mongodb://localhost";
        AutoResetEvent _readerAutoResetEvent = new AutoResetEvent(false);
        protected Exception _asyncException = null;
        protected List<Entry> _asyncReadResults = new List<Entry>();

        [Test]
        public async void Test()
        {
            // initialize a server connection to a locally-running MongoDB server
            IServerConnection serverConnection = new ServerConnection(LOCAL_MONGO_SERVER_CONNECTION_STRING);
            // connect to the existing db on the server (or create it if it does not already exist)
            IDatabaseConnection databaseConnection = new DatabaseConnection(serverConnection, "MyFirstDatabase");

            databaseConnection.Connect();

            //////////////////////
            // CONTEXTUAL SCOPE //
            //////////////////////

            // create a Writer to write to the database
            IWriter writer = new Writer(databaseConnection);
            // create a Reader to read from the database
            IReader reader = new Reader(databaseConnection);
            // create an Updater to update the database
            IUpdater updater = new Updater(databaseConnection);

            Entry exampleMongoDBEntry = new Entry();
            exampleMongoDBEntry.Message = "Hello";

            // write the object to the "MyFirstCollection" Collection that exists within the 
            // previously referenced "MyFirstDatabase" that was used to create the "writer" object
            writer.Write<Entry>("MyFirstCollection", exampleMongoDBEntry);

            IEnumerable<Entry> readEntrys = reader.Read<Entry>("MyFirstCollection", // within this collection...
                                                               "Message",// for the object field "Description"
                                                               "Hello");// return matches for 'Hello'
            Assert.AreEqual(1, readEntrys.Count());

            /////////////////////////////
            // ASYNCHRONOUS OPERATIONS //
            /////////////////////////////

            // read, write and update asynchronously using System.Threading.Task
            IAsyncReader asyncReader = new AsyncReader(reader);
            readEntrys = await asyncReader.ReadAsync<Entry>("MyFirstCollection", "Message", "Hello");
            Assert.AreEqual(1, readEntrys.Count());

            IAsyncWriter asyncWriter = new AsyncWriter(writer);
            IAsyncUpdater asyncUpdater = new AsyncUpdater(updater);

            // or delegate call backs
            IAsyncDelegateReader asyncDelegateReader = new AsyncDelegateReader(reader);
            asyncDelegateReader.AsyncReadCompleted += new ReadCompletedEvent(readerCallBack);
            asyncDelegateReader.ReadAsync<Entry>("MyFirstCollection", "Message", "Hello");
            _readerAutoResetEvent.WaitOne();

            Assert.AreEqual(1, _asyncReadResults.Count());

            IAsyncDelegateWriter asyncDelegateWriter = new AsyncDelegateWriter(writer);
            IAsyncDelegateUpdater asyncDelegateUpdater = new AsyncDelegateUpdater(updater);

            /////////////////////////////
            // OPERATIONAL GRANULARITY //
            /////////////////////////////

            // get a little higher level with the EasyMongo.Database namespace to reference a database
            IDatabaseReader databaseReader = new DatabaseReader(reader, asyncReader);
            databaseReader.Read<Entry>("MyFirstCollection", "Message", "Hello");
            readEntrys = await databaseReader.ReadAsync<Entry>("MyFirstCollection", "Message", "Hello");
            Assert.AreEqual(1, readEntrys.Count());

            IDatabaseWriter databaseWriter = new DatabaseWriter(writer, asyncWriter);
            IDatabaseUpdater databaseUpdater = new DatabaseUpdater(updater, asyncUpdater);

            /////////////////////
            // GENERIC CLASSES //
            /////////////////////

            // Instead of defining generic type arguments at the method level,
            // you can do it once at the class declaration
            IWriter<Entry> writerT = new Writer<Entry>(writer);
            writerT.Write("MySecondCollection", new Entry() { Message = "Goodbye World (Generically)" });

            //////////////////////////////////
            // REDUCE CLIENT RESPONSIBILITY //
            //////////////////////////////////

            // operate only against "MyFirstDatabase"'s "MySecondCollection"
            ICollectionReader collectionReader = new CollectionReader(databaseReader, "MySecondCollection");
            readEntrys = collectionReader.Read<Entry>("Message", "Goodbye");
            Assert.AreEqual(1, readEntrys.Count());

            ///////////////////////////////////////
            // SIMPLIFY CREATION VIA NINJECT IoC //
            ///////////////////////////////////////

            // because EasyMongo is a componentized framework built with blocks of functionality, EasyMongo
            // works great with DI containers and Inversion of Control. 
            // here's an example of using the nuget Ninject extension to load EasyMongo mappings and a conn 
            // string from configuration
            Ninject.IKernel kernel = new Ninject.StandardKernel();
            ICollectionUpdater collectionUpdater = kernel.TryGet<ICollectionUpdater>();

            // the alternative to this would be:
            IServerConnection serverConn = new ServerConnection(LOCAL_MONGO_SERVER_CONNECTION_STRING);
            IDatabaseConnection databaseConnn = new DatabaseConnection(serverConn, "MyFirstDatabase");
            IDatabaseUpdater databaseUpdatr = new DatabaseUpdater(updater, asyncUpdater);
            ICollectionUpdater collectionUpdaterTheHardWay = new CollectionUpdater(databaseUpdater, "MySecondCollection");

            serverConnection.DropAllDatabases();// remove test entrys
        }

        void readerCallBack(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<Entry> results = (IEnumerable<Entry>)e;
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }
    }
}