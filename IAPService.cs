﻿using AnkurPrathisthan.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AnkurPrathisthan
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAPService
    {
       [WebInvoke(Method = "POST",
       RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
       UriTemplate = "UserLogin")]
       [OperationContract]
       userdetailsEntity UserLogin(string EmailID, string Password, string deviceinfo, string isnewapp);
       
    }   

      
}
