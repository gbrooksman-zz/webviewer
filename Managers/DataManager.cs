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

namespace webviewer.Managers
{
    public class DataManager
    {
        string connString = string.Empty;

        public DataManager(IConfiguration config)
        {
            
        }

        public List<Format> GetFormats()
        {
           List<Format> formats = new List<Format>();


            return formats;
        }

        public List<Subformat> GetSubFormats(string formatCode)
        {
            List<Subformat> subFormats = new List<Subformat>();


            return subFormats;
        }

        public void CreateTestData()
        {
          

        }

    }

}