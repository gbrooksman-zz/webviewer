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
    public  class CardManager
    {
        public CardManager()
        {

        }

        public string RenderResults(List<Document> docs, SearchParameters searchParams)
        {
            StringBuilder sb = new StringBuilder();

            string searchString = RenderSearchString(searchParams);
 
            sb.Append("<div id='divMain'>");

            sb.Append(searchString);            

            foreach (Document doc in docs)
            {
                sb.Append("<div class='card' style='display: inline-block;vertical-align: top;'");
                sb.AppendFormat(" onclick = docGuidClick('{0}')>", doc.RecordID);
                sb.AppendFormat("<p>Product Name: {0}<br/>", doc.ProductName);
                sb.AppendFormat("Language: {0}<br/>", doc.Language);
                sb.AppendFormat("Template: {0}, Publoished on {1}</p>", doc.Subformat, doc.PublishedDate.ToShortDateString());
                sb.Append("</div>");
            }

            sb.Append("<div>");

            return sb.ToString();        
        }

        private string RenderSearchString(SearchParameters searchParams)
        {
            StringBuilder sb = new StringBuilder();

            string ss = "Search Criteria: ";

            if (!string.IsNullOrEmpty(searchParams.ProductID))
            {
                ss += " Product Id:  " +  searchParams.ProductID;
            }

            if (!string.IsNullOrEmpty(searchParams.ProductName))
            {
                 ss += " Product Name: " + searchParams.ProductName;
            }

            if (!string.IsNullOrEmpty(searchParams.Format))
            {
                ss += " Format:  " +  searchParams.Format;
            }

            if (!string.IsNullOrEmpty(searchParams.Subformat))
            {
                ss += " Subformat: " +   searchParams.Subformat;
            }

            if (!string.IsNullOrEmpty(searchParams.Language))
            {
                 ss += " Language: " +  searchParams.Language;
            }

            if (searchParams.Authorization > 0 )
            {
                 ss += " Authorization: " +  searchParams.AuthorizationDescription;
            }

            sb.AppendFormat("<div class='well' style='width:95vw;'>{0}</div>",ss);

            return sb.ToString();
        }



    }
}