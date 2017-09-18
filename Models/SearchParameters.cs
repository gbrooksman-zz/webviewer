using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webviewer.Models
{
    public class SearchParameters : PDFEntity
    {
        //what to search for
        public SearchParameters() { }

        public string ProductIDFilter {get; set;}

        public string ProductNameFilter {get; set;}

        public string PublishedDateFilter {get; set;}

        public string CASNumberFilter {get; set;}

        public string ComponentIDFilter {get; set;}

    }
}