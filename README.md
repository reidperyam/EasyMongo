Latest Release
==================

	Install-Package EasyMongo -Version 0.1.3-alpha -Pre

EasyMongo Overview
==================

  MongoDB and it's C# driver are great but its operations aren't driven by interfaces, making dependency injection,     Inversion of Control and testing in support of composite architectures additional development effort. 
  
  EasyMongo addresses this to support componentized connection, database, collection and document Read, Write Update operations. Operational scoping supports the principle of least responsibility/LoD and is helpful in
  developing reusable and maintanable software and applications - thus EasyMongo.

Succinctly you might say that EasyMongo:
  - Is a facade and superset of the official 10gen MongoDB C# driver
  - Supplies additional asynchronous functionality in two flavors:
		- asynchronous delegate callbacks
		- System.Threading.Tasks
  - Interface-driven object model simplifies testing and supports DI/IoC.
  - Reinterfaces operations of the underlying 10gen driver for simplistic client consumption supporting rapid application development. 
  - Is predecated on architectural operational granularity and least responsibility

Implementation
==============
- .Net 4.5
- Built on top of MongoDB C# driver 1.9.2

Tests
=====
- 350+ end-to-end NUnit integration tests written to execute against a localhost mongoDB server 
- 93.2% code coverage
- Execute in 4 minutes
- Useful as documentation of functionality
- Executes against started, localhost mongoDB server (accessible via the connection string: "mongodb://localhost", or within MongoVue simply "localhost")

QuickStart
==============

Install the latest nuget package via Visual Studio's Package Manager Console:

	Install-Package EasyMongo -Version 0.1.3-alpha -Pre
	
Introduction
==============

When using EasyMongo you will be referencing components using the interfaces within the EasyMongo.Contract namespace:

	IAsyncDelegateReader - granual asynch CRUD operations with delegate callback
	IAsyncDelegateReaderT (generic impl)
	IAsyncDelegateUpdater
	IAsyncDelegateUpdaterT
	IAsyncDelegateWriter
	IAsyncDelegateWriterT
	IAsyncReader - granular asynch CRUD operations with System.Threading.Tasks
	IAsyncReaderT
	IAsyncUpdater
	IAsyncUpdaterT
	IAsyncWriter
	IAsyncWriterT
	ICollectionReader - synchronous and asynch operations for a single collection
	ICollectionReaderT
	ICollectionUpdater
	ICollectionUpdaterT
	ICollectionWriter
	ICollectionWriterT
	IDatabaseConnection - connection operations for a database
	IDatabaseReader - synchronous and asynch operations for a single database
	IDatabaseReaderT
	IDatabaseUpdater
	IDatabaseUpdaterT
	IDatabaseWriter
	IDatabaseWriterT
	IReader - granual CRUD operations
	IReaderT
	IServerConnection - connection operations for a server/
	IUpdater
	IUpdaterT
	IWriter
	IWriterT	
	
These interfaces are implemented by the following classes:

	EasyMongo.Async.Delegates.AsyncDelegateReader   
	EasyMongo.Async.Delegates.AsyncDelegateReaderT
	EasyMongo.Async.Delegates.AsyncDelegateUpdater
	EasyMongo.Async.Delegates.AsyncDelegateUpdaterT
	EasyMongo.Async.Delegates.AsyncDelegateWriter
	EasyMongo.Async.Delegates.AsyncDelegateWriterT
	EasyMongo.Async.AsyncReader           
	EasyMongo.Async.IAsyncReaderT
	EasyMongo.Async.AsyncUpdater
	EasyMongo.Async.AsyncUpdaterT
	EasyMongo.Async.AsyncWriter
	EasyMongo.Async.AsyncWriterT
	EasyMongo.Collection.CollectionReader      
	EasyMongo.Collection.CollectionReaderT
	EasyMongo.Collection.CollectionUpdater
	EasyMongo.Collection.CollectionUpdaterT
	EasyMongo.Collection.CollectionWriter
	EasyMongo.Collection.CollectionWriterT
	EasyMongo.DatabaseConnection   
	EasyMongo.Database.DatabaseReader       
	EasyMongo.Database.DatabaseReaderT
	EasyMongo.Database.DatabaseUpdater
	EasyMongo.Database.DatabaseUpdaterT
	EasyMongo.Database.DatabaseWriter
	EasyMongo.Database.DatabaseWriterT
	EasyMongo.Reader              
	EasyMongo.ReaderT
	EasyMongo.ServerConnection    
	EasyMongo.Updater
	EasyMongo.UpdaterT
	EasyMongo.Writer
	EasyMongo.WriterT	
	
You can manually wire them up like so:

	IServerConnection serverConnection = new ServerConnection("mongodb://localhost");
	IDatabaseConnection databaseConnection = new DatabaseConnection(serverConnection, "MyMongoDatabase");
	IReader reader = new Reader(databaseConnection);
	
...but it's really designed to integrate into your favorite dependency injection framework. The registrations are simplistic 
and intuitive so integrating the parts of EasyMongo you need (and nothing else!) is meant to be easy.
		
Examples
==============
   1. Startup a localhost MongoDB server.
   2. Step through the following [TestFixture] located at EasyMongo.Test.ReadmeExampleTestFixture.cs to see execution paths.
	  The free MongoDB visualization tool, MongoVue may be useful for following CRUD operations against the MongoDB.
     
		namespace EasyMongo.Readme.Example.Test
		{
		    using System;
		    using System.Collections.Generic;
		    using System.Linq;
		    using System.Threading;
		    using EasyMongo;
		    using EasyMongo.Async;
		    using EasyMongo.Async.Delegates;
		    using EasyMongo.Collection;
		    using EasyMongo.Contract;
		    using EasyMongo.Database;
		    using MongoDB.Bson;
		    using MongoDB.Driver;
		    using MongoDB.Driver.Builders;
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
		
		        IServerConnection   _serverConnection;
		        IDatabaseConnection _databaseConnection;
		
		        [TestFixtureSetUp]
		        public void TestFixtureSetUp()
		        {
		            // initialize a server connection to a locally-running MongoDB server
		            _serverConnection = new ServerConnection(LOCAL_MONGO_SERVER_CONNECTION_STRING);
		            // connect to the existing db on the server (or create it if it does not already exist)
		            _databaseConnection = new DatabaseConnection(_serverConnection, "MyFirstDatabase");
		        }
		
		        [Test]
		        public async void Test()
		        {
		            _databaseConnection.Connect();
		
		            /////////////////////////////////////
		            // OPERATIONAL, CONTEXUAL SCOPE... //
		            /////////////////////////////////////
		
		            // create a Writer to write to the database
		            IWriter writer = new Writer(_databaseConnection);
		            // create a Reader to read from the database
		            IReader reader = new Reader(_databaseConnection);
		            // create an Updater to update the database
		            IUpdater updater = new Updater(_databaseConnection);
		
		            Entry exampleMongoDBEntry = new Entry();
		            exampleMongoDBEntry.Message = "Hello";
		
		            // write the object to the "MyFirstCollection" Collection that exists within the 
		            // previously referenced "MyFirstDatabase" that was used to create the "writer" object
		            writer.Write<Entry>("MyFirstCollection", exampleMongoDBEntry);
		
		            IEnumerable<Entry> readEntrys = reader.Read<Entry>("MyFirstCollection", // within this collection...
		                                                               "Message",// for the object field "Description"
		                                                               "Hello");// return matches for 'Hello'
		            Assert.AreEqual(1, readEntrys.Count());
		
		            ////////////////////////////////////
		            // AND ASYNCHRONOUS OPERATIONS... //
		            ////////////////////////////////////
		
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
		
		            /////////////////////////////////////////////
		            // FOR A SERVER, DATABASE OR COLLECTION... //
		            /////////////////////////////////////////////
		
		            // get a little higher level with the EasyMongo.Database namespace to target a database for operations
		            IDatabaseReader databaseReader = new DatabaseReader(reader, asyncReader);
		            IDatabaseWriter databaseWriter = new DatabaseWriter(writer, asyncWriter);
		            IDatabaseUpdater databaseUpdater = new DatabaseUpdater(updater, asyncUpdater);
		
		            // or a little lower level with the EasyMongo.Collection namespace to target a specific Collection
		            ICollectionReader collectionReader = new CollectionReader(databaseReader, "MyFirstCollection");
		            ICollectionWriter collectionWriter = new CollectionWriter(databaseWriter, "MyFirstCollection");
		            ICollectionUpdater collectionUpdater = new CollectionUpdater(databaseUpdater, "MyFirstCollection");
		
		            ///////////////////////////////////////////////
		            // TO RESTRICT CLIENT SCOPE (LAW OF DEMETER) //
		            ///////////////////////////////////////////////
		
		            // operate only against "MyFirstDatabase"'s "MySecondCollection"
		            readEntrys = collectionReader.Read<Entry>("Message", "Hello");
		            Assert.AreEqual(1, readEntrys.Count());
		
		            /////////////////////
		            // GENERIC CLASSES //
		            /////////////////////
		
		            // Instead of defining generic type arguments at the method level,
		            // you can do it once at the class declaration
		            IWriter<Entry> writerT = new Writer<Entry>(writer);
		            writerT.Write("MySecondCollection", new Entry() { Message = "Goodbye World (Generically)" });
		
		            ///////////////////////////////
		            // SIMPLIFY CREATION VIA IoC //
		            ///////////////////////////////
		
		            // because EasyMongo is a componentized framework built with blocks of functionality, EasyMongo
		            // works great with DI containers and Inversion of Control. 
		            // here's an example of using the nuget Ninject extension to load EasyMongo mappings and a conn 
		            // string from configuration
		            Ninject.IKernel kernel = new Ninject.StandardKernel();
		            ICollectionUpdater<Entry> collectionUpdaterT = kernel.TryGet<ICollectionUpdater<Entry>>();
		
		            // the alternative to this would be:
		            IServerConnection serverConn = new ServerConnection(LOCAL_MONGO_SERVER_CONNECTION_STRING);
		            IDatabaseConnection databaseConnn = new DatabaseConnection(serverConn, "MyFirstDatabase");
		            IDatabaseUpdater databaseUpdatr = new DatabaseUpdater(updater, asyncUpdater);
		            ICollectionUpdater collectionUpdaterTheHardWay = new CollectionUpdater(databaseUpdater, "MySecondCollection");
		
		            /////////////////////////
		            // SIMPLE QUERIES...   //
		            /////////////////////////
		
		            databaseReader.Read<Entry>("MyFirstCollection", "Message", "Hello");
		            readEntrys = await databaseReader.ReadAsync<Entry>("MyFirstCollection", "Message", "Hello");
		            Assert.AreEqual(1, readEntrys.Count());
		
		            /////////////////////////
		            // POWERFUL QUERIES... //
		            /////////////////////////
		
		            // when more robust querying is needed leverage power of underlying MongoDB driver IMongoQuery
		            IMongoQuery query1 = Query.Matches("Message", new BsonRegularExpression("HE", "i"));
		
		            IEnumerable<Entry> queryResults = reader.Execute<Entry>("MyFirstCollection", query1);
		            Assert.AreEqual(1, queryResults.Count());
		            Assert.AreEqual("Hello", queryResults.ElementAt(0).Message);
		
		            //////////////////////
		            // AND COMBINATIONS //
		            //////////////////////
		
		            Entry exampleMongoDBEntry2 = new Entry();
		            exampleMongoDBEntry2.Message = "Hello Again";
		
		            Entry exampleMongoDBEntry3 = new Entry();
		            exampleMongoDBEntry3.Message = "Goodbye";
		
		            writer.Write<Entry>("MyFirstCollection", exampleMongoDBEntry2);
		            writer.Write<Entry>("MyFirstCollection", exampleMongoDBEntry3);
		
		            // "AND" multiple IMongoQueries...
		            IMongoQuery query2 = Query.Matches("Message", new BsonRegularExpression("Again"));
		            queryResults = reader.ExecuteAnds<Entry>("MyFirstCollection", new []{ query1, query2});
		            Assert.AreEqual(1, queryResults.Count());
		            Assert.AreEqual("Hello Again", queryResults.ElementAt(0).Message);
		
		            // "OR" multiple IMongoQueries...
		            IMongoQuery query3 = Query.Matches("Message", new BsonRegularExpression("Goo"));
		            queryResults = reader.ExecuteOrs<Entry>("MyFirstCollection", new[] { query1, query2, query3 });
		            Assert.AreEqual(3, queryResults.Count());
		            Assert.AreEqual("Hello", queryResults.ElementAt(0).Message);
		            Assert.AreEqual("Hello Again", queryResults.ElementAt(1).Message);
		            Assert.AreEqual("Goodbye", queryResults.ElementAt(2).Message);         
		        }
		
		        [TestFixtureTearDown]
		        public void TestFixtureTearDown()
		        {
		            _databaseConnection.DropCollection<Entry>("MyFirstCollection");// remove test entrys
		            _databaseConnection.DropCollection<Entry>("MySecondCollection");
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

