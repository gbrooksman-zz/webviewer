using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webviewer.Models;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;


namespace webviewer.Managers
{
    public  class ControlManager
    {
        //  when using taghelpers, for the value of the input to be submitted as form data, 
        //  it needs to have a 'name' attribute. not too obvious
        DataManager dataMgr ;

        public ControlManager(IConfiguration _iconfiguration)
        {
            dataMgr = new DataManager();
        }

        public  string CreateTextBox(WebViewerControl control )
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


        public  string CreateDatePcker(WebViewerControl control )
        {
            StringBuilder builder = new StringBuilder();

            AddLabel(builder, control.Name);            
            builder.AppendFormat("<input asp-for = '{0}' ", control.Name);
            builder.AppendFormat("id = 'txt{0}' ", control.Name);
            builder.AppendFormat("name = '{0}' ", control.Name);
            builder.Append("type = 'date' ");
            builder.Append("</input>");
            if (control.HasFilterDropDown)
            {
                AddFilterDropdown(builder, control.Name);
            }
            AddBreak(builder);
            return builder.ToString();
        }


        public  string CreateButton(WebViewerControl control)
        {
            StringBuilder builder = new StringBuilder();
            AddBreak(builder);
            builder.AppendFormat("<input type = 'submit' name = '{0}' " , control.DisplayName);
            builder.Append("</input>");
            AddBreak(builder);
            return builder.ToString();
        }


        public  string CreateDropDown(WebViewerControl control)
        {
            StringBuilder builder = new StringBuilder();
            AddLabel(builder, control.Name);
            builder.AppendFormat("<select asp-for = '{0}' name='{1}'>", control.Name,control.Name);

            if (control.Name == "Format")
            {
                foreach (var item in GetFormats())
                {
                    builder.AppendFormat("<option value = '{0}'>{1}</option>",item.Value, item.Text);
                }
            }
            else
            {
                foreach (var item in GetSubFormats())
                {
                    builder.AppendFormat("<option value = '{0}'>{1}</option>",item.Value, item.Text);
                }
            }
            builder.Append("</select>");
            AddBreak(builder);
            return builder.ToString();
        }


        public string CreateLogo(WebViewerControl control)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<img src={0}  height='{1}' width='{2}' />", "../images/" + control.Name, control.Size, control.Size); 
            return builder.ToString();         
        }

        public string CreateLabel(WebViewerControl control)
        {
            StringBuilder builder = new StringBuilder();
            AddLabel(builder,control.DisplayName,control.Size);
            return builder.ToString();         
        }


        private  void AddLabel(StringBuilder sb, string modelItemName, int size = 15)
        {
            sb.AppendFormat("<label style = 'font-size:{0}px;'>{1}</label>",size,modelItemName);
            AddBreak(sb,1);
        }

        private  void AddBreak(StringBuilder sb, int count = 2)
        {
            for (int x = 0; x <= count; x++) 
            {       
                sb.Append("<br/>");
            }
        }

        private  void AddFilterDropdown(StringBuilder sb, string modelItemName)
        {
            sb.AppendFormat("<select id = '{0}' name = '{1}'  asp-for = '{2}'>","ddFilter" +  modelItemName, modelItemName + "Filter", modelItemName + "Filter");
            sb.AppendFormat("<option selected = 'selected' value = '{0}'>{1}</option>","eq","Equals");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","sw", "Starts With");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","ew","Ends With");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","ct","Contains");
            sb.Append("</select>");
        }

        private  void AddDateFilterDropdown(StringBuilder sb, string modelItemName)
        {
            sb.AppendFormat("<select id = '{0}' name = '{1}' asp-for = '{2}' >", "ddDateFilter" +  modelItemName , modelItemName + "Filter" , modelItemName + "Filter");
            sb.AppendFormat("<option selected = 'selected' value = '{0}'>{1}</option>","on", "On");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","ob","On or Before");
            sb.AppendFormat("<option value = '{0}'>{1}</option>","oa","on or After");
            sb.Append("</select>");
        }

        private  List<SelectListItem> GetFormats()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            List<Format> formats  = dataMgr.GetFormats();

            foreach(var format in formats)
            {
                items.Add(new SelectListItem
                {
                    Text = format.Name,
                    Value = format.Code
                });
            }           

            return  items;
        }


        private  List<SelectListItem> GetSubFormats()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            List<Subformat> subformats  = dataMgr.GetSubFormats("MTR");

            foreach(var subformat in subformats)
            {
                items.Add(new SelectListItem
                {
                    Text = subformat.Name,
                    Value = subformat.Code
                });
            }           

            return  items;
        }


    }
}