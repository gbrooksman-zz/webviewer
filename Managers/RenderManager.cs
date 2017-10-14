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


namespace webviewer.Managers
{
    public  class RenderManager
    {
        public RenderManager(){}

        public string RenderBody( ControlConfig _config, ControlManager controlMgr)
        {
            List<WebViewerControl> bodyControls = _config.Controls;

            StringBuilder sb = new StringBuilder();
 
            sb.Append("<div id='divMain'>");

            foreach (WebViewerControl control in _config.Controls)
            {
                switch (control.Type)
                {
                    case "TextBox":
                        sb.Append(controlMgr.CreateTextBox(control));
                        break;
                    case "DatePicker":
                        sb.Append(controlMgr.CreateDatePcker(control));
                        break;

                    case "Button":  //always a submit type
                        sb.Append(controlMgr.CreateButton(control));
                        break;

                    case "DropDown":
                        sb.Append(controlMgr.CreateDropDown(control));
                        break;
                }
            }

            sb.Append("</div>");  //row 

            return sb.ToString();
        }

        public string RenderHeader(ControlConfig config, ControlManager controlMgr)
        {
            List<WebViewerControl> headerControls = config.Header;

            StringBuilder sb = new StringBuilder();

            sb.Append("<div id='divHeader'>");

             //render header
            if (headerControls.Count > 0)
            {
                foreach (WebViewerControl control in headerControls)
                {
                    switch (control.Type)
                    {                   
                        case "CompanyLogo":
                            sb.Append(controlMgr.CreateLogo(control));
                            break;
                        case "Label":
                            sb.Append(controlMgr.CreateLabel(control,"2"));
                            break;
                    }
                }
            }

            sb.Append("</div>");  

            return sb.ToString();
        }

        public string RenderFooter(ControlConfig config, ControlManager controlMgr)
        {
            List<WebViewerControl> footerControls = config.Footer;

            StringBuilder sb = new StringBuilder();

            sb.Append("<div id='divFooter'>");
            //render footer
            if (footerControls.Count > 0)
            {
                foreach (WebViewerControl control in footerControls)
                {
                    switch (control.Type)
                    {                   
                        case "CompanyLogo":
                            sb.Append( controlMgr.CreateLogo(control));
                            break;
                        case "Label":
                            sb.Append(controlMgr.CreateLabel(control, null));
                            break;
                    }            
                }
            }

            sb.Append("</div>");  //row

            return sb.ToString();
        }


    }

}