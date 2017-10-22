using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using webviewer.Managers;
using webviewer.Models;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace webviewer.Controllers
{
    public class ApiController : Controller
    { 

        private IMemoryCache _cache;

        private DataManager dataMgr;

        private static CoreEntities coreEntities = new CoreEntities();

         private IConfiguration _iconfiguration;

        public ApiController(IConfiguration iconfiguration,
                             IMemoryCache memoryCache)
        {            
            _iconfiguration = iconfiguration;
            _cache = memoryCache;
            dataMgr = new DataManager(_cache,_iconfiguration);     

            coreEntities.AuthorizationList = dataMgr.GetsAuthLevels();
            coreEntities.FormatList = dataMgr.GetFormats();
            coreEntities.SubformatList = dataMgr.GetSubFormats();
            coreEntities.LanguageList = dataMgr.GetLanguages();      

        }

        [HttpGet]
        public IActionResult GetFormats()
        {
            List<Format> formats = coreEntities.FormatList.ToList();            
            return Json(formats);
        }

        [HttpGet]
        public IActionResult GetSubformats(string Format)
        {
            List<Subformat> subformats = coreEntities.SubformatList
                                            .Where(s => s.FormatCode == Format)                
                                            .ToList();            
            return Json(subformats);
        }


        [HttpGet]
        public IActionResult GetLanguages()
        {
            List<Language> langs = coreEntities.LanguageList.ToList();            
            return Json(langs);
        }


        [HttpGet]
        public IActionResult GetAuthorizations()
        {
            List<Authorization> auths = coreEntities.AuthorizationList.ToList();            
            return Json(auths);
        }

       [HttpGet]
        public IActionResult Search(SearchParameters searchParams, bool fetchPDFs = false, int maxDocs = 100)
        {
            List<Document> docs = dataMgr.GetDocuments(searchParams, fetchPDFs, maxDocs);

            return Json(docs);
        }

    }
}