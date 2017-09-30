using System;

namespace webviewer.Models
{
    public class SearchParameters : Document
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