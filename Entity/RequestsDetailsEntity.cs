using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnkurPrathisthan.Entity
{
    public class RequestsDetailsEntity
    {
        public string RequestedFor {get; set;}
        public string BookName { get; set; }
        public string AuthorName { get; set; }
        public string ClusterName { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedDate { get; set; }

    }
}