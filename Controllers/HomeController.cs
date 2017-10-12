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

namespace webviewer.Controllers
{
    public class HomeController : Controller
    {
 
        private readonly ControlConfig _config;

        private IConfiguration _iconfiguration;
        private DataManager dataMgr;
        private CardManager cardMgr;

        public HomeController(IOptions<ControlConfig> controlConfigAccessor, IConfiguration iconfiguration)
        {
            _config = controlConfigAccessor.Value;
             dataMgr = new DataManager();
             _iconfiguration = iconfiguration;
             cardMgr = new CardManager();
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

            ViewData["headerString"] = RenderHeader( _config.Header, controlMgr);

            List<Document> docs = dataMgr.GetDocuments(searchParams);
            ViewData["resultString"] = cardMgr.RenderResults(docs,searchParams);

            ViewData["footerString"] = RenderFooter(_config.Footer,controlMgr );

            return View("/Views/Home/Result.cshtml");
        }

        public IActionResult Index()
        {
            string searchControls = string.Empty;

            var controlMgr = new ControlManager(_iconfiguration);

            ViewData["headerString"] = RenderHeader( _config.Header, controlMgr);

            foreach (WebViewerControl control in _config.Controls)
            {
                switch (control.Type)
                {
                    case "TextBox":
                        searchControls += controlMgr.CreateTextBox(control);
                        break;
                    case "DatePicker":
                        searchControls += controlMgr.CreateDatePcker(control);
                        break;

                    case "Button":  //always a submit type
                        searchControls += controlMgr.CreateButton(control);
                        break;

                    case "DropDown":
                        searchControls += controlMgr.CreateDropDown(control);
                        break;
                }
            }

            ViewData["paramsString"] = searchControls;

            ViewData["footerString"] = RenderFooter(_config.Footer,controlMgr );

            return View();
        }

        private string RenderHeader(List<WebViewerControl> headerControls, ControlManager controlMgr)
        {
            string header = string.Empty;

             //render header
            if (headerControls.Count > 0)
            {
                foreach (WebViewerControl control in headerControls)
                {
                    switch (control.Type)
                    {                   
                        case "CompanyLogo":
                            header += controlMgr.CreateLogo(control);
                            break;
                        case "Label":
                            header += controlMgr.CreateLabel(control);
                            break;
                    }
                }
            }

            return header;
        }


        private string RenderFooter(List<WebViewerControl> footerControls,  ControlManager controlMgr)
        {
            string footer = string.Empty;

            //render footer
            if (footerControls.Count > 0)
            {
                foreach (WebViewerControl control in footerControls)
                {
                    switch (control.Type)
                    {                   
                        case "CompanyLogo":
                            footer += controlMgr.CreateLogo(control);
                            break;
                        case "Label":
                            footer += controlMgr.CreateLabel(control);
                            break;
                    }            
                }
            }

            return footer;

        }
    }
}