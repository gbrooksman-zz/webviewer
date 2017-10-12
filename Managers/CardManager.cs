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

            foreach (Document doc in docs)
            {
                sb.AppendFormat("<div class='card' style='display: inline-block;vertical-align: top;' onclick = docGuidClick('{0}')>", doc.RecordID);
                sb.AppendFormat("<p>Product Name: {0}</p>", doc.ProductName);
                sb.Append("<br/><br/>");
                sb.AppendFormat("<p>Template: {0}, Publoished on {1}</p>", doc.Subformat, doc.PublishedDate);
                sb.Append("</div>");
            }

            return sb.ToString();        
        }






    }

}