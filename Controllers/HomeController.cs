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
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using webviewer.Managers;
using webviewer.Models;
using System.Text;

namespace webviewer.Controllers
{
    public class HomeController : Controller
    {
 
        private readonly ControlConfig _config;
        private IConfiguration _iconfiguration;
        private DataManager dataMgr = new DataManager();
        private CardManager cardMgr = new CardManager();
        private RenderManager renderMgr = new RenderManager();

        public HomeController(IOptions<ControlConfig> controlConfigAccessor, 
                              IConfiguration iconfiguration)
        {
            _config = controlConfigAccessor.Value;
             _iconfiguration = iconfiguration;
        }

        [HttpPost]
        public IActionResult Result(SearchParameters searchParams)
        {     
            // for DEV only
            searchParams.ProductID = "DTE73";
            searchParams.Format = "MTR";
            searchParams.Subformat = "DATA";
            // for DEV only

            var controlMgr = new ControlManager(_iconfiguration);

            ViewData["headerString"] =renderMgr. RenderHeader( _config, controlMgr);

            List<Document> docs = dataMgr.GetDocuments(searchParams);
            ViewData["resultString"] = cardMgr.RenderResults(docs,searchParams);

            ViewData["footerString"] = renderMgr.RenderFooter(_config,controlMgr );

            return View("/Views/Home/Result.cshtml");
        }

        public IActionResult Index()
        {
            string searchControls = string.Empty;

            var controlMgr = new ControlManager(_iconfiguration);

            ViewData["headerString"] = renderMgr.RenderHeader( _config, controlMgr);           

            ViewData["bodyString"] = renderMgr.RenderBody(_config, controlMgr );

            ViewData["footerString"] = renderMgr.RenderFooter(_config, controlMgr );

            return View();
        }


    }
}