EasyMongo Overview
==================

- A C# facade to the official 10gen MongoDB C# driver. that aims to obfuscates underlying complexity when convenient.
- Simplified, interface-driven object model which makes testing easy.
- Contains a default, NinjectModule plugin for auto-loaded bindings to support IoC out of the box.

Implementation
==============
- .Net 4.0
- Built on top of MongoDB C# driver 1.8.2

Tests
=====
- Over 200 end-to-end NUnit integration tests written to execute against locally-deployed mongoDB server.

QuickStart
==============

//TODO

using EasyMongo;
using EasyMongo.Contract;

...

[Test]
public void Test()
{

}



