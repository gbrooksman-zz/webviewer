using System;
using System.Collections.Generic;
using webviewer.Models;

namespace webviewer.Managers
{

    public interface IDataManager
    {
        List<Format> GetFormats();

        List<Subformat> GetSubformats();

        PDFEntity GetDocument(SearchParameters searchParams);

        List<PDFEntity> GetDocuments(SearchParameters searchParams);    

    }



}