using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkiomBackendTest.Models;

namespace WorkiomBackendTest.DB
{
    public class DBContext
    {

        private readonly IMongoDatabase _mongoDb;
        public DBContext()
        {
            var client = new MongoClient("mongodb+srv://dbuser:Windows2020@cluster0.i3dbn.azure.mongodb.net/Workiom?retryWrites=true&w=majority&connect=replicaSet");
            _mongoDb = client.GetDatabase("Workiom");
        }
        //public IMongoCollection<BsonDocument> Compaines
        //{
        //    get
        //    {
        //        return _mongoDb.GetCollection<BsonDocument>("Companies");
        //    }
        //}

        //public IMongoCollection<Contact> Contacts
        //{
        //    get
        //    {
        //        return _mongoDb.GetCollection<Contact>("Contacts");
        //    }
        //}
    }

}
