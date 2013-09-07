﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public delegate void WriteCompletedEvent(object sender);

    public interface IWriterAsync<T>
    {
        event WriteCompletedEvent AsyncWriteCompleted;

        void WriteAsync(string collectionName, T entry);

        IWriterAsync<T> Create(IWriter<T> writer);
    }
}