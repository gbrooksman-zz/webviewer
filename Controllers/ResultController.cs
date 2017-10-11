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
using webviewer.Models;


namespace webviewer.Controllers
{
    public class ResultController : Controller
    {
        private readonly ControlConfig _config;

        public ResultController(IOptions<ControlConfig> controlConfigAccessor)
        {
            _config = controlConfigAccessor.Value;
        }
       
    }

}