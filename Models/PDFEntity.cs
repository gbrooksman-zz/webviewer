using System;

namespace webviewer.Models
{
    public class PDFEntity
    {
        public PDFEntity(){}

        public Guid RecordID {get; set;}

        public string ProductID {get; set;}
        
        public string ProductName {get; set;}

        public string Format {get; set;}

        public string Subformat {get; set;}

        public string Language {get; set;}

        public string Plant {get; set;}

        public int DocType {get; set;}

        public string DocPath {get; set;}

        public string Supplier {get; set;}

        public DateTime RevisedDate {get; set;}

        public DateTime IssueDate {get; set;}

        public DateTime DisposalDate {get; set;}

        public DateTime PublishedDate {get; set;}

        public int Authorization {get; set;}

        public DateTime DateStamp {get; set;}

        public string UserUpdated {get; set;}

        public string Document {get; set;}

        public string Custom1 {get; set;}

        public string Custom2 {get; set;}

        public string Custom3 {get; set;}

        public string Custom4 {get; set;}

        public string Custom5 {get; set;}

        public string Keywords {get; set;}

        public string CasNumbers { get; set; }

        public string ComponentIDs { get; set; }

        public int AuthorizationLevel { get; set; }

        public float RevisionNumber { get; set; }

        public bool IsS3 { get; set; }

    }
}
