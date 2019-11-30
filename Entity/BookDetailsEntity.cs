using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnkurPrathisthan.Entity
{
    public class BookDetailsEntity
    {
        public string BookName { get; set; }      
        public string PublisherName  { get; set; }   
        public string AuthorName  { get; set; }   
        public string Price  { get; set; }
        public string Stock { get; set; }
        public string CategoryName  { get; set; }   
        public string Language  { get; set; }   
        public string EmailID { get; set; }
        public string cmd { get; set; }
        public string LanguageID { get; set; }
        public string PublisherID { get; set; }
        public string CategoryID { get; set; }
        public string BookID { get; set; }
        public string Message { get; set; }

    }   

    public class CategoryDetails
    {
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CreatedBy { get; set; }
        public string Message { get; set; }
        public string ModifiedBy { get; set; }

    }

    public class LanguageDetails
    {
        public string LanguageID { get; set; }
        public string LanguageName { get; set; }
        public string CreatedBy { get; set; }
        public string Message { get; set; }
        public string ModifiedBy { get; set; }  
    }

    public class PublisherDetails
    {
        public string PublisherID { get; set; }
        public string PublisherName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Message { get; set; }
    }

}