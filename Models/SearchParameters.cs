using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webviewer.Models
{
    public class SearchParameters
    {
        //what to search for
        public SearchParameters() { }

        [Display(AutoGenerateFilter = true)]
        public string ProductID {get; set;}

        [Display(AutoGenerateFilter = true)]
        public string ProductName{ get; set; }

        [Display(AutoGenerateFilter = true)]
        public DateTime PublishedDate { get; set; }

        [Display(AutoGenerateFilter = true)]
        public string Format { get; set; }

        [Display(AutoGenerateFilter = true)]
        public string Subformat { get; set; }

        [Display(AutoGenerateFilter = true)]
        public string Language { get; set; }

        [Display(AutoGenerateFilter = true)]
        public int Authorization { get; set; }

        [Display(AutoGenerateFilter = true)]
        public string CasNumber { get; set; }

        [Display(AutoGenerateFilter = true)]
        public string ComponentID { get; set; }

        [Display(AutoGenerateFilter = true)]
        public string ChemicalName { get; set; }






    }
}