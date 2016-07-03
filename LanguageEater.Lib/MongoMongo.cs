using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageEater.Lib
{
    class MongoMongo
    {
        private void insert5Gram(string[] ngram)
        {
            // or use a connection string
            var client = new MongoClient("mongodb://localhost:27017");
            return;

            var database = client.GetDatabase("foo");
            var collection = database.GetCollection<BsonDocument>("bar");
            var document = new BsonDocument
            {
                { "name", "MongoDB" },
                { "type", "Database" },
                { "count", 1 },
                { "info", new BsonDocument
                    {
                        { "x", 203 },
                        { "y", 102 }
                    }}
            };

            collection.InsertOneAsync(document);

            // generate 100 documents with a counter ranging from 0 - 99
            var documents = Enumerable.Range(0, 100).Select(i => new BsonDocument("counter", i));
            //collection.InsertMany(documents);
             collection.InsertManyAsync(documents);

            var count = collection.Count(new BsonDocument());

            var filter = Builders<BsonDocument>.Filter.Eq("i", 71);
             document = collection.Find(filter).First();
            Console.WriteLine(document);

             filter = Builders<BsonDocument>.Filter.Gt("i", 50);
            var cursor = collection.Find(filter).ToCursor();
            foreach (var d in cursor.ToEnumerable())
            {
                Console.WriteLine(d);
            }
        }
    }
}
