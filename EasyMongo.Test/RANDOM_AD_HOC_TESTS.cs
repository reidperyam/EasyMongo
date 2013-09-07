﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Globalization;
using System.Diagnostics;

namespace EasyMongo.Test
{
    [TestFixture]
    public class RandomTest 
    {
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