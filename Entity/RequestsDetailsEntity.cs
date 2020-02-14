using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnkurPrathisthan.Entity
{
    public class RequestsDetailsEntity
    {
        public string LibrarianName {get; set;}
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string ClusterName { get; set; }
        public string LibrarianID { get; set; }
        public string RequestedDate { get; set; }
        public string MemberID { get; set; }
        public string RequestID{ get; set; }
        public string MemberName{ get; set; }
        public string BookID { get; set; }
        public string AuthorID { get; set; }
        public string ClusterID { get; set; }
        public string Status { get; set; }
        public string RequestStatus { get; set; }
    }
}