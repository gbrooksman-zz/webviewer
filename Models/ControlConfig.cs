using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webviewer.Models
{
    public class ControlConfig
    {
        public ControlConfig() { }

        public List<WebViewerControl> Header { get; set; }
        public List<WebViewerControl> Controls { get; set; }
        public List<WebViewerControl> Footer { get; set; }

    }

    public class WebViewerControl
    {
        public int Size { get; set; }

        public string Type { get; set; }

        public string Action { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

         public string FieldName { get; set; }

        public bool HasFilterDropDown  { get; set; }
    } 
}