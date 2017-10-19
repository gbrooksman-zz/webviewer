using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using webviewer.Managers;
using webviewer.Models;
using Microsoft.Extensions.Caching.Memory;

namespace webviewer.Controllers
{    
    public class ResultController : Controller
    {
        private IMemoryCache _cache;
        private DataManager dataMgr;
        private DBHelpers db = new DBHelpers();
        private readonly ControlConfig _config;
        private IConfiguration _iconfiguration;

        public ResultController(IOptions<ControlConfig> controlConfigAccessor,
                                IConfiguration iconfiguration,
                                IMemoryCache memoryCache)
        {
            _config = controlConfigAccessor.Value;
            _cache = memoryCache;
            _iconfiguration = iconfiguration;
            dataMgr  = new DataManager(_cache, _iconfiguration);
        }

        [HttpGet]
        public IActionResult GetDoc(string guid)
        {
            Document doc = dataMgr.GetDocument(db.GetDBGuid(guid));
            
            return new FileContentResult(doc.Content,"application/pdf");            
        }
       
    }

}