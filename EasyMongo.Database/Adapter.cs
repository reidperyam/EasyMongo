using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo;
using EasyMongo.Async;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Database
{
    // TODO - move the implementations of the embedded interfaces to the constructor instead of hard-coding them?
    public abstract class Adapter
    {
        protected IReader  _mongoReader;
        protected IWriter  _mongoWriter;
        protected IUpdater _mongoUpdater;

        protected IReaderAsync  _mongoReaderAsync;
        protected IWriterAsync  _mongoWriterAsync;    
        protected IUpdaterAsync _mongoUpdaterAsync;

        protected MongoCollection _mongoCollection;

        #region    In Development - TODO - These constructors will replace existing constructors
        // NOTICE - the dbConnection driving these injected readers/writers must be Connected!!!
        // TODO: This is why we need to implement Ninject to replace these hard-coded interface implementations with injectable dependencies 
        protected Adapter(//IServerConnection serverConnection, 
                          //string            dbName,
                          IReader           reader, 
                          IWriter           writer, 
                          IUpdater          updater,
                          IReaderAsync      readerAsync, 
                          IWriterAsync      writerAsync,
                          IUpdaterAsync     updaterAsync)
        {
           // IDatabaseConnection<T> mongoDatabaseConnection = ConnectToDatabase(serverConnection, dbName);
           // InitializeAdapter(mongoDatabaseConnection);
            _mongoReader       = reader;
            _mongoWriter       = writer;
            _mongoUpdater      = updater;
            _mongoReaderAsync  = readerAsync;
            _mongoWriterAsync  = writerAsync;
            _mongoUpdaterAsync = updaterAsync;
        }

        /*
        // TODO: This is why we need to implement Ninject to replace these hard-coded interface implementations with injectable dependencies 
        protected Adapter(IServerConnection      mongoServerConnection,
                          IDatabaseConnection<T> mongoDatabaseConnection,
                          IReader<T>             reader,
                          IWriter<T>             writer,
                          IUpdater<T>            updater,
                          IReaderAsync<T>        readerAsync,
                          IWriterAsync<T>        writerAsync,
                          IUpdaterAsync<T>       updaterAsync)
        {
           // InitializeAdapter(mongoDatabaseConnection);
            _mongoReader       = reader;
            _mongoWriter       = writer;
            _mongoUpdater      = updater;
            _mongoReaderAsync  = readerAsync;
            _mongoWriterAsync  = writerAsync;
            _mongoUpdaterAsync = updaterAsync;
        }*/
        #endregion In Development

        /*
        protected Adapter(IServerConnection serverConnection, string dbName) 
            : this(serverConnection, new DatabaseConnection(serverConnection, dbName))
        {
            IDatabaseConnection mongoDatabaseConnection = ConnectToDatabase(serverConnection, dbName);
            InitializeAdapter(mongoDatabaseConnection);
        }


        // TODO: This is why we need to implement Ninject to replace these hard-coded interface implementations with injectable dependencies 
        private Adapter(IServerConnection mongoServerConnection,
                        IDatabaseConnection mongoDatabaseConnection)
            : this(new EasyMongo.Reader(mongoDatabaseConnection),
                   new EasyMongo.Writer(mongoDatabaseConnection),
                   new EasyMongo.Updater(mongoDatabaseConnection))
        {
        }

        private Adapter(IReader reader, IWriter writer, IUpdater updater)
            : this(new ReaderAsync(reader),
                   new WriterAsync(writer),
                   new UpdaterAsync(updater))
        {
            _mongoReader  = reader;
            _mongoWriter  = writer;
            _mongoUpdater = updater;
        }

        private Adapter(IReaderAsync readerAsync, IWriterAsync writerAsync, IUpdaterAsync updaterAsync)
        {
            _mongoReaderAsync  = readerAsync;           
            _mongoWriterAsync  = writerAsync;          
            _mongoUpdaterAsync = updaterAsync;
        }
         * */

        protected IDatabaseConnection ConnectToDatabase(IServerConnection mongoServerConnection, string databaseName)
        {
            mongoServerConnection.Connect();
            ServerIsInitialized(mongoServerConnection);
            DatabaseConnection mongoDatabaseConnection = new DatabaseConnection(mongoServerConnection, databaseName);
            mongoDatabaseConnection.Connect();
            DatabaseIsInitialized(mongoDatabaseConnection);
            
            return mongoDatabaseConnection;
        }

        private void ServerIsInitialized(IServerConnection serverConnection)
        {
            if (!serverConnection.CanConnect())
                throw new MongoServerConnectionException("MongoServerConnection is not initialized");
        }

        private void DatabaseIsInitialized(IDatabaseConnection mongoDatabaseConnection)
        {
            if (mongoDatabaseConnection == null)
                throw new MongoDatabaseConnectionException("MongoDatabaseConnection is not initialized");
        }
    }
}
