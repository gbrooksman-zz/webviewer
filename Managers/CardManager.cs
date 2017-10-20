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
        public class CardManager
    {
        public CardManager()
        {

        }

        public string RenderResults(List<Document> docs, SearchParameters searchParams, CoreEntities coreEntities)
        {
            StringBuilder sb = new StringBuilder();

            string searchString = RenderSearchString(searchParams, docs.Count);
 
            sb.Append("<div id='divMain'>");

            sb.Append(searchString);            

            foreach (Document doc in docs)
            {
                string langName = coreEntities.LanguageList
                                    .Where(l => l.Code == doc.Language)
                                    .FirstOrDefault().Name; 

                string fmt = coreEntities.FormatList
                                    .Where(f => f.Code == doc.Format)
                                    .FirstOrDefault().Name;

               string subFmt = coreEntities.SubformatList
                                    .Where(s => s.FormatCode == doc.Format && s.Code == doc.Subformat)
                                    .FirstOrDefault().Name;

                sb.Append("<div class='card' style='display:inline-block;vertical-align:top;padding:5px;'");
                sb.AppendFormat(" onclick = docGuidClick('{0}')>", doc.RecordID);
                sb.AppendFormat("<p>Product Name: {0}</p>", doc.ProductName);
               // sb.Append("<img src='../images/pdf.jpg' height='40px' width='40px' alt='PDF'/><br/>");
                sb.AppendFormat("<p>Language: {0}<br/>", langName);
                sb.AppendFormat("Template: {0}, Published on {1}</p>", subFmt, doc.PublishedDate.ToShortDateString());
                
                if (searchParams.Authorization > -99)
                {
                    sb.AppendFormat("<p>For {0} </p>", coreEntities.AuthorizationList
                                                        .Where(a => a.Level == doc.Authorization)
                                                        .FirstOrDefault().Description);
                }

                sb.Append("</div>");
            }

            sb.Append("<div>");

            return sb.ToString();        
        }

        private string RenderSearchString(SearchParameters searchParams, int docCount)
        {
            StringBuilder sb = new StringBuilder();

            string strLeft = "Search Criteria: ";

            if (!string.IsNullOrEmpty(searchParams.ProductID))
            {
                strLeft += " Product Id:  " +  searchParams.ProductID;
            }

            if (!string.IsNullOrEmpty(searchParams.ProductName))
            {
                 strLeft += " Product Name: " + searchParams.ProductName;
            }

            if (!string.IsNullOrEmpty(searchParams.Format) && (searchParams.Format != "%"))
            {
                strLeft += " Format:  " +  searchParams.Format;
            }

            if (!string.IsNullOrEmpty(searchParams.Subformat) && (searchParams.Subformat != "%"))
            {
                strLeft += " Subformat: " +   searchParams.Subformat;
            }

            if (!string.IsNullOrEmpty(searchParams.Language) && (searchParams.Language != "%"))
            {
                 strLeft += " Language: " +  searchParams.Language;
            }

            if (searchParams.Authorization > -99 )
            {
                 strLeft += " Authorization: " +  searchParams.AuthorizationDescription;
            }

            string strRight = "<div id='divSSRight' Matching Documents: " + docCount.ToString() + "</div>";

            sb.AppendFormat("<div class='well' style='width:95vw;'>{0}</div>",strLeft);

            return sb.ToString();
        }
    }
}