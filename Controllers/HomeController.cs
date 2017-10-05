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

        public HomeController(IOptions<ControlConfig> controlConfigAccessor, IConfiguration iconfiguration)
        {
            _config = controlConfigAccessor.Value;
             dataMgr = new DataManager();
             _iconfiguration = iconfiguration;
        }

        [HttpPost]
        public IActionResult Result(SearchParameters searchParams)
        {
           // string resultParams = string.Empty;

           // resultParams = _config.ResultGridTitle;

          //  ResultGrid grid = _config.Grid;

           // foreach (Column col in grid.Columns)
           // {
          //    resultParams += col.DisplayName + "<br/>";                 
          //  }

           // List<Filter> filters = dataMgr.GetFilterCollection(searchParams);

            ViewData["resultString"] = "";  

            return View("/Views/Home/Result.cshtml");
        }

        public IActionResult Index()
        {
            string header = string.Empty;
            string searchParams = string.Empty;
            string footer = string.Empty;

            var controlMgr = new ControlManager(_iconfiguration);
            List<WebViewerControl> headerControls = _config.Header;
            List<WebViewerControl> footerControls = _config.Footer;

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

            //render body

            foreach (WebViewerControl control in _config.Controls)
            {
                switch (control.Type)
                {
                    case "TextBox":
                        searchParams += controlMgr.CreateTextBox(control);
                        break;
                    case "DatePicker":
                        searchParams += controlMgr.CreateDatePcker(control);
                        break;

                    case "Button":  //always a submit type
                        searchParams += controlMgr.CreateButton(control);
                        break;

                    case "DropDown":
                        searchParams += controlMgr.CreateDropDown(control);
                        break;
                }
            }

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

            ViewData["headerString"] = header; 
            ViewData["paramsString"] = searchParams;           
            ViewData["footerString"] = footer; 

            return View();
        }
    }
}