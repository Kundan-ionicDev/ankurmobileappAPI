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
        public string Librarian { get; set; }
        public string Members { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Message { get; set; }
    }
}