using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnkurPrathisthan.Entity
{
    public class ClusterDetailsEntity
    {
        public string ClusterID { get; set; }
        public string ClusterName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }        
        public string ClusterCode { get; set; }
        public string MobileNo { get; set; }       
        public string LibrarianID { get; set; }
        public string Members { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
    }

    public class ClusterHeadEntity
    {
        public string  ClusterHeadID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public string AltMobileNo { get; set; }
        public string ClusterRegionID { get; set; }        
        public string Address { get; set; }
        public string AdminEmailID { get; set; }
       
    }
}