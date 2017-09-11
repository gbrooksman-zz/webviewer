using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webviewer
{
    public class ControlConfig
    {
        public ControlConfig() { }

        public List<WebViewerControl> Controls { get; set; }

        public ResultGrid Grid { get; set; }

        public string ResultGridTitle { get; set; }
    }

    public class WebViewerControl
    {
        public int Size { get; set; }

        public string Type { get; set; }

        public string Action { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }
    }


    public class ResultGrid
    {
       public ResultGrid(){}

       public List<Column> Columns { get; set; }
    }

    public class Column
    {
        public Column(){}
        public string Name { get; set; }

        public string DisplayName { get; set; }

         public string Type { get; set; }

         public int Size { get; set; }

        public string TextCode { get; set; }
    }
}