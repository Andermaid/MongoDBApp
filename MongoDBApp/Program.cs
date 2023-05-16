﻿using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");

            var db = client.GetDatabase("mybase");
            
            //Неявное создание коллекции
            var indirectCol = db.GetCollection<BsonDocument>("indirect");

            //Проверка заполненности коллекции и добавление документов
            if (indirectCol.Find("{}").ToList().Count == 0)
            {
                //Добавление по одному
                for (int i = 0; i < 10; i++)
                {
                    await indirectCol.InsertOneAsync(new BsonDocument { { "name", "singleDoc" + i.ToString() } });
                }
            }

            //Вывод документов коллекции
            var indirectDocs = indirectCol.Find("{}").Project("{_id:0}").ToList();
            foreach (var document in indirectDocs) 
            {
                Console.WriteLine(document);
            }

            //Очистка коллекции
            await indirectCol.DeleteManyAsync("{}");

            //Явное создание коллекции
            await db.CreateCollectionAsync("direct");
            var directCol = db.GetCollection<BsonDocument>("direct");

            //Проверка заполненности коллекции и добавление документов
            if (directCol.Find("{}").ToList().Count == 0)
            {
                //Добавление списка
                List<BsonDocument> documentList = new List<BsonDocument>();
                for (int i = 0; i < 10; i++)
                {
                    documentList.Add(new BsonDocument { { "name", "multipleDoc" + i.ToString() }, { "data", "some data " + i.ToString() } });
                }
                await directCol.InsertManyAsync(documentList);
            }

            //Вывод документов коллекции
            var directDocs = directCol.Find("{}").Project("{_id:0}").ToList();
            foreach (var document in directDocs)
            {
                Console.WriteLine(document);
            }

            //Удаление коллекций
            await db.DropCollectionAsync("indirect");
            await db.DropCollectionAsync("direct");

            //Вывод списка коллекций
            var collections = await db.ListCollectionNamesAsync();
            foreach (var collection in collections.ToList())
            {
                Console.WriteLine(collection);
            }
        }
    }
}