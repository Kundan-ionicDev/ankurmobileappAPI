using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnkurPrathisthan.Entity
{
    public class VolunteerEntity
    {
        public string LoginID { get; set; }
        public string FirstName {get;set;} 
        public string LastName {get;set;}
        public string EmailID {get;set;}
        public string DOB {get;set;}
        public string ContactNo {get;set;}
        public string Address {get;set;}        
        public string AdminEmail { get; set; }
        public string Img { get; set; }
        public string ImgPath { get; set; }
        public string Message { get; set; }
        public string RoleID { get; set; }
        public string Version { get; set; }
    }

    public class DonorEntity
    {
        public string FullName { get; set; }
        public string Inthenameof { get; set; }
        public string EmailID { get; set; }
        public string DOB { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public int Amount { get; set; }
        public string PaymentMode { get; set; }
        public string ImgPath { get; set; }
        public string AdminEmailID { get; set; }
        public string Description { get; set; }
        public string RegDate { get; set; }
        public string PAN { get; set; }
        public string Amountinwords { get; set; }
        public string BirthdayFlag { get; set; }
        public string DonationTowards { get; set; }
        public string DonorID { get; set; }
        public int TemporaryFlag { get; set; }
        public string VolEmailID { get; set; }
        public string CreatedBy { get; set; }
        
    }

    public class GetSlides
    {
        public string ID { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string PDFPath { get; set; }
        public string Shareable { get; set; }

    }

    public class ContactUs
    {
        public string FullName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Comments { get; set; }
        public string Subject { get; set; }
        public int TicketID { get; set; }
    }
    
}