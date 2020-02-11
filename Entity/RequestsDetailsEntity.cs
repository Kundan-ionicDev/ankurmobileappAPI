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
        public int LibrarianID { get; set; }
        public string RequestedDate { get; set; }
        public int MemberID { get; set; }
        public int RequestID{ get; set; }
        public string MemberName{ get; set; }
        public int BookID { get; set; }
        public int AuthorID { get; set; }
        public int ClusterID { get; set; }
        public string Status { get; set; }
        public string RequestStatus { get; set; }
    }
}