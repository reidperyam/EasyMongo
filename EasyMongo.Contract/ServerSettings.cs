using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB
{
    /// <summary>
    /// Adapter class for MongoDB.MongoServerSettings
    /// </summary>
    /// This class might be thrown away since the purpose behind it was to absolve consumer of mongoDB assembly from referencing the C# driver assemblies
    /// ... but even this adapter class has MongoCredentials, ConnectionMode classes displayed in ctor... these classes are defined in the C# driver assemblies
    /// and force dependency on them by anything referencing.
    /// the solution is to break down these classes into their component parts, include them as ctor parameters. But I'm kind of thinking that would be a pain in the 
    /// ass.. I think I'll wait until I need it
    public class ServerSettings
    {
        MongoServerSettings _mongoServerSettings;

        #region constructors
        ServerSettings(
            ConnectionMode connectionMode,
            TimeSpan connectTimeout,
            MongoCredentialsStore credentialsStore,
            MongoCredentials defaultCredentials,
            Bson.GuidRepresentation guidRepresentation,
            bool ipv6,
            TimeSpan maxConnectionIdleTime,
            TimeSpan maxConnectionLifeTime,
            int maxConnectionPoolSize,
            int minConnectionPoolSize,
            string replicaSetName,
            SafeMode safeMode,
            IEnumerable<MongoServerAddress> servers,
            bool slaveOk,
            TimeSpan socketTimeout,
            int waitQueueSize,
            TimeSpan waitQueueTimeout)
        {
            _mongoServerSettings = new MongoServerSettings(connectionMode,
                                                        connectTimeout,
                                                        credentialsStore,
                                                        defaultCredentials,
                                                        guidRepresentation,
                                                        ipv6,
                                                        maxConnectionIdleTime,
                                                        maxConnectionLifeTime,
                                                        maxConnectionPoolSize,
                                                        minConnectionPoolSize,
                                                        replicaSetName,
                                                        safeMode,
                                                        servers,
                                                        slaveOk,
                                                        socketTimeout,
                                                        waitQueueSize,
                                                        waitQueueTimeout); 
        }

        ServerSettings()
        {
            _mongoServerSettings = new MongoServerSettings();

        }
        #endregion constructors

        public ConnectionMode connectionMode          { get{return _mongoServerSettings.ConnectionMode;}set{_mongoServerSettings.ConnectionMode=value;}}
        public TimeSpan connectTimeout                { get { return _mongoServerSettings.ConnectTimeout; } set { _mongoServerSettings.ConnectTimeout = value; } }
        public MongoCredentialsStore credentialsStore { get { return _mongoServerSettings.CredentialsStore; } set { _mongoServerSettings.CredentialsStore = value; } }
        public MongoCredentials defaultCredentials { get { return _mongoServerSettings.DefaultCredentials; } set { _mongoServerSettings.DefaultCredentials = value; } }
        public Bson.GuidRepresentation guidRepresentation { get { return _mongoServerSettings.GuidRepresentation; } set { _mongoServerSettings.GuidRepresentation = value; } }
        public bool ipv6{get{return _mongoServerSettings.IPv6;}set{_mongoServerSettings.IPv6=value;}}
        public TimeSpan maxConnectionIdleTime { get { return _mongoServerSettings.MaxConnectionIdleTime; } set { _mongoServerSettings.MaxConnectionIdleTime = value; } }
        public TimeSpan maxConnectionLifeTime { get { return _mongoServerSettings.MaxConnectionLifeTime; } set { _mongoServerSettings.MaxConnectionLifeTime = value; } }
        public int maxConnectionPoolSize{get{return _mongoServerSettings.MaxConnectionPoolSize;}set{_mongoServerSettings.MaxConnectionPoolSize=value;}}
        public int minConnectionPoolSize { get { return _mongoServerSettings.MinConnectionPoolSize; } set { _mongoServerSettings.MinConnectionPoolSize = value; } }
        public string replicaSetName { get { return _mongoServerSettings.ReplicaSetName; } set { _mongoServerSettings.ReplicaSetName = value; } }
        public SafeMode safeMode{get{return _mongoServerSettings.SafeMode;}set{_mongoServerSettings.SafeMode=value;}}
        public IEnumerable<MongoServerAddress> servers{get{return _mongoServerSettings.Servers;}set{_mongoServerSettings.Servers=value;}}
        public bool slaveOk{get{return _mongoServerSettings.SlaveOk;}set{_mongoServerSettings.SlaveOk=value;}}
        public TimeSpan socketTimeout{get{return _mongoServerSettings.SocketTimeout;}set{_mongoServerSettings.SocketTimeout=value;}}
        public int waitQueueSize{get{return _mongoServerSettings.WaitQueueSize;}set{_mongoServerSettings.WaitQueueSize=value;}}
        public TimeSpan waitQueueTimeout { get { return _mongoServerSettings.WaitQueueTimeout; } set { _mongoServerSettings.WaitQueueTimeout = value; } }

    }
}
