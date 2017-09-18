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
        //when using taghelpers, for the value of the input to be submitted as form data, 
        //it needs to have a 'name' attribute. not too obvious

        public static string CreateTextBox(WebViewerControl control )
        {
            StringBuilder builder = new StringBuilder();

            AddLabel(builder, control.Name);            
            builder.AppendFormat("<input asp-for = '{0}' ", control.Name);
            builder.AppendFormat("id = 'txt{0}' ", control.Name);
            builder.AppendFormat("name = '{0}' ", control.Name);
            builder.AppendFormat("size = {0} ", control.Size);
            builder.Append("</input>");
            if (control.HasFilterDropDown)
            {
                AddFilterDropdown(builder, control.Name);
            }
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
            builder.AppendFormat("<select asp-for = '{0}' name='{1}'>", control.Name,control.Name);
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

        private static void AddFilterDropdown(StringBuilder sb, string modelItemName)
        {
            sb.AppendFormat("<select id = '{0}' name = '{1}'  asp-for = '{2}'>","ddFilter" +  modelItemName, modelItemName + "Filter", modelItemName + "Filter");
            sb.AppendFormat("<option selected = 'selected' value = '{0}'>{1}</option>","sw", "Starts With");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","ew","Ends With");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","ct","Contains");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","me","Matches Exactly");
            sb.Append("</select>");
        }

        private static void AddDateFilterDropdown(StringBuilder sb, string modelItemName)
        {
            sb.AppendFormat("<select id = '{0}' name = '{1}' asp-for = '{2}' >", "ddDateFilter" +  modelItemName , modelItemName + "Filter" , modelItemName + "Filter");
            sb.AppendFormat("<option selected = 'selected' value = '{0}'>{1}</option>","on", "On");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","ob","On or Before");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","oa","on or After");
            sb.Append("</select>");
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