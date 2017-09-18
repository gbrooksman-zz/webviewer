using System;
using MongoDB.Bson;

namespace webviewer.Models
{
    public class Format
    {
        public Format() {}

        public BsonObjectId _id {get; set;}

        public string Code {get; set;}

        public string Name {get; set;}


    }


}