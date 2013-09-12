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
    public abstract class Adapter<T> where T : IEasyMongoEntry
    {
        protected IReader<T>  _mongoReader;
        protected IWriter<T>  _mongoWriter;
        protected IUpdater<T> _mongoUpdater;

        protected IReaderAsync<T>  _mongoReaderAsync;
        protected IWriterAsync<T>  _mongoWriterAsync;    
        protected IUpdaterAsync<T> _mongoUpdaterAsync;

        protected MongoCollection _mongoCollection;

        protected Adapter(IServerConnection serverConnection, string dbName) 
            : this(serverConnection, new DatabaseConnection<T>(serverConnection, dbName))
        {
            IDatabaseConnection<T> mongoDatabaseConnection = ConnectToDatabase(serverConnection, dbName);
            InitializeAdapter(mongoDatabaseConnection);
        }

        private Adapter(IServerConnection mongoServerConnection,
                       IDatabaseConnection<T> mongoDatabaseConnection)
            : this(new EasyMongo.Reader<T>(mongoDatabaseConnection),
                   new EasyMongo.Writer<T>(mongoDatabaseConnection),
                   new EasyMongo.Updater<T>(mongoDatabaseConnection))
        {
        }

        private Adapter(IReader<T> reader, IWriter<T> writer, IUpdater<T> updater)
            : this(new ReaderAsync<T>(reader),
                   new WriterAsync<T>(writer),
                   new UpdaterAsync<T>(updater))
        {
            _mongoReader  = reader;
            _mongoWriter  = writer;
            _mongoUpdater = updater;
        }

        private Adapter(IReaderAsync<T> readerAsync, IWriterAsync<T> writerAsync, IUpdaterAsync<T> updaterAsync)
        {
            _mongoReaderAsync  = readerAsync;           
            _mongoWriterAsync  = writerAsync;          
            _mongoUpdaterAsync = updaterAsync;
        }

        protected IDatabaseConnection<T> ConnectToDatabase(IServerConnection mongoServerConnection, string databaseName)
        {
            mongoServerConnection.Connect();
            ServerIsInitialized(mongoServerConnection);
            DatabaseConnection<T> mongoDatabaseConnection = new DatabaseConnection<T>(mongoServerConnection, databaseName);
            mongoDatabaseConnection.Connect();
            DatabaseIsInitialized(mongoDatabaseConnection);
            
            return mongoDatabaseConnection;
        }

        protected void InitializeAdapter(IDatabaseConnection<T> mongoDatabaseConnection)
        {
            //** Use interface-defined factory method to fetch new implementation for the _mongoDatabaseConnection
            _mongoReader = _mongoReader.Create(mongoDatabaseConnection);
            _mongoWriter = _mongoWriter.Create(mongoDatabaseConnection);
            _mongoUpdater = _mongoUpdater.Create(mongoDatabaseConnection);

            _mongoReaderAsync = _mongoReaderAsync.Create(_mongoReader);
            _mongoWriterAsync = _mongoWriterAsync.Create(_mongoWriter);
            _mongoUpdaterAsync = _mongoUpdaterAsync.Create(_mongoUpdater);
        }

        private void ServerIsInitialized(IServerConnection serverConnection)
        {
            if (!serverConnection.CanConnect())
                throw new MongoServerConnectionException("MongoServerConnection is not initialized");
        }

        private void DatabaseIsInitialized(IDatabaseConnection<T> mongoDatabaseConnection)
        {
            if (mongoDatabaseConnection == null)
                throw new MongoDatabaseConnectionException("MongoDatabaseConnection is not initialized");
        }
    }
}
