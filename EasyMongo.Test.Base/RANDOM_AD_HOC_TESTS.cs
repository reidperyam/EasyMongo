using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Globalization;
using System.Diagnostics;
using EasyMongo;
using EasyMongo.Test.Base;
using EasyMongo.Contract;
using Ninject;
using Ninject.Modules;

namespace EasyMongo.Test.Base
{
    [TestFixture]
    class RandomTest : IntegrationTestFixture
    {
        [Test, Ignore]
        public void DateParse()
        {
            Debugger.Launch();
            
            CultureInfo provider = CultureInfo.InvariantCulture;

            DateTime date = DateTime.ParseExact("12/01/2013","d",provider);

            Assert.IsNotNull(date);
        }

        [Test, Ignore]
        public void ServerConnectionAsyncCtorTest()
        {
            Debugger.Launch();

            ServerConnection serverConnection = new ServerConnection("mongodb://localhost");

            int i;
            for ( i = 0; !serverConnection.CanConnect(); ++i)
            ;
        }

        // this test was used to demonstrate generic type inferrence on classes with arguments
        // showing how it isn't required to specify the templated, generic class argument with
        // some method calls
        [Ignore,Test]
        public void TimeStampPersistance()
        {
            Entry testEntry = new Entry();
            testEntry.Message = "Hello World";
            testEntry.TimeStamp = _beforeTest;

            // This shouldn't be possible... since the method signature
            // requires a generic type to be provided with the call...
            _databaseWriter.Write(MONGO_COLLECTION_1_NAME, testEntry);
            IEnumerable<Entry> returned = ReadMongoEntry<Entry>(MONGO_COLLECTION_1_NAME,testEntry.Message);

            Assert.IsNotEmpty(returned);
            Assert.AreEqual(1, returned.Count());

            Assert.AreEqual(_beforeTest, testEntry.TimeStamp);
            Assert.AreEqual(testEntry.TimeStamp, returned.First().TimeStamp);
            Assert.AreEqual(testEntry.Message, returned.First().Message);
            Assert.AreEqual(testEntry.ID, returned.First().ID);

            returned = null;
            DateTime now = System.DateTime.Now;
            testEntry.TimeStamp = now; 

            // This shouldn't be possible... since the method signature
            // requires a generic type to be provided with the call...
            _databaseWriter.Write(MONGO_COLLECTION_1_NAME, testEntry);
            returned = ReadMongoEntry<Entry>(MONGO_COLLECTION_1_NAME, testEntry.Message);

            Assert.IsNotEmpty(returned);
            Assert.AreEqual(1, returned.Count());

            Assert.AreEqual(now, testEntry.TimeStamp);
            Assert.AreEqual(testEntry.TimeStamp, returned.First().TimeStamp);
            Assert.AreEqual(testEntry.Message, returned.First().Message);
            Assert.AreEqual(testEntry.ID, returned.First().ID);
        }


        [Ignore,Test]
        public void GenericVersusNonGenericTest()
        {
            Entry testEntry = new Entry();
            testEntry.Message = "Hello World";
            testEntry.TimeStamp = _beforeTest;

            // This shouldn't be possible... since the method signature
            // requires a generic type to be provided with the call...
            _databaseWriter.Write(MONGO_COLLECTION_1_NAME, testEntry);
            IEnumerable<Entry> returned = ReadMongoEntry<Entry>(MONGO_COLLECTION_1_NAME, testEntry.Message);

            Assert.IsNotEmpty(returned);
            Assert.AreEqual(1, returned.Count());

            Assert.AreEqual(_beforeTest, testEntry.TimeStamp);
            Assert.AreEqual(testEntry.TimeStamp, returned.First().TimeStamp);
            Assert.AreEqual(testEntry.Message, returned.First().Message);
            Assert.AreEqual(testEntry.ID, returned.First().ID);

            returned = null;
            DateTime now = System.DateTime.Now;
            testEntry.TimeStamp = now;

            // This shouldn't be possible... since the method signature
            // requires a generic type to be provided with the call...
            _databaseWriter.Write(MONGO_COLLECTION_1_NAME, testEntry);
            returned = ReadMongoEntry<Entry>(MONGO_COLLECTION_1_NAME, testEntry.Message);

            Assert.IsNotEmpty(returned);
            Assert.AreEqual(1, returned.Count());

            Assert.AreEqual(now, testEntry.TimeStamp);
            Assert.AreEqual(testEntry.TimeStamp, returned.First().TimeStamp);
            Assert.AreEqual(testEntry.Message, returned.First().Message);
            Assert.AreEqual(testEntry.ID, returned.First().ID);
        }
    }
}
