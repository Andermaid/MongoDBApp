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
            
            //Проверка заполненности коллекции и генерация документов
            var testCol = db.GetCollection<BsonDocument>("test");
            if (testCol.Find("{}").ToList().Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    await testCol.InsertOneAsync(new BsonDocument { { "key" + i.ToString(), "value" + i.ToString() } });
                }
            }

            //Вывод документов коллекции
            var documents = testCol.Find("{}").Project("{_id:0}").ToList();
            foreach (var document in documents) 
            {
                Console.WriteLine(document);
            }

            //Очистка коллекции
            await testCol.DeleteManyAsync("{}");
        }
    }
}