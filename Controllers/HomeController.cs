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
    public class HomeController : Controller
    { 
        private IMemoryCache _cache;
        private readonly ControlConfig _config;
        private IConfiguration _iconfiguration;
        private DataManager dataMgr;
       
        private static CoreEntities coreEntities = new CoreEntities();
        private CardManager cardMgr = new CardManager();
        
        private RenderManager renderMgr = new RenderManager();

        public HomeController(IOptions<ControlConfig> controlConfigAccessor, 
                              IConfiguration iconfiguration,
                              IMemoryCache memoryCache)
        {
            _config = controlConfigAccessor.Value;
            _iconfiguration = iconfiguration;
            _cache = memoryCache;
            dataMgr = new DataManager(_cache,_iconfiguration);      

            coreEntities.AuthorizationList = dataMgr.GetsAuthLevels();
            coreEntities.FormatList = dataMgr.GetFormats();
            coreEntities.SubformatList = dataMgr.GetSubFormats();
            coreEntities.LanguageList = dataMgr.GetLanguages();                 
        }

        [HttpPost]
        public IActionResult Result(SearchParameters searchParams)
        {     
            // for DEV only _iconfiguration = iconfiguration;
           searchParams.ProductID = "DTE73";
           searchParams.Format = "MTR";
           searchParams.Subformat = "DATA";
            // for DEV only

            var controlMgr = new ControlManager(_iconfiguration, _cache);

            ViewData["headerString"] = GetOrSetCacheEntry("header",controlMgr); 

            searchParams.AuthorizationDescription 
                        =  coreEntities.AuthorizationList
                                .Where(a => a.Level == searchParams.Authorization)
                                .FirstOrDefault().Description;

            searchParams.LanguageName 
                    =  coreEntities.LanguageList
                        .Where(l=>l.Code == searchParams.Language)
                        .FirstOrDefault().Name;

            List<Document> docs = dataMgr.GetDocuments(searchParams);
            ViewData["resultString"] = cardMgr.RenderResults(docs,searchParams, coreEntities);

            ViewData["footerString"] = GetOrSetCacheEntry("footer",controlMgr);

            return View("/Views/Home/Result.cshtml");
        }

        public IActionResult Index()
        {
            string searchControls = string.Empty;

            var controlMgr = new ControlManager(_iconfiguration, _cache);

            ViewData["headerString"] = GetOrSetCacheEntry("header",controlMgr);           

            ViewData["bodyString"] = GetOrSetCacheEntry("index_body",controlMgr); 

            ViewData["footerString"] = GetOrSetCacheEntry("footer",controlMgr); 

            return View();
        }

        [HttpGet]
        public IActionResult GetSubformats(string Format)
        {
            List<Subformat> subformats = coreEntities.SubformatList
                                            .Where(s => s.FormatCode == Format)                
                                            .ToList();            
            return Json(subformats);
        }

        private string GetOrSetCacheEntry(string cacheKey, ControlManager controlMgr )
        { 
            string cacheValue = string.Empty;

            if (!_cache.TryGetValue(cacheKey, out cacheValue))
            {                
                switch (cacheKey)
                {
                    case "footer":
                        cacheValue = renderMgr.RenderFooter(_config, controlMgr);
                        break;
                    case "header":
                        cacheValue = renderMgr.RenderHeader( _config, controlMgr);
                        break;
                    case "index_body":
                        cacheValue = renderMgr.RenderBody(_config, controlMgr);
                        break;
                }   

                 var cacheEntryOptions = new MemoryCacheEntryOptions()
                                                .SetPriority(CacheItemPriority.NeverRemove);            
                
                _cache.Set(cacheKey, cacheValue, cacheEntryOptions);
            }

            return cacheValue;
        }
    }
}