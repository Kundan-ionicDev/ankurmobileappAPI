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
using System.Drawing;
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
        userdetailsEntity UserLogin(string EmailID, string Password, string deviceinfo, string platform, string FCMID, string IMEI);

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "UserLogout")]
        [OperationContract]
        string UserLogout(string EmailID);

        //[WebInvoke(Method = "POST",
        //RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        //UriTemplate = "UserRegister")]
        //[OperationContract]
        //List<userdetailsEntity> UserRegister(string FirstName, string LastName, string EmailID, string RoleID, string ClusterCode,
        //    string MobileNo = "");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "SendOTPEmail")]
        [OperationContract]
        string SendOTPEmail(string EmailID);

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ValidateOTP")]
        [OperationContract]
        string ValidateOTP(string EmailID, string OTP, string Password);
        //[END] For Authentication

        //[STARt] For Book Management
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetBooks")]
        [OperationContract]
        List<BookDetailsEntity> GetBooks();

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetBooksData")]
        [OperationContract]
        List<BookDetails> GetBooksData();

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageBooks")]
        [OperationContract]
        List<BookDetailsEntity> ManageBooks(string BookName, string cmd, string EmailID, string Price, string Author, string Stock, string CategoryID,
        string LanguageID, string PublisherID, string BookDescription, string ThumbImg64, string Img1, string BookID = "");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetQrCodes")]
        [OperationContract]
        List<BookDetailsEntity> GetQrCodes(string EmailID);

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
        string LibrarianID, string Members, string AdminEmailID, string Image64 = "", string ClusterID = "");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetClusterHeads")]
        [OperationContract]
        List<ClusterHeadEntity> GetClusterHeads();        

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
        List<LibrarianDetailsEntity> ManageLibrarians(int cmd, string FirstName, string LastName, string EmailID,
        string Address, string MobileNo, string AltMobileNo, string ClusterID, string AdminEmailID, string Image64, string DOB, string LibrarianID = "");
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
        string AltMobileNo, string ClusterID, string DOB, string AdminEmailID, string Image64, string MemberID = "");
        //[END] For Member Management

        //[START] For Approvals Module
        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetRequests")]
        [OperationContract]
        List<RequestsDetailsEntity> GetRequests(string EmailID);

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageRequests")]
        [OperationContract]
        List<RequestsDetailsEntity> ManageRequests(int cmd, string BookID, string MemberID, string senderEmailID, string RequestID = "");  
        //[END] For Approvals Module

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetLatestShayari")]
        [OperationContract]
        List<GetLatestShayari> GetLatestShayari();

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "SubmitLatestShayari")]
        [OperationContract]
        List<GetLatestShayari> SubmitLatestShayari(string msg, string EmailID, string Category);


        #region AP Donor

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "APLogin")]
        [OperationContract]
        List<VolunteerEntity> APLogin(string EmailID, string Password, string FCM = "");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageVolunteer")]
        [OperationContract]
        List<VolunteerEntity> ManageVolunteer(string cmd, string FirstName, string LastName, string EmailID, string ContactNo, string DOB, string Address, string AdminEmailID, string Img = "", string LoginID = "");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetVolunteers")]
        [OperationContract]
        List<VolunteerEntity> GetVolunteers();

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "AddDonors")]
        [OperationContract]
        List<DonorEntity> AddDonors(string FirstName, string LastName, string EmailID, string ContactNo, string DOB, string Address, int Amount, string PaymentMode, string AdminEmailID,string Description="");

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetDonors")]
        [OperationContract]
        List<DonorEntity> GetDonors(string EmailID);

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetDonorBirthdays")]
        [OperationContract]
        List<DonorEntity> GetDonorBirthdays();

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "GetCelebrateRequest")]
        [OperationContract]
        List<CelebrateEntity> GetCelebrateRequest();

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "ManageCelebrateRequest")]
        [OperationContract]
        List<CelebrateEntity> ManageCelebrateRequest(int cmd, string FirstName, string LastName,
        string EmailID, string Contact, string Date, string VolEmailID, int AreaID, int OccassionID, string ID);

        //[WebInvoke(Method = "POST",
        //RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        //UriTemplate = "ManageCelebrateRequest")]
        //[OperationContract]
        //List<CelebrateEntity> ManageCelebrateRequest(int cmd, string FirstName, string LastName,
        //string EmailID, string Contact, string Date, string VolEmailID, int AreaID, int OccassionID, string ID);

        //[WebInvoke(Method = "POST",
        //RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        //UriTemplate = "SendReceiptMail")]
        //[OperationContract]
        //string SendReceiptMail(string EmailID, int DonorID);

        [WebInvoke(Method = "POST",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "DeclineEmailSMS")]
        [OperationContract]
        string DeclineEmailSMS(string EmailID);

        #endregion AP Donor

    }
      
}
