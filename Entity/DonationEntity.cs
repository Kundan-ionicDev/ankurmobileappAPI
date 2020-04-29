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
        public string ImgPath {get;set;}
        public string AdminEmail { get; set; }
        public string Img { get; set; }
       // public string Password { get; set; }
        public string Message { get; set; }
        public string RoleID { get; set; }
    }

    public class DonorEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
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
        
    }
    
}