using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");

            var db = client.GetDatabase("mybase");
            var col = db.GetCollection<BsonDocument>("test");
            //await col.InsertOneAsync(new BsonDocument { { "key", "value" } });

            var documents = col.Find("{}").Project("{_id:0}").ToList();
            foreach (var document in documents) 
            {
                Console.WriteLine(document);
            }

            using (var cursor = client.ListDatabases())
            {
                var databases = cursor.ToList();
                foreach (var database in databases)
                {
                    Console.WriteLine(database);
                }
            }
        }
    }
}