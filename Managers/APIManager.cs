using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wercs.DTE.WebViewer.Library.Models;
using System.Text;
using System.Net.Http;


namespace webviewer.Managers
{
    public class DataManager
    {

    static string baseURL = "http://localhost:5001/";

    private static async Task<string> GetSearchResults(SearchParameters searchParams)
    {
        using (var httpClient = new HttpClient())
        {
            return await httpClient.GetStringAsync(baseURL);
        }
    }

    }

}