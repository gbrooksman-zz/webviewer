using System;
using MongoDB.Bson;

namespace webviewer.Models
{
    public class Subformat
    {
        public Subformat() {}

         public BsonObjectId _id {get; set;}

        public string Code {get; set;}

        public string FormatCode {get; set;}

        public string Name {get; set;}
    }
}