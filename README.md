EasyMongo Overview
==================

  MongoDB is great but its operations aren't driven by interfaces, making dependency injection and Inversion of
  Control for object composition harder than it should be. 
  
  EasyMongo a facade to the official 10gen Mongo DB C# driver that componentizes database and collection Read, Write Update 
  and connection operations. Granular operational scoping supports the principle of least responsibility/LoD and is helpful in
  developing reusable and maintanable software and applications.

Succinctly you might say that EasyMongo:
  - Is a C# facade to the official 10gen MongoDB C# driver providing interface-driven composition and operational granularity.
  - Supplies additional asynchronous functionality in two flavors:
		- asynchronous delegate callbacks
		- System.Threading.Tasks
  - Interface-driven object model simplifies testing and supports DI/IoC.
  - Available Ninject.Extensions.EasyMongo nuget package supporting DI/IoC with Ninject automagically.
  - Abstracts some operations of the underlying 10gen driver for simplistic consumption. 

Implementation
==============
- .Net 4.5
- Built on top of MongoDB C# driver 1.9.2

Tests
=====
- 350+ end-to-end NUnit integration tests written to execute against a localhost mongoDB server 
- 93.2% code coverage
- Execute in 5 minutes
- Useful as documentation of functionality

QuickStart
==============

  Install the latest nuget package via Visual Studio's Package Manager Console:

	Install-Package EasyMongo -Pre 
	
Introduction
==============

	When using EasyMongo you will be referencing components using the interfaces within the EasyMongo.Contract namespace:
	
		IAsyncDelegateReader   - granual asynch CRUD operations with delegate callback
		IAsyncDelegateReaderT (generic impl)
		IAsyncDelegateUpdater
		IAsyncDelegateUpdaterT
		IAsyncDelegateWriter
		IAsyncDelegateWriterT
		IAsyncReader           - granular asynch CRUD operations with System.Threading.Tasks
		IAsyncReaderT
		IAsyncUpdater
		IAsyncUpdaterT
		IAsyncWriter
		IAsyncWriterT
		ICollectionReader      - synchronous and asynchronous operations for a single collection
		ICollectionReaderT
		ICollectionUpdater
		ICollectionUpdaterT
		ICollectionWriter
		ICollectionWriterT
		IDatabaseConnection   - connection operations for a database
		IDatabaseReader       - synchronous and asynchronous operations for a single database
		IDatabaseReaderT
		IDatabaseUpdater
		IDatabaseUpdaterT
		IDatabaseWriter
		IDatabaseWriterT
		IReader              - granual CRUD operations
		IReaderT
		IServerConnection    - connection operations for a server/
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
		IDatabaseConnection databaseConnection = new DatabaseConnection(serverConnection);
		IReader reader = new Reader(databaseConnection);
		
	...but it's really designed to integrate into your favorite dependency injection framework. The registrations are simplistic 
	and intuitive so integrating the parts of EasyMongo you need (and nothing else!) is meant to be easy.
	If you use Ninject there's a nuget extension package available: Ninject.Extensions.EasyMongo.
		
Examples
==============
   1. Startup a localhost MongoDB server.
   2. Step through the following [TestFixture] located at EasyMongo.Test.ReadmeExampleTestFixture.cs to see execution paths.
	  The free MongoDB visualization tool, MongoVue may be useful for following CRUD operations against the MongoDB.
     
		using System;
		using System.Collections.Generic;
		using System.Linq;
		using System.Threading;
		using NUnit.Framework;
		using EasyMongo;
		using EasyMongo.Contract;
		using EasyMongo.Async;
		using EasyMongo.Async.Delegates;
		using EasyMongo.Database;
		using EasyMongo.Collection;
		using Ninject;

		namespace EasyMongo.Readme.Example.Test
		{
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
					writerT.Write("MySecondCollection", new Entry() { Message = "Goodbye World (Generically)" });// cp writerT.Write<Entry>(...)

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

					// because EasyMongo is a componentized framework built with blocks of functionality, creating an
					// object build from many others isn't so easy... That's why EasyMongo provides the Ninject.Extensions.EasyMongo
					// nuget package to automatically provide dependency injection/IoC type bindings so that creating instances of
					// otherwise onerous compositions is as easy as the following:
					Ninject.IKernel kernel = new Ninject.StandardKernel();
					ICollectionUpdater collectionUpdater = kernel.TryGet<ICollectionUpdater>();

					// the alternative to this:
					IServerConnection serverConn = new ServerConnection(LOCAL_MONGO_SERVER_CONNECTION_STRING);
					IDatabaseConnection databaseConnn = new DatabaseConnection(serverConn, "MyFirstDatabase");
					IDatabaseUpdater databaseUpdatr = new DatabaseUpdater(updater, asyncUpdater);
					ICollectionUpdater collectionUpdaterTheHardWay = new CollectionUpdater(databaseUpdater, "MySecondCollection");

					serverConnection.DropAllDatabases();
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


