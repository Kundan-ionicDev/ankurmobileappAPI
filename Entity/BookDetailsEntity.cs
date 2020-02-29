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
        public string qrcode { get; set; }
        public string ThumbImage { get; set; }
        public string BookDescription { get; set; }
        public string Image2 { get; set; }
    }

    public class Images
    {
        public string BookID { get; set; }
        public string ClusterID { get; set; }
        public string LibrarianID { get; set; }
        public string MemberID { get; set; }
        public string Ankur{ get; set; }
        public string ClusterHeadID { get; set; }
        public string BookImageId { get; set; }
        public string BookImagePath { get; set; }
        public string BookThumbImage { get; set; }
        public string BookThumbImagePath{ get; set; }
        public string clusterImage { get; set; }
        public string clusterImagePath { get; set; }
        public string LibImg { get; set; }
        public string LibImgpath{ get; set; }
        public string MemberPath { get; set; }
        public string MemberIMGID { get; set; }
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

    public class BookDetails
    {
        public List<PublisherDetails> Publishers { get;set;}
        public List<LanguageDetails> Languages { get; set; }
        public List<CategoryDetails> Categories { get; set; }
        public List<Images> Images { get; set; }
    }

    public class BooksData
    {
        public string  PublisherID { get; set; }
        public string PublisherName{ get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string LanguageID { get; set; }
        public string LanguageName { get; set; }
        public string BookID { get; set; }
        public string ClusterID { get; set; }
        public string LibrarianID { get; set; }
        public string MemberID { get; set; }
        public string Ankur { get; set; }
        public string ClusterHeadID { get; set; }
        public string BookImageId { get; set; }
        public string BookImagePath { get; set; }
        public string BookThumbImage { get; set; }
        public string BookThumbImagePath { get; set; }
        public string clusterImage { get; set; }
        public string clusterImagePath { get; set; }
        public string LibImg { get; set; }
        public string LibImgpath { get; set; }
        public string MemberPath { get; set; }
        public string MemberIMGID { get; set; }



    }
}