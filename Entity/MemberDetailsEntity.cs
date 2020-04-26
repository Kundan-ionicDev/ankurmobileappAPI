using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AnkurPrathisthan.Entity
{
    public class MemberDetailsEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MemberName { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string AltMobileNo { get; set; }
        public string EmailID { get; set; }
        public string DOB { get; set; }
        public string ClusterName { get; set; }
        public string ClusterID { get; set; }
        public string MemberID { get; set; }
        public string Image{ get; set; }
    }

    public class GetLatestShayari
    {
        public string Msg { get; set; }
        public string Category { get; set; }
    }
}