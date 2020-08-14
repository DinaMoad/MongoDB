using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkiomBackendTest.DB;
using WorkiomBackendTest.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace WorkiomBackendTest.Services
{

    public class MonogDBService
    {
        DBContext _db;
        IMongoDatabase _mongoDb;
        public MonogDBService()
        {
            var client = new MongoClient("mongodb+srv://dbuser:Windows2020@cluster0.i3dbn.azure.mongodb.net/Workiom?retryWrites=true&w=majority&connect=replicaSet");
            _mongoDb = client.GetDatabase("Workiom");
        }

        public List<BsonDocument> Get(string collectionName)
        {
            List<BsonDocument> list = new List<BsonDocument>();
            var data = GetMongoCollection(collectionName).Find(comp => true).ToListAsync().Result;

            foreach (BsonDocument dataStr in data)
            {
                BsonDocument doc = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(dataStr);
                list.Add(doc);
            }

            return list;
        }

        public void Add(BsonDocument company, string collectionName)
        {
            GetMongoCollection(collectionName).InsertOne(company);
        }
        public BsonDocument Get(string collectionName, string id)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", int.Parse(id));

            var result = GetMongoCollection(collectionName).Find(filter);

            return result.FirstOrDefault();
        }
        public async Task Update(BsonDocument company, string id, string collectionName)
        {
            try
            {
                await GetMongoCollection(collectionName).ReplaceOneAsync(CreateIdFilter(id), company);
            }
            catch
            {
                throw;
            }
        }

        public async Task Remove(string id,string collectionName)
        {
            await GetMongoCollection(collectionName).DeleteOneAsync(CreateIdFilter(id));
        }

        public async Task<List<BsonDocument>> FilterData(string key, string value, string collectionName)
        {
            int parsedValue = Int32.MinValue;
            int.TryParse(value, out parsedValue);

            var filter = Builders<BsonDocument>.Filter.Regex(key, new BsonRegularExpression(value, "i"))
                | Builders<BsonDocument>.Filter.Eq(key, parsedValue);


            return GetMongoCollection(collectionName).Find(filter).ToList();
        }

        private FilterDefinition<BsonDocument> CreateIdFilter(string id)
        {
            return Builders<BsonDocument>.Filter.Eq("_id", int.Parse(id));
        }


        private IMongoCollection<BsonDocument> GetMongoCollection(string collectionName)
        {
            return _mongoDb.GetCollection<BsonDocument>(collectionName);
        }

       

    }
}
