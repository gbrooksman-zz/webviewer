using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using webviewer.Managers;
using Wercs.DTE.WebViewer.Library.Models;


namespace webviewer.Controllers
{
    public class ResultController : Controller
    {
        private readonly ControlConfig _config;

        public ResultController(IOptions<ControlConfig> controlConfigAccessor)
        {
            _config = controlConfigAccessor.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string resultParams = string.Empty;

           // ResultGrid grid = _config.Grid;

           // resultParams = grid.Title;

            ViewData["resultString"] = resultParams;   


            return View("Shared/Result.cshtml");

        }
    }



}