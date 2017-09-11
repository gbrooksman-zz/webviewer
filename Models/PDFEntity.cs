using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webviewer.Models
{
    public class PDFEntity
    {
        public PDFEntity(){}

        public Guid RecordID {get; set;}

        public string ProductID {get; set;}
        
        public string ProductName {get; set;}

        public string Format {get; set;}

        public string Subformat {get; set;}

        public string Language {get; set;}

        public DateTime PublishedDate {get; set;}

        public int Authorization {get; set;}

        public DateTime DateStamp {get; set;}

        public string UserUpdated {get; set;}

        public string Document {get; set;}

        public string Custom1 {get; set;}

        public string Custom2 {get; set;}

        public string Custom3 {get; set;}

        public string Custom4 {get; set;}

        public string Custom5 {get; set;}



    }

}