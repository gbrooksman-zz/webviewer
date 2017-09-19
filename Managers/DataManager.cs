using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webviewer.Models;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver.Builders;







namespace webviewer.Managers
{
    public class DataManager
    {
        MongoClient client;
        IMongoDatabase database;

        string connString = string.Empty;

        public DataManager(IConfiguration config)
        {
            connString = config.GetSection("ConnectionStrings").GetSection("Mongo").Value;

            client = new MongoClient(connString);

            database = client.GetDatabase("webviewer");

            CreateTestData();
        }

        public List<Format> GetFormats()
        {
            var formatCollection = database.GetCollection<Format>("formats");            

            var list =  formatCollection.Find(x => x.Name.Length > 0 ).ToList();

            return list;
        }

        public List<Subformat> GetSubFormats(string formatCode)
        {
            var subformatCollection = database.GetCollection<Subformat>("subformats");            

            var list =  subformatCollection.Find(s => s.FormatCode == formatCode).ToList();

            return list;
        }

        public void CreateTestData()
        {
            var formatCollection = database.GetCollection<Format>("formats");

            var filter = Builders<Format>.Filter.Empty;

            formatCollection.DeleteMany(filter);

            formatCollection.InsertOne(new Format {Code="MTR", Name = "Mster"});
            formatCollection.InsertOne(new Format {Code="EUR", Name = "Europe"});
            formatCollection.InsertOne(new Format {Code="NAM", Name = "North America"});

            //-------------------------------------------------------------------------------

            var subformatCollection = database.GetCollection<Subformat>("subformats");

            var filter1 = Builders<Subformat>.Filter.Empty;

            subformatCollection.DeleteMany(filter1);

            subformatCollection.InsertOne(new Subformat {Code="MTR1", FormatCode="MTR", Name = "Master1"});
            subformatCollection.InsertOne(new Subformat {Code="MTR2", FormatCode="MTR", Name = "Master2"});
            subformatCollection.InsertOne(new Subformat {Code="MTR3", FormatCode="MTR", Name = "Master3"});

            subformatCollection.InsertOne(new Subformat {Code="EUR1", FormatCode="EUR", Name = "Europe1"});
            subformatCollection.InsertOne(new Subformat {Code="EUR2", FormatCode="EUR", Name = "Europe2"});
            subformatCollection.InsertOne(new Subformat {Code="EUR3", FormatCode="EUR", Name = "Europe3"});
      
            subformatCollection.InsertOne(new Subformat {Code="NAM1", FormatCode="NAM", Name = "North America1"});
            subformatCollection.InsertOne(new Subformat {Code="NAM2", FormatCode="NAM", Name = "North America2"});

         //-------------------------------------------------------------------------------


        }

    }

}