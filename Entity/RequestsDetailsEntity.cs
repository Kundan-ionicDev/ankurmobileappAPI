using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnkurPrathisthan.Entity
{
    public class RequestsDetailsEntity
    {
        public string LibrarianName {get; set;}
        public string LibrarianMobNo { get; set; }
        public string BookName { get; set; }        
        public string ClusterName { get; set; }
        public string LibrarianID { get; set; }
        public string RequestedDate { get; set; }
        public string MemberID { get; set; }
        public string RequestID{ get; set; }
        public string MemberName{ get; set; }
        public string BookID { get; set; }
        public string RequestAcceptDate { get; set; }
        public string ClusterID { get; set; }
        public string Status { get; set; }
        public string RequestStatus { get; set; }
        public string RequestDelDate{ get; set; }
        public string RequestRetDate { get; set; }
        public string Message { get; set; }
        public string FCM { get; set; }
        public string ClusterContactNo { get; set; }
        public string Stock { get; set; }
        public string BooksAvailable { get; set; }
        public string BooksUnAvailable { get; set; }

    }
}