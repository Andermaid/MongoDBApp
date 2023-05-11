using MongoDB.Driver;

namespace MongoDBApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");

            using (var cursor = await client.ListDatabasesAsync())
            {
                var databases = cursor.ToList();
                foreach (var database in databases)
                {
                    Console.WriteLine(database);
                }
            }

            Console.WriteLine("Hello, World!");
        }
    }
}