Steps to use ILMERGE-GUI to merge MongoDB.Driver & MongoDB.Bson into EasyMongo.dll assembly
1-combine all EasyMongo assemblies (EasyMongo.dll,EasyMongo.Async.dll,EasyMongo.Contract.dll,EasyMongo.Database.dll,EasyMongo.Collection.dll)
  into a single assembly, EasyMongo.dll
  - 4.0 Client profile
  - sign assembly with key file
2-combine created assembly along with MongoDB.Driver.dll, MongoDB.Bson.dll into single assembly, EasyMongo.dll specifying "internalize" option
  - 4.0 Client profile
  - sign assembly with key file