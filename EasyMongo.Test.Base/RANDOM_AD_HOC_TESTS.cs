using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Globalization;
using System.Diagnostics;
using EasyMongo.Test.Base;
using EasyMongo.Contract;
using Ninject;
using Ninject.Modules;

namespace EasyMongo.Test.Base
{
    [TestFixture]
    class RandomTest : IntegrationTestFixture
    {
        public interface IEntry { }

        public class Entry : IEntry { }

        public interface IDBConnection<T> where T : IEntry { }

        public class DBConnection<T> : IDBConnection<T> where T : IEntry { }

        class TestModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IEntry>().To<Entry>();
                Bind(typeof(IDBConnection<IEntry>)).To(typeof(DBConnection<Entry>));
            }
        }

        [Explicit, Test]
        public void NinjectGenericLoadTest()
        {
            StandardKernel kernel = new StandardKernel(new TestModule());
            var ninjected = kernel.TryGet(typeof(IDBConnection<IEntry>));
            Assert.IsInstanceOf<DBConnection<Entry>>(ninjected); 
        }

        [Explicit,Test]
        public void DateParse()
        {
            Debugger.Launch();
            
            CultureInfo provider = CultureInfo.InvariantCulture;

            DateTime date = DateTime.ParseExact("12/01/2013","d",provider);

            Assert.IsNotNull(date);
        }

        [Explicit, Test]
        public void ServerConnectionAsyncCtorTest()
        {
            Debugger.Launch();

            ServerConnection serverConnection = new ServerConnection("mongodb://localhost");

            int i;
            for ( i = 0; !serverConnection.CanConnect(); ++i)
            ;
        }
    }
}
