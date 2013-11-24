EasyMongo Overview
==================

  MongoDB is great but its operations aren't driven by interfaces, making dependency injection and Inversion of
  Control for object composition harder than it should be.
  
  EasyMongo is just a facade to the official 10gen Mongo DB C# driver that splits the operations into separate, 
  interface-driven operations. Additionally granular operational scoping allows you use operational contracts 
  against the Database or Collection level, (instead of globbing everything together) to support the principle of
  least responsibility/LoD.

Succinctly you might say EasyMongo is:
  - A C# facade to the official 10gen MongoDB C# driver providing interface-driven composition and operational granularity
  - Simplified, interface-driven object model which makes testing easy.
  - Ninject.Extensions.EasyMongo nuget package supporting DI/IoC with Ninject automagically.
  - Takes the liberty of hiding abstract, operations of the underlying 10gen driver not typically leveraged 
    within many application use cases explosing, for the most part, simplistic CRUD operations. 

Implementation
==============
- .Net 4.0
- Built on top of MongoDB C# driver 1.8.2 (current EasyMongo release)

Tests
=====
- Over 250 end-to-end NUnit integration tests written to execute against a locally-deployed mongoDB server.

QuickStart
==============

  Install the latest nuget package via Visual Studio's Package Manager Console:
 - Install-Package EasyMongo -Pre


		using System;
		using System.Collections.Generic;
		using NUnit.Framework;
		using EasyMongo;
		using EasyMongo.Contract;
		using EasyMongo.Async;
		using EasyMongo.Database;
		using EasyMongo.Collection;
		using Ninject;

		namespace JunkyProject
		{
			/// <summary>
			/// A flexible TestFixture useful for verifying user functionality of the EasyMongo nuget package
			/// </summary>
			[TestFixture]
			public class TestFixture
			{
				[TestFixtureSetUp]
				public void TestFixtureSetup()
				{

				}

				private readonly string LOCAL_MONGO_SERVER_CONNECTION_STRING = "mongodb://localhost";

				[Test]
				public void Test()
				{
					System.Diagnostics.Debugger.Launch();
					// initialize a server connection to a locally-running MongoDB server
					IServerConnection serverConnection = new ServerConnection(LOCAL_MONGO_SERVER_CONNECTION_STRING);
					// connect to the existing db on the server (or create it if it does not already exist)
					IDatabaseConnection databaseConnection = new DatabaseConnection(serverConnection, "MyFirstDatabase");

					databaseConnection.Connect();

					  /////////////////////////////
					 // OPERATIONAL GRANULARITY //
					/////////////////////////////

					// create a Writer to write to the database
					IWriter writer = new Writer(databaseConnection);
					// create a Reader to read from the database
					IReader reader = new Reader(databaseConnection);
					// create an Updater to update the database
					IUpdater updater = new Updater(databaseConnection);

					Entry exampleMongoDBEntry = new Entry();
					exampleMongoDBEntry.Message = "Goodbye World";

					// write the created Entry object to the "MyFirstCollection" Collection that exists within the 
					// previously referenced "MyFirstDatabase" that was used to create the "writer" object
					writer.Write<Entry>("MyFirstCollection", exampleMongoDBEntry);

					IEnumerable<Entry> readEntrys = reader.Read<Entry>("MyFirstCollection", // within this collection...
																	   "Description",// for the object field "Description"
																	   "Hello");// return matches for 'Hello'
					  /////////////////////////////
					 // ASYNCHRONOUS OPERATIONS //
					/////////////////////////////

					// read, write and update asynchnronously
					IReaderAsync readerAsync = new ReaderAsync(reader);
					readerAsync.AsyncReadCompleted += new ReadCompletedEvent(readerCallBack);
					readerAsync.ReadAsync<Entry>("MyFirstCollection", "Description", "Goodbye");

					IWriterAsync writerAsync = new WriterAsync(writer);
					IUpdaterAsync updaterAsync = new UpdaterAsync(updater);

					  //////////////////////////////////////////
					 // OR LESS LESS OPERATIONAL GRANULARITY //
					//////////////////////////////////////////

					// get a little higher level with the EasyMongo.Database assembly
					IDatabaseReader databaseReader = new DatabaseReader(reader, readerAsync);
					databaseReader.Read<Entry>("MyFirstCollection", "Description", "Goodbye");
					databaseReader.ReadAsync<Entry>("MyFirstCollection", "Description", "Goodbye");

					IDatabaseWriter databaseWriter = new DatabaseWriter(writer, writerAsync);
					IDatabaseUpdater databaseUpdater = new DatabaseUpdater(updater, updaterAsync);

					  /////////////////////
					 // GENERIC CLASSES //
					/////////////////////

					// Instead of defining generic type arguments at the method level,
					// you can do it once at the class declaration
					IWriter<Entry> writerT = new Writer<Entry>(writer);
					writerT.Write("MyFirstCollection", new Entry() { Message = "Goodbye World (Generically)" });// cp writerT.Write<Entry>(...)

					  //////////////////////
					 // CONTEXTUAL SCOPE //
					//////////////////////

					// control which database is referenced via composition
					databaseConnection = new DatabaseConnection(serverConnection, "MySecondDatabase");
					reader = new Reader(databaseConnection);

					  //////////////////////////////////
					 // REDUCE CLIENT RESPONSIBILITY //
					//////////////////////////////////

					// operate only against "MyFirstDatabase"'s "MySecondCollection"
					ICollectionReader collectionReader = new CollectionReader(databaseReader, "MySecondCollection");
					collectionReader.Read<Entry>("Message", "Goodbye");

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
					IUpdater updatr = new Updater(databaseConnn);
					IUpdaterAsync updatrAsync = new UpdaterAsync(updatr);
					IDatabaseUpdater databaseUpdatr = new DatabaseUpdater(updatr, updaterAsync);
					ICollectionUpdater collectionUpdaterTheHardWay = new CollectionUpdater(databaseUpdater, "MySecondCollection");

				}

				void readerCallBack(object e, Exception ex)
				{
					throw new NotImplementedException();
				}
			}
		}


