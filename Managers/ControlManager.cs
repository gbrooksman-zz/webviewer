using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webviewer.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace webviewer.Managers
{
    public static class ControlManager
    {
        public static string CreateTextBox(WebViewerControl control )
        {
            StringBuilder builder = new StringBuilder();

            AddLabel(builder, control.Name);            
            builder.AppendFormat("<input asp-for = '{0}' ", control.Name);
            builder.AppendFormat("id = 'txt{0}' ", control.Name);
            builder.AppendFormat("size = {0} ", control.Size);
            builder.Append("</input>");
            AddBreak(builder);
            return builder.ToString();
        }

        public static string CreateButton(WebViewerControl control)
        {
            StringBuilder builder = new StringBuilder();
            AddBreak(builder);
            builder.AppendFormat("<input type = 'submit' name = '{0}' " , control.DisplayName);
            builder.Append("</input>");
            AddBreak(builder);
            return builder.ToString();
        }

        public static string CreateDropDown(WebViewerControl control)
        {
            StringBuilder builder = new StringBuilder();
            AddLabel(builder, control.Name);
            builder.AppendFormat("<select asp-for = '{0}'>", control.Name);
            foreach (var item in GetSubFormats())
                builder.AppendFormat("<option value = '{0}'>{1}</option>",item.Value, item.Text);
            builder.Append("</select>");
            AddBreak(builder);
            return builder.ToString();
        }

        private static void AddLabel(StringBuilder sb, string modelItemName)
        {
            sb.AppendFormat("<label>{0}</label>", modelItemName);
            sb.Append("<br/>");
        }

        private static void AddBreak(StringBuilder sb, int count = 2)
        {
            for (int x = 0; x <= count; x++)        
                sb.Append("<br/>");
        }

        private static List<SelectListItem> GetFormats()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "Master",
                Value = "MTR"
            });
            items.Add(new SelectListItem
            {
                Text = "Japan",
                Value = "JPN",
                Selected = true
            });
            items.Add(new SelectListItem
            {
                Text = "Europe",
                Value = "EU"
            });            

            return  items;
        }

        private static List<SelectListItem> GetSubFormats()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = "America",
                Value = "USA"
            });
            items.Add(new SelectListItem
            {
                Text = "Mexico",
                Value = "MEX",
                Selected = true
            });
            items.Add(new SelectListItem
            {
                Text = "Canada",
                Value = "CAN"
            });

            return items;
        }

    }
}