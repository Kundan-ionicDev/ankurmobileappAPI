﻿using AnkurPrathisthan.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Collections;
namespace AnkurPrathisthan
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAPService
    {
        //[START] For Authentication
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "UserLogin")]
        [OperationContract]
        userdetailsEntity UserLogin(string EmailID, string Password, string deviceinfo, string isnewapp);

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "UserLogout")]
        [OperationContract]
        string UserLogout(string EmailID);

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "UserRegister")]
        [OperationContract]
        List<userdetailsEntity> UserRegister(string FirstName, string LastName, string EmailID, string Password, string RoleID, string ClusterCode,
        string MobileNo = "");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ForgotPassword")]
        [OperationContract]
        userdetailsEntity ForgotPassword(string EmailID, string Password);
        //[END] Added by ARya For Authentication
        //[STARt] For Book Management

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetBooks")]
        [OperationContract]
        List<BookDetailsEntity> GetBooks();

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageBooks")]
        [OperationContract]
        List<BookDetailsEntity> ManageBooks(string BookName, string cmd, string EmailID, string Price, string Author, string Stock, string CategoryID,
        string LanguageID, string PublisherID, string BookID = "");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageCategories")]
        [OperationContract]
        List<CategoryDetails> ManageCategories(string cmd, string CategoryName, string Email, string CategoryID = "");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageLanguages")]
        [OperationContract]
        List<LanguageDetails> ManageLanguages(string cmd, string LanguageName, string Email, string LanguageID = "");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManagePublishers")]
        [OperationContract]
        List<PublisherDetails> ManagePublishers(string cmd, string PublisherName, string Email, string PublisherID = "");
        //[END] For Book Management

        //[Start] For Cluster Management
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetClusters")]
        [OperationContract]
        List<ClusterDetailsEntity> GetClusters();

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageClusters")]
        [OperationContract]
        List<ClusterDetailsEntity> ManageClusters(string ClusterName, string ClusterCode, string cmd, string EmailID, string Address, string MobileNo,
        string LibEmailID, string Members, string AdminEmailID, string ClusterID = "");

        //[END] For Cluster Management
        //[START] For Librarian Management
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetLibrarians")]
        [OperationContract]
        List<LibrarianDetailsEntity> GetLibrarians();        

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageLibrarians")]
        [OperationContract]
        List<LibrarianDetailsEntity> ManageLibrarians(int cmd, string FirstName, string LastName, string EmailID, string Address, string MobileNo, 
        string AltMobileNo,string ClusterID,string AdminEmailID, string LibrarianID="");
        //[END] For Librarian Management

        //[START] For Member Management
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetMembers")]
        [OperationContract]
        List<MemberDetailsEntity> GetMembers();        

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageMembers")]
        [OperationContract]
        List<MemberDetailsEntity> ManageMembers(int cmd, string FirstName, string LastName, string EmailID, string Address, string MobileNo, 
        string AltMobileNo, string ClusterID, string DOB, string AdminEmailID, string MemberID = "");
        //[END] For Member Management

        //[START] For Approvals Module
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetRequests")]
        [OperationContract]
        List<RequestsDetailsEntity> GetRequests();

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageRequests")]
        [OperationContract]
        List<RequestsDetailsEntity> ManageRequests(int cmd, string FirstName, string LastName, string BookID, string LibrarianID,
        string MemberID, string ClusterID, string RequestID, int AuthorID, string RequestStatus);  
        //[END] For Approvals Module

        //[START]

        //GetVersionDetails
        //GetBooksList

        //[END]

    }
      
}
