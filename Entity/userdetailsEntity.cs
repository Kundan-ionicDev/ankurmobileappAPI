using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnkurPrathisthan.Entity
{
    public class userdetailsEntity
    {
        public string Message { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }       
        public string Dob { get; set; }
        public string Mobile { get; set; }
        public string Rolename { get; set; }
        public int  RoleID { get; set; }  
        public string ClusterName { get; set; }
        public string FullName{ get; set; }
        public int ClusterCode { get; set; }
        
    }

    public class ForgotPasswordEntity
    {
        public string OTP { get; set; }
        public string Email { get; set; }

    }
}