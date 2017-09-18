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
        private DataManager dataMgr;

        public HomeController(IOptions<ControlConfig> controlConfigAccessor, IConfiguration _iconfiguration)
        {
            _config = controlConfigAccessor.Value;
             dataMgr = new DataManager(_iconfiguration);
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

            ViewData["resultString"] = "";  

            return View("/Views/Home/Result.cshtml");
        }

        public IActionResult Index()
        {
            string searchParams = string.Empty;

            foreach (WebViewerControl control in _config.Controls)
            {
                switch (control.Type)
                {
                    case "TextBox":
                        searchParams += ControlManager.CreateTextBox(control);
                        break;

                    case "Button":  //always a submit type
                        searchParams += ControlManager.CreateButton(control);
                        break;

                    case "DropDown":
                        searchParams += ControlManager.CreateDropDown(control);
                        break;
                }
            }

            ViewData["paramsString"] = searchParams;           

            return View();
        }


    }
}