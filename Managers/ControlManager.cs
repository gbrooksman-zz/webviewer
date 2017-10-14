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
        DataManager dataMgr = new DataManager();

        public ControlManager(IConfiguration _iconfiguration)
        {

        }

        public  string CreateTextBox(WebViewerControl control )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='row' style='padding:10px;'>");
                sb.Append("<div class='col-sm-2'>");
                    AddLabel(sb, control.DisplayName); 
                sb.Append("</div>");  
                sb.Append("<div class='col-sm-5'>");         
                    sb.AppendFormat("<input asp-for = '{0}' class='form-control' ", control.Name);
                    sb.AppendFormat("id = 'txt{0}' ", control.Name);
                    sb.AppendFormat("name = '{0}' ", control.Name);
                    sb.AppendFormat("size = {0} />", control.Size);  
                sb.Append("</div>");                   
                if (control.HasFilterDropDown)
                {
                    sb.Append("<div class='col-sm-5'>");
                        AddFilterDropdown(sb, control.Name);
                    sb.Append("</div>");          
                }               
            sb.Append("</div>"); 
            return sb.ToString();
        }

        public string CreateDatePcker(WebViewerControl control )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='row' style='padding:10px;'>");
                sb.Append("<div class='col-sm-2'>");
                    AddLabel(sb, control.DisplayName); 
                sb.Append("</div>");  
                sb.Append("<div class='col-sm-5'>");           
                    sb.AppendFormat("<input asp-for = '{0}' ", control.Name);
                    sb.AppendFormat("id = 'txt{0}' ", control.Name);
                    sb.AppendFormat("name = '{0}' ", control.Name);
                    sb.Append("type = 'date' />");                               
                    if (control.HasFilterDropDown)
                    {
                        sb.Append("<div class='col-sm-5'></div>");   
                            AddFilterDropdown(sb, control.Name);
                        sb.Append("</div>");   
                    }
                sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }


        public  string CreateButton(WebViewerControl control)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='row' style='padding:10px;'>");
                sb.Append("<div class='col-sm-2'>");
                    sb.AppendFormat("<button type = 'submit' name = '{0}' class='btn btn-primary'>{1} " , control.DisplayName, control.DisplayName);
                    sb.Append("</button>");
                sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }


        public  string CreateDropDown(WebViewerControl control)
        {
            StringBuilder sb = new StringBuilder();

             sb.Append("<div class='row' style='padding:10px;'>");
                sb.Append("<div class='col-sm-2'>");
                    AddLabel(sb, control.DisplayName);
                sb.Append("</div>");  
                sb.Append("<div class='col-sm-5'>");   
                    sb.AppendFormat("<select asp-for = '{0}' name='{1}'>", control.Name,control.Name);
                        if (control.Name == "Format")
                        {
                            foreach (var item in GetFormats())
                            {
                                sb.AppendFormat("<option value = '{0}'>{1}</option>",item.Value, item.Text);
                            }
                        }
                        else
                        {
                            foreach (var item in GetSubFormats())
                            {
                                sb.AppendFormat("<option value = '{0}'>{1}</option>",item.Value, item.Text);
                            }
                        }
                    sb.Append("</select>");
                sb.Append("</div>");
            sb.Append("</div>");          
            return sb.ToString();
        }


        public string CreateLogo(WebViewerControl control)
        {
            StringBuilder sb = new StringBuilder();            
            sb.AppendFormat("<img src={0} height = '{1}' width = '{2}' style='padding:10px;' alt='logo' />", "../images/" + control.Name, control.Size, control.Size); 
            return sb.ToString();         
        }
      
        public string CreateLabel(WebViewerControl control, string headingTagNumber)
        {
            StringBuilder sb = new StringBuilder();            
            if (!string.IsNullOrEmpty(headingTagNumber)) sb.AppendFormat("<h{0}>", headingTagNumber);
            AddLabel(sb,control.DisplayName);
            if (!string.IsNullOrEmpty(headingTagNumber)) sb.AppendFormat("</h{0}>", headingTagNumber);            
            return sb.ToString();         
        }


        private  void AddLabel(StringBuilder sb, string modelItemName)
        {
            sb.Append(modelItemName);            
        }

        // private  void AddBreak(StringBuilder sb, int count = 2)
        // {
        //     for (int x = 0; x <= count; x++) 
        //     {       
        //         sb.Append("<br/>");
        //     }
        // }

        private  void AddFilterDropdown(StringBuilder sb, string modelItemName)
        {            
            sb.AppendFormat("<select id = '{0}' name = '{1}'  asp-for = '{2}'>","ddFilter" +  modelItemName, modelItemName + "Filter", modelItemName + "Filter");
            sb.AppendFormat("<option selected 6= 'selected' value = '{0}'>{1}</option>","eq","Equals");
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