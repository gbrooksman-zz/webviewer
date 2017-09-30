using System;
using System.Collections.Generic;
using webviewer.Models;

namespace webviewer.Managers
{

    public interface IDataManager
    {
        List<Format> GetFormats();

        List<Subformat> GetSubformats();

        Document GetDocument(SearchParameters searchParams);

        List<Document> GetDocuments(SearchParameters searchParams);    

    }



}