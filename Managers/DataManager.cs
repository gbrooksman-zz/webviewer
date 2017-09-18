using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webviewer.Models;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;

namespace webviewer.Managers
{
    public class DataManager
    {

        MongoClient client;
        IMongoDatabase database;

        string connString = string.Empty;

        public DataManager(IConfiguration config)
        {
            connString =  config.GetSection("ConnectionStrings").GetSection("Mongo").Value;

            client = new MongoClient(connString);

            database = client.GetDatabase("webviewer");
        }


        public List<Format> GetFormats()
        {
            var formatCollection = database.GetCollection<Format>("formats");            

            var list =  formatCollection.Find(x => x.Name.Length > 0 ).ToList();

            return list;

        }

    }

}