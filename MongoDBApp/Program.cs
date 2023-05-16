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
            
            //Проверка заполненности коллекции и добавление документов
            var testCol = db.GetCollection<BsonDocument>("test");
            if (testCol.Find("{}").ToList().Count == 0)
            {
                //Добавление по одному
                for (int i = 0; i < 10; i++)
                {
                    await testCol.InsertOneAsync(new BsonDocument { { "name", "singleDoc" + i.ToString() } });
                }
                //Добавление коллекции
                List<BsonDocument> documentList = new List<BsonDocument>();
                for (int i = 0; i < 10; i++)
                {
                    documentList.Add(new BsonDocument { { "name", "multipleDoc" + i.ToString() } });
                }
                await testCol.InsertManyAsync(documentList);
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