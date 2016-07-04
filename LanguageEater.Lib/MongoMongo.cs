using LanguageEater.Lib.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LanguageEater.Lib
{
    class MongoMongo

    {
        private static MongoClient client = new MongoClient(System.Configuration.ConfigurationManager.ConnectionStrings["mongo"].ConnectionString);


        private static BsonDocument getNGramDocOnly(FiveGram ngram)
        {
            var document = BsonDocument.Create(ngram);
            document.Remove("Count");

            return document;
        }

        private void insert5Gram(FiveGram ngram)
        {
            
            var database = client.GetDatabase("english5");
            var collection = database.GetCollection<BsonDocument>("corpus");
            var document =  BsonDocument.Create(ngram);

            //     var filter = new BsonDocumentFilterDefinition.Eq("i", 71);

            //   var newDoc = new BsonDocument { { "_id", 123 }, { "someKey", "someValue" } };

            var filterObj = new MongoDB.Driver.BsonDocumentFilterDefinition<BsonDocument>(getNGramDocOnly(ngram));
            var updateObj = new BsonDocument("$inc", new BsonDocument { { "Count", ngram.Count } });
           
          var newDoc= collection.FindOneAndUpdate<BsonDocument>(
               filter:filterObj,
               update: updateObj,
               options:new FindOneAndUpdateOptions<BsonDocument, BsonDocument>() { ReturnDocument = ReturnDocument.After });

            Console.WriteLine(newDoc);

            //collection.FindOneAndUpdate(
            //    filter: filterObj,
            //    options: new UpdateOptions { IsUpsert = true },
            //    update: Update.Inc()
            //    //ndModify(Queue, SortBy.Null, Update.Inc("MessageCount", 1), true).GetModifiedDocumentAs<UserDocument>();

            // collection.UpdateOne(
            //       filter: ,
            //      options: new UpdateOptions { IsUpsert = true },
            //      update: document);
            //  collection.InsertOneAsync(document);

            return;

          //  var collection = database.GetCollection<BsonDocument>("bar");
           /* var document = new BsonDocument
            {
                { "name", "MongoDB" },
                { "type", "Database" },
                { "count", 1 },
                { "info", new BsonDocument
                    {
                        { "x", 203 },
                        { "y", 102 }
                    }}
            };*/

          
            //// generate 100 documents with a counter ranging from 0 - 99
            //var documents = Enumerable.Range(0, 100).Select(i => new BsonDocument("counter", i));
            ////collection.InsertMany(documents);
            // collection.InsertManyAsync(documents);

            //var count = collection.Count(new BsonDocument());

            // document = collection.Find(filter).First();
            //Console.WriteLine(document);

            // filter = Builders<BsonDocument>.Filter.Gt("i", 50);
            //var cursor = collection.Find(filter).ToCursor();
            //foreach (var d in cursor.ToEnumerable())
            //{
            //    Console.WriteLine(d);
            //}
        }
    }
}
