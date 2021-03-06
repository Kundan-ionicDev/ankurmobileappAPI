﻿using AnkurPrathisthan.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Gma.QrCodeNet.Encoding;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Drawing;
using System.Web;
using System.Security.Cryptography;
using System.Security.AccessControl;

//using System.iTextSharp.text;
//using iTextSharp.text.pdf;



namespace AnkurPrathisthan
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class APService : IAPService
    {
        //[START] For Email sending          
        public static string PORTNO = System.Configuration.ConfigurationManager.AppSettings["PORTNO"].ToString();
        public static string SMTPSERVER = System.Configuration.ConfigurationManager.AppSettings["SMTPSERVER"].ToString();
        public static string USERNAME = System.Configuration.ConfigurationManager.AppSettings["USERNAME"].ToString();
        public static string PASSWORD = System.Configuration.ConfigurationManager.AppSettings["PASSWORD"].ToString();
        //[END] For Email sending

        //[START]FOR NOTIFICATION SENDING
        public static string serverapikey = System.Configuration.ConfigurationManager.AppSettings["serverapikey"].ToString();
        //[END] FOR NOTIFICATION SENDING
        #region Logs
        


        //public static void //WriteToFile(string ExceptionMessage, string ExceptionStackTrace, string ExceptionLocation)
        //{
        //    StringBuilder errorMessage = new StringBuilder();
        //    errorMessage.Append("Exception - " + ExceptionMessage.ToString() + Environment.NewLine + Environment.NewLine);
        //    errorMessage.Append("Date & Time - " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + Environment.NewLine + Environment.NewLine);
        //    errorMessage.Append("Exception occured in " + ExceptionLocation + " in " +  Environment.NewLine);
        //    errorMessage.Append("Stake Trace - " + ExceptionStackTrace + Environment.NewLine + Environment.NewLine);
        //    errorMessage.Append(Environment.NewLine + Environment.NewLine);
            
        //    string applicationPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
        //    string logFilePath = HttpContext.Current.Server.MapPath("~Logs" + "/04Mar2020.txt");//(applicationPath +  @"Logs\" + DateTime.Today.ToString("ddMMMyyyy") + ".txt");
        //    // System.IO.FileStream file = System.IO.File.Create(HttpContext.Current.Server.MapPath("~Logs" + "/error.ext"));

             
        //    if (File.Exists(logFilePath))
        //    {
        //        FileStream fs = new FileStream(logFilePath, FileMode.Open);
        //        StreamWriter str = new StreamWriter(fs);
        //        str.BaseStream.Seek(0, SeekOrigin.End);
        //        str.Write(errorMessage);
        //        str.Flush();
        //        str.Close();
        //        fs.Close();
        //    }
        //    else
        //    {
        //        FileStream fs = new FileStream(logFilePath,FileMode.Append, FileAccess.Write, FileShare.Read);
        //        StreamWriter str = new StreamWriter(fs);
        //        str.BaseStream.Seek(0, SeekOrigin.End);
        //        str.Write(errorMessage);
        //        str.Flush();
        //        str.Close();
        //        fs.Close();
        //    }
        //}


        //public static void //WriteToFile(string ExceptionMethod,string Output, string Login="")
        //{
        //    StringBuilder errorMessage = new StringBuilder();
        //    errorMessage.Append("APService:: " + DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + ExceptionMethod.ToString() + Environment.NewLine);
        //    errorMessage.Append("APService::" + ExceptionMethod + " in " + Output + Environment.NewLine);
        //    errorMessage.Append("User:: " + Login + Environment.NewLine);    
        //    string applicationPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
        //    string logFilePath = (applicationPath + @"Logs\" + DateTime.Today.ToString("ddMMMyyyy") + ".txt");

        //    if (File.Exists(logFilePath))
        //    {
        //        FileStream fs = new FileStream(logFilePath, FileMode.Open);
        //        StreamWriter str = new StreamWriter(fs);
        //        str.BaseStream.Seek(0, SeekOrigin.End);
        //        str.Write(errorMessage);
        //        str.Flush();
        //        str.Close();
        //        fs.Close();
        //    }
        //    else
        //    {
        //        FileStream fs = new FileStream(logFilePath, FileMode.OpenOrCreate);
        //        StreamWriter str = new StreamWriter(fs);
        //        str.BaseStream.Seek(0, SeekOrigin.End);
        //        str.Write(errorMessage);
        //        str.Flush();
        //        str.Close();
        //        fs.Close();
        //    }
        //}   
            //string filename = ExceptionMethod + DateTime.Today.ToString("ddMMMyyyy") + ".txt";
            //string logFilePath = @"C:\Logs\" + filename;         

            //if (File.Exists(logFilePath))
            //{
            //    FileStream fs = new FileStream(logFilePath, FileMode.Open);
            //    StreamWriter str = new StreamWriter(fs);
            //    str.BaseStream.Seek(0, SeekOrigin.End);
            //    str.Write(errorMessage);
            //    str.Flush();
            //    str.Close();
            //    fs.Close();
            //}
            //else
            //{
            //    FileStream fs = new FileStream(logFilePath, FileMode.OpenOrCreate);
            //    StreamWriter str = new StreamWriter(fs);
            //    str.BaseStream.Seek(0, SeekOrigin.End);
            //    str.Write(errorMessage);
            //    str.Flush();
            //    str.Close();
            //    fs.Close();
            //}
       // }



        
        #endregion Logs

        //[START] FOR login & logout
        public userdetailsEntity UserLogin(string EmailID, string Password, string deviceinfo, string platform, string FCMID, string IMEI)
        {
            if (deviceinfo == null)
            {
                deviceinfo = "";
            }
            if (platform == null)
            {
                platform = "";
            }
            if (FCMID == null)
            {
                FCMID = "";
            }
            if (IMEI == null)
            {
                IMEI = "";
            }
            //   string result = "";
            DataSet ds = new DataSet();
            clsAuthentication objGeneral = new clsAuthentication();
            userdetailsEntity entity = new userdetailsEntity();
            try
            {
              //  //WriteToFile("UserLogin", "[START]", EmailID);
                if (EmailID == null || Password == null)
                { }
                else
                {
                    ds = objGeneral.GetUserDetails(EmailID, Password, FCMID);
                 //   //WriteToFile("UserLogin", "Login[START]", EmailID);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
                            if (entity.Message == "Success")
                            {
                                entity.FullName = Convert.ToString(ds.Tables[0].Rows[0]["FullName"]);
                                entity.EmailId = Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]);
                                entity.RoleID = Convert.ToInt32(ds.Tables[0].Rows[0]["RoleID"]);
                                entity.ClusterCode = Convert.ToInt32(ds.Tables[0].Rows[0]["ClusterCode"]);
                            }
                        }
                    }
                }
            //    //WriteToFile("UserLogin", "Login[END]", EmailID);
            }              

            catch (Exception ex)
            {
             //  //WriteToFile("UserLogin", ex.StackTrace, EmailID);                
            }
            finally
            {
                ds.Clear();
                ds.Dispose();
            }
            return entity;
        }
        public string UserLogout(string EmailID)
        {
            DataSet ds = new DataSet();
            clsAuthentication objGeneral = new clsAuthentication();
            string result = "";
            try
            {
                if (EmailID == null)
                {
                   
                }

                else
                {
                    ds = objGeneral.LogoutDetail(EmailID);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            result = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
             //  //WriteToFile("UserLogout" , ex.Message, EmailID);                
            }
            return result;
        }


        // This method will be used for Admin Registration or for Admin creation
        public List<userdetailsEntity> UserRegister(string FirstName, string LastName, string EmailID, string RoleID, string ClusterCode,
        string MobileNo = "")
        {
            DataSet ds = new DataSet();
            string Password = ""; //generated from email sending 
            clsAuthentication objAuth = new clsAuthentication();
            List<userdetailsEntity> entity = new List<userdetailsEntity>();
            try
            {
                //WriteToFile("UserRegister", "", EmailID);
                if (EmailID == null || FirstName == null)
                {                    
                }
                else
                {
                    Password = objAuth.SendEmail(EmailID); //Email sending for new password for new user
                  //  //WriteToFile("UserRegister",Password,EmailID);
                    ds = objAuth.RegisterUser(FirstName, LastName, EmailID, Password, ClusterCode, RoleID, MobileNo, "");
                    ////WriteToFile("UserRegister", "UserRegistered", EmailID);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.Add(new userdetailsEntity
                            {
                                Firstname = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                                Lastname = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                                EmailId = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                                Password = Convert.ToString(ds.Tables[0].Rows[i]["Password"]),
                                ClusterCode = Convert.ToInt32(ds.Tables[0].Rows[i]["ClusterCode"]),
                                Mobile = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                                RoleID = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleID"]),
                                Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"])

                            });

                        }
                    }

                }
              //  //WriteToFile("UserRegistration", "UserRegistered[END]", EmailID);
            }

            catch (Exception ex)
            {
              //  //WriteToFile("UserRegistration",ex.Message, EmailID);                
            }
            return entity;
        }
        //[END] For Admin Creation
        //[START] FOR FORGOT PASSWORD VIA OTP ON EMAIL
        public string SendOTPEmail(string EmailID)
        {
            DataSet ds = new DataSet();
            string newOTP = "";
            string Message = ""; string objIsEmailSent = "";
            DataSet dsInsert = new DataSet();
            clsAuthentication obj = new clsAuthentication();
            userdetailsEntity entity = new userdetailsEntity();
            clsBookManagement bm = new clsBookManagement();

            try
            {               
                newOTP = obj.CreateOTP();                
               // objIsEmailSent = obj.SendOTPEmail(EmailID, newOTP);
                dsInsert = obj.InsertOtp(newOTP, EmailID);
                if (dsInsert.Tables.Count > 0 && dsInsert.Tables[0].Rows.Count > 0)
                {
                    Message = dsInsert.Tables[0].Rows[0]["Message"].ToString();
                    if (Message == "SUCCESS")
                    {
                        objIsEmailSent = obj.SendOTPEmail(EmailID, newOTP);
                    }
                }                
            }
            catch (Exception ex)
            {
                bm.InsertError(EmailID, "SendOTPEmail","Message"+ ex.Message + "StackTrace"+ex.StackTrace,"CreateOTP");        
            }
            return objIsEmailSent;
        }
        //[END] FOR FORGOT PASSWORD VIA OTP ON EMAIL        

        public string ValidateOTP(string EmailID, string OTP, string Password)
        {
            string Message = "";
            DataSet ds = new DataSet();
            clsAuthentication obj = new clsAuthentication();
            DataSet dspwdchange = new DataSet();
            try
            {
               // //WriteToFile("ValidateOTP", OTP, EmailID);
                ds = obj.ValidateOTP(EmailID, OTP);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Message"].ToString() == "VALID")
                    {
                        dspwdchange = obj.PasswordReset(EmailID, Password);
                        if (dspwdchange.Tables.Count > 0)
                        {
                            Message = dspwdchange.Tables[0].Rows[0]["Message"].ToString();
                        }
                    }
                }
             //   //WriteToFile("ValidateOTP", "[END]", EmailID);
            }
            catch (Exception ex)
            {
              //  //WriteToFile("ValidateOTP", ex.StackTrace, ex.Message +"Email"+ EmailID);                
            }
            return Message;
        }
            
            
        //[START] For Book Management        
        public List<BookDetailsEntity> GetBooks()
        {
            clsBookManagement objbook = new clsBookManagement();
            List<BookDetailsEntity> entity = new List<BookDetailsEntity>();
            DataSet ds = new DataSet();
            try
            {
                //WriteToFile("GetBooks","[START]", "Admin");
                ds = objbook.ShowBooks();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add(new BookDetailsEntity
                        {                            
                            BookID = Convert.ToString(ds.Tables[0].Rows[i]["BookID"]),
                            BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]),
                            BookDescription = Convert.ToString(ds.Tables[0].Rows[i]["BookDescription"]),
                            AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["AuthorName"]),
                            Price = Convert.ToString(ds.Tables[0].Rows[i]["Price"]),
                            Stock = Convert.ToString(ds.Tables[0].Rows[i]["Stock"]),
                            CategoryName = Convert.ToString(ds.Tables[0].Rows[i]["CategoryName"]),
                            Language = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesName"]),
                            CategoryID = Convert.ToString(ds.Tables[0].Rows[i]["CategoryID"]),                            
                            LanguageID = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesID"]),                           
                            PublisherID = Convert.ToString(ds.Tables[0].Rows[i]["PublisherID"]),
                            PublisherName = Convert.ToString(ds.Tables[0].Rows[i]["PublisherName"]),
                            ThumbImage = Convert.ToString(ds.Tables[0].Rows[i]["ThumbImage"]) == "" ? "/Uploads/thumbnail.png" : Convert.ToString(ds.Tables[0].Rows[i]["ThumbImage"]),
                            Image2 = Convert.ToString(ds.Tables[0].Rows[i]["Image2"]) == "" ? "/Uploads/thumbnail.png" :Convert.ToString(ds.Tables[0].Rows[i]["Image2"])
                        });
                    }
                }
              //  //WriteToFile("GetBooks", "[END]", "Admin");
            }
            catch (Exception ex)
            {
                //WriteToFile("GetBooks-----", ex.ToString(), "ADmin");
            }
            return entity;

        }
        //to get publisher author cateory languages data
        public List<BookDetails> GetBooksData()
        {
            DataSet ds = new DataSet();
            List<BooksData> bd = new List<BooksData>();
            List<BookDetails> objBooks = new List<BookDetails>();
            clsBookManagement bm = new clsBookManagement();

            List<PublisherDetails> objPublishers = new List<PublisherDetails>();
            List<LanguageDetails> objLanguages = new List<LanguageDetails>();
            List<CategoryDetails> objCategories = new List<CategoryDetails>();
            List<Images> objImages = new List<Images>();
            try
            {
                //WriteToFile("GetBooksData", "START", "Admin");
                ds = bm.GetData();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objLanguages.Add(new LanguageDetails
                        {
                            LanguageID = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesID"]),
                            LanguageName = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesName"]),
                        });                        
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        objPublishers.Add(new PublisherDetails
                        {
                            PublisherID = Convert.ToString(ds.Tables[1].Rows[i]["PublisherID"]),
                            PublisherName = Convert.ToString(ds.Tables[1].Rows[i]["PublisherName"]),
                        });
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        objCategories.Add(new CategoryDetails
                        {
                            CategoryID = Convert.ToString(ds.Tables[2].Rows[i]["CategoryID"]),
                            CategoryName = Convert.ToString(ds.Tables[2].Rows[i]["CategoryName"]),
                        });
                    }
                }

                objBooks.Add(new BookDetails()
                {
                    Categories = objCategories,
                    Languages = objLanguages,
                    Publishers = objPublishers,                   

                });
                //WriteToFile("GetBooksData", "END", "Admin");
            }
            catch (Exception ex)
            {
                //WriteToFile("GetBooksData", ex.Message, "");
            }

            return objBooks;

        }

        public List<BookDetailsEntity> ManageBooks(string BookName, string cmd, string EmailID, string Price, string Author, string Stock, string CategoryID,
        string LanguageID, string PublisherID,string BookDescription,string ThumbImg64,string Img1, string BookID = "")
        {
            List<BookDetailsEntity> entity = new List<BookDetailsEntity>();
            DataSet ds = new DataSet();
            clsBookManagement bm = new clsBookManagement();
            clsQRCode qrc = new clsQRCode();
          // string filepath = @"F:\k_dev\AnkurPrathisthan\Uploads\Books\" ;
            string filepath = @"C:\ankurmobileappAPI-Development\Uploads\Books\";           
            string DBPath = "/Uploads/Books/";
            string ThumbImgPath = "", Image2Path="";
            Image ThumbImg, Image2;
            if (EmailID == null)
                EmailID = "";
            if (Price == null)
                Price = "";
            if (Author == null)
                Author = "";
            if (Stock == null) Stock = "";
            if (CategoryID == null)
                CategoryID = "";
            if (LanguageID == null)
                LanguageID = "";
            if (PublisherID == null)
                PublisherID = "";
            try
            {
                //WriteToFile("ManageBooks","START",EmailID);
                if (BookName == null || cmd == null)
                {
                   
                }
                    else
                    {
                        string DBImgPath1 = "";
                        string DBImgPath2 = "";
                        if (cmd.Trim() == "1" || cmd.Trim() == "2")
                        {
                            if (ThumbImg64 != "" && ThumbImg64 != null && BookName!=null  && BookName!= "")
                            {
                                try
                                {
                                    //[START] unique thumb image id 
                                    string ThumbID = GenerateImageID();
                                    //[END] unique thumb  image id
                                    string ThumbImgName1 = BookName + ThumbID; //ThumbImgNAme
                                    ThumbImg = Base64ToImage(ThumbImg64, filepath, ThumbImgName1);//ThumbImgNAme PNG
                                    ThumbImgPath = filepath + ThumbImgName1;////ThumbImgNAme Path;
                                    string Image2ID = GenerateImageID(); //unique image2 id
                                    string ImgName2 = BookName + Image2ID; //image2 name
                                    Image2 = Base64ToImage(Img1, filepath, ImgName2); //Image 2 PNG 
                                    Image2Path = filepath+ImgName2; //Image 2 Path

                                    DBImgPath1 = DBPath + ThumbImgName1;//DB path Img1
                                    DBImgPath2 = DBPath + ImgName2;//DB path Img2

                                    //WriteToFile("ManageBooks", DBImgPath2, EmailID);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                        ds = bm.HandleBooks(BookName, cmd, EmailID, Price, Author, Stock, CategoryID, LanguageID, PublisherID, BookID,
                            BookDescription, DBImgPath1, DBImgPath2, "");
                        //WriteToFile("ManageBooks", "Book added/deleted/updated", EmailID);
                        
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {    
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.Add(new BookDetailsEntity
                            {
                                BookID = Convert.ToString(ds.Tables[0].Rows[i]["BookID"]),
                                BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]),
                                AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["AuthorName"]),
                                Price = Convert.ToString(ds.Tables[0].Rows[i]["Price"]),
                                Stock = Convert.ToString(ds.Tables[0].Rows[i]["Stock"]),
                                CategoryID = Convert.ToString(ds.Tables[0].Rows[i]["CategoryID"]),
                                LanguageID = Convert.ToString(ds.Tables[0].Rows[i]["LanguageID"]),
                                EmailID = Convert.ToString(ds.Tables[0].Rows[i]["AddedBy"]),
                                PublisherID = Convert.ToString(ds.Tables[0].Rows[i]["PublisherID"]),
                                BookDescription = Convert.ToString(ds.Tables[0].Rows[i]["BookDescription"]),
                                ThumbImage = Convert.ToString(ds.Tables[0].Rows[i]["ThumbImage"]),
                                Image2= Convert.ToString(ds.Tables[0].Rows[i]["Image2"]),
                               // qrcode = Convert.ToString(ds.Tables[0].Rows[i]["qrcode"])
                            });
                        } 
                    }
                }
              //  //WriteToFile("ManageBooks", "END", EmailID);
            }
            catch (Exception ex)
            {
               // //WriteToFile("ManageBooks",ex.Message,EmailID);
            }
            return entity;
        }


        private static string GetQRCode (string BookID,string BookName)
        {
            string strQRCode = "";
            DataSet ds = new DataSet();
            clsQRCode qrc = new clsQRCode();
            strQRCode = qrc.GenerateQRCode(BookID, BookName);
            return strQRCode ;
        }

        public List<BookDetailsEntity> GetQrCodes (string EmailID)
        {
            List<BookDetailsEntity> entity = new List<BookDetailsEntity>();
            DataSet ds = new DataSet();
            clsQRCode objqrcode = new clsQRCode();
            ds = objqrcode.GetCode(EmailID);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    entity.Add(new BookDetailsEntity
                    {
                        qrcode = Convert.ToString(ds.Tables[0].Rows[i]["QrCode"]),
                        qrcodepath = Convert.ToString(ds.Tables[0].Rows[i]["QrCodePath"]),
                        mailsent = Convert.ToString(ds.Tables[0].Rows[i]["MailSent"]),
                        BookID = Convert.ToString(ds.Tables[0].Rows[i]["BookID"]),
                        BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]),
                        EmailID = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]),
                    });
                }
            }
            return entity;
        }

        public List<CategoryDetails> ManageCategories(string cmd, string CategoryName, string Email, string CategoryID = "")
        {
            List<CategoryDetails> entity = new List<CategoryDetails>();
            clsBookManagement bm = new clsBookManagement();
            DataSet ds = new DataSet();
            if (Email == null)
                Email = "";

            try
            {
              //  //WriteToFile("ManageCategories", "START", Email);
                if (cmd == null || CategoryName == null)
                {                   
                }
                else
                {
                    ds = bm.HandleCategories(cmd, CategoryName, Email, CategoryID);
                 //   //WriteToFile("ManageCategories","Add/Delete",Email);
                    //For adding
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.Add(new CategoryDetails
                            {
                                ModifiedBy = Convert.ToString(ds.Tables[0].Rows[i]["ModifiedBy"]),
                                CategoryName = Convert.ToString(ds.Tables[0].Rows[i]["CategoryName"]),
                                CreatedBy = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]),
                                CategoryID = Convert.ToString(ds.Tables[0].Rows[i]["CategoryID"]),
                                Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]),
                            });

                        }
                    }
                }
               // //WriteToFile("ManageCategories", "END", Email);
            }
            catch (Exception ex)
            {
              //  //WriteToFile("ManageCategories", ex.Message, Email);
            }

            return entity;
        }//WriteToFile
        public List<LanguageDetails> ManageLanguages(string cmd, string LanguageName, string Email, string LanguageID = "")
        {
            List<LanguageDetails> entity = new List<LanguageDetails>();
            clsBookManagement bm = new clsBookManagement();
            DataSet ds = new DataSet();
            if (Email == null)
                Email = "";

            try
            {
               // //WriteToFile("ManageLanguages","START",Email);
                if (cmd == null || LanguageName == null)
                {  }
                else
                {
                    ds = bm.HandleLanguages(cmd, LanguageName, Email, LanguageID);
                    //WriteToFile("ManageLanguages", "Add/delete", Email);
                    //For adding
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.Add(new LanguageDetails
                            {

                                LanguageID = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesID"]),
                                LanguageName = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesName"]),
                                CreatedBy = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]),
                                ModifiedBy = Convert.ToString(ds.Tables[0].Rows[i]["ModifiedBy"]),
                                Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]),
                            });

                        }
                    }
                }
                //WriteToFile("ManageLanguages", "END", Email);
            }
            catch (Exception ex)
            {
                //WriteToFile("ManageLanguages",ex.Message,Email);
            }

            return entity;
        }

        public List<PublisherDetails> ManagePublishers(string cmd, string PublisherName, string Email, string PublisherID = "")
        {
            List<PublisherDetails> entity = new List<PublisherDetails>();
            clsBookManagement bm = new clsBookManagement();
            DataSet ds = new DataSet();
            if (Email == null)
                Email = "";
            try
            {
                //WriteToFile("ManagePublishers","START",Email);
                if (cmd == null || PublisherName == null)
                {                   
                }
                else
                {
                    ds = bm.HandlePublishers(cmd, PublisherName, Email, PublisherID);
                    //WriteToFile("ManagePublishers", "Add/delete", Email);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.Add(new PublisherDetails
                            {
                                PublisherID = Convert.ToString(ds.Tables[0].Rows[i]["PublisherID"]),
                                ModifiedBy = Convert.ToString(ds.Tables[0].Rows[i]["ModifiedBy"]),
                                PublisherName = Convert.ToString(ds.Tables[0].Rows[i]["PublisherName"]),
                                CreatedBy = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]),
                                Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]),
                            });
                        }
                    }
                }
                //WriteToFile("ManagePublishers", "END", Email);
            }
            catch (Exception ex)
            {
                //WriteToFile("ManagePublishers", ex.Message, Email);
            }
            return entity;
        }
        //[END] For Book Management


        //[START] For Cluster Management       
        public List<ClusterDetailsEntity> GetClusters()
        {
            clsClusterManagement cm = new clsClusterManagement();
            DataSet ds = new DataSet();
            List<ClusterDetailsEntity> entity = new List<ClusterDetailsEntity>();
            try
            {
                //WriteToFile("GetClusters", "START", "Admin");
                ds = cm.ShowClusters();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add(new ClusterDetailsEntity
                        {
                            ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                            ClusterCode = Convert.ToString(ds.Tables[0].Rows[i]["ClusterCode"]),
                            ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]),
                            Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                            MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                            Members = Convert.ToString(ds.Tables[0].Rows[i]["Members"]),
                            LibrarianID = Convert.ToString(ds.Tables[0].Rows[i]["Librarian"]),
                            Image = Convert.ToString(ds.Tables[0].Rows[i]["Img"]) == "" ? "/Uploads/thumbnail.png" : Convert.ToString(ds.Tables[0].Rows[i]["Img"]),
                        });
                    }
                }
                //WriteToFile("GetClusters","START","Admin");
            }
            catch (Exception ex)
            {
                //WriteToFile("GetClusters", ex.StackTrace, "Admin");
            }
            return entity;
        }

        public List<ClusterDetailsEntity> ManageClusters(string ClusterName, string ClusterCode, string cmd, string EmailID,
            string Address, string MobileNo,
        string LibrarianID, string Members, string AdminEmailID, string Image64 = "", string ClusterID = "")
        {
            List<ClusterDetailsEntity> entity = new List<ClusterDetailsEntity>();
            DataSet ds = new DataSet();
            clsClusterManagement bm = new clsClusterManagement();
            Image Image; string ImagePath = ""; string DBImgPath = "";
           // string filepath = @"F:\k_dev\AnkurPrathisthan\Uploads\Clusters\";
            string DBPath = "/Uploads/Clusters/";
           string filepath = @"C:\ankurmobileappAPI-Development\Uploads\Clusters\";           
            if (EmailID == null)
                EmailID = "";
            if (ClusterName == null)
                ClusterName = "";
            if (ClusterCode == null)
                ClusterCode = "";
            if (Address == null) Address = "";
            if (MobileNo == null)
                MobileNo = "";
            if (LibrarianID == null)
                LibrarianID = "";
            if (Members == null)
                Members = "";
            if (AdminEmailID == null)
                AdminEmailID = "";

            try
            {
                //WriteToFile("ManageClusters", "START", EmailID);          
                    if (cmd.Trim() == "1" || cmd.Trim() =="2")
                    {
                        if (Image64 != "" && Image64 != null)
                        {
                            try
                            {
                                //[START] unique thumb image id 
                                string ImageID = GenerateImageID();
                                //[END] unique thumb  image id
                                string ImageName = ClusterName + ImageID; //ThumbImgNAme
                                Image = Base64ToImage(Image64, filepath, ImageName);//ThumbImgNAme PNG
                                ImagePath = filepath + ImageName;////ThumbImgNAme Path;
                                DBImgPath = DBPath + ImageName;
                                //WriteToFile("ManageClusters", DBImgPath, EmailID);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    ds = bm.HandleClusters(ClusterName, ClusterCode, cmd, EmailID, Address, MobileNo, LibrarianID, Members, AdminEmailID, ClusterID, DBImgPath);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    { 
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {                            
                            entity.Add(new ClusterDetailsEntity
                            {
                                ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                                ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]),
                                ClusterCode = Convert.ToString(ds.Tables[0].Rows[i]["ClusterCode"]),
                                Email = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                                Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                                MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                                Members = Convert.ToString(ds.Tables[0].Rows[i]["Members"]),
                                LibrarianID = Convert.ToString(ds.Tables[0].Rows[i]["Librarian"]),
                                Image = Convert.ToString(ds.Tables[0].Rows[i]["Img"]),
                            });
                        }                        
                    }
                    //WriteToFile("ManageClusters", "END", EmailID);
            }
            catch (Exception ex)
            {
                //WriteToFile("ManageClusters", ex.Message, EmailID);
            }
            return entity;
        }

        ////[START]For ClusterHead Role
        public List<ClusterHeadEntity> GetClusterHeads()
        {
            List<ClusterHeadEntity> entity = new List<ClusterHeadEntity>();
            DataSet ds = new DataSet();
            clsClusterManagement lm = new clsClusterManagement();
            try
            {
                ds = lm.ShowClusterHeads();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add(new ClusterHeadEntity
                        {
                            ClusterHeadID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterHeadID"]),
                            ClusterRegionID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterRegionId"]),
                            FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                            LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                            Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                            MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                            AltMobileNo = Convert.ToString(ds.Tables[0].Rows[i]["AltMobileNo"]),
                            EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                            Image = Convert.ToString(ds.Tables[0].Rows[i]["Img"]),
                            DOB = Convert.ToString(ds.Tables[0].Rows[i]["DOB"]),
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in API ---GetClusterHeadsAPI" + ex.Message);
                throw ex;
            }
            return entity;
        }        

        //[END] For CLuster Management

        //[START] For Librarian Management        


        public List<LibrarianDetailsEntity> GetLibrarians()
        {
            List<LibrarianDetailsEntity> entity = new List<LibrarianDetailsEntity>();
            DataSet ds = new DataSet();
            clsLibrarianManagement lm = new clsLibrarianManagement();
            try
            {
                //WriteToFile("GetLibrarians","START","ADMIN");
                ds = lm.ShowLibrarians();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add(new LibrarianDetailsEntity
                        {
                            LibrarianID = Convert.ToString(ds.Tables[0].Rows[i]["LibrarianID"]),
                            FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                            LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                            Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                            MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                            EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                            ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                            Image = Convert.ToString(ds.Tables[0].Rows[i]["Img"]) == "" ? "/Uploads/thumbnail.png" : Convert.ToString(ds.Tables[0].Rows[i]["Img"]),
                            DOB = Convert.ToString(ds.Tables[0].Rows[i]["DOB"]),
                        });

                    }
                }
                //WriteToFile("GetLibrarians", "END", "ADMIN");
            }
            catch (Exception ex)
            {
                //WriteToFile("GetLibrarians", ex.Message, "ADMIN");
            }
            return entity;
        }
        public List<LibrarianDetailsEntity> ManageLibrarians(int cmd, string FirstName, string LastName, string EmailID,
        string Address, string MobileNo,string AltMobileNo, string ClusterID, string AdminEmailID,string Image64,string DOB, string LibrarianID = "")
        {
            List<LibrarianDetailsEntity> entity = new List<LibrarianDetailsEntity>();
            DataSet ds = new DataSet();
            clsLibrarianManagement lm = new clsLibrarianManagement();
            clsAuthentication objauth = new clsAuthentication();
            // string filepath = @"F:\k_dev\AnkurPrathisthan\Uploads\Librarian\";
            string filepath = @"C:\ankurmobileappAPI-Development\Uploads\Librarian\";
            string DBPath = "/Uploads/Librarian/";
            Image Image; string ImagePath = "";
            string Password = null;
            if (FirstName == null)
                FirstName = "";
            if (LastName == null)
                LastName = "";
            if (EmailID == null)
                EmailID = "";
            if (Address == null) Address = "";
            if (MobileNo == null)
                MobileNo = "";
            if (AltMobileNo == null)
                AltMobileNo = "";
            if (ClusterID == null)
                ClusterID = "";
            if (AdminEmailID == null)
                AdminEmailID = "";

            try
            {
                string DBImgPath = "";
                //WriteToFile("ManageLibrarians", "START", EmailID);
                if (cmd == 1 || cmd == 2)
                {
                    if (Image64 != "" && Image64 != null)
                    {
                        try
                        {
                            //[START] unique thumb image id 
                            string ImageID = GenerateImageID();
                            //[END] unique thumb  image id
                            string ImageName = EmailID + ImageID; //ThumbImgNAme
                            Image = Base64ToImage(Image64, filepath, ImageName);//ThumbImgNAme PNG
                            ImagePath = filepath + ImageName;////ThumbImgNAme Path; 
                            DBImgPath = DBPath + ImageName;//DB pth
                            //WriteToFile("ManageLibrarians", DBImgPath, EmailID);
                        }
                        catch (Exception ex)
                        {
                            //WriteToFile("ManageLibrarians", ex.Message, EmailID);
                        }
                    }
                }

                ds = lm.HandleLibrarians(cmd, FirstName, LastName, EmailID, Address, MobileNo, AltMobileNo, ClusterID, AdminEmailID, LibrarianID, DBImgPath, DOB);
                //WriteToFile("ManageLibrarians", "Add/update/delete", EmailID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add(new LibrarianDetailsEntity
                        {
                            LibrarianID = Convert.ToString(ds.Tables[0].Rows[i]["LibrarianID"]),
                            FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                            LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                            EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                            Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                            MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                            AltMobileNo = Convert.ToString(ds.Tables[0].Rows[i]["AltMobileNo"]),
                            ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                            CreatedBy = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]),
                            ModifiedBy = Convert.ToString(ds.Tables[0].Rows[i]["ModifiedBy"]),
                            Image= Convert.ToString(ds.Tables[0].Rows[i]["Img"]),
                            DOB= Convert.ToString(ds.Tables[0].Rows[i]["DOB"]),
                        });
                    }                    
                }
            }
            catch (Exception ex)
            {
                //WriteToFile("ManageLibrarians", ex.Message, EmailID);
            }
            return entity;
        }
        //[END] For Librarian Management
        // [START] For Member Management
        public List<MemberDetailsEntity> GetMembers()
        {
            DataSet ds = new DataSet();
            clsMemberManagement mem = new clsMemberManagement();
            List<MemberDetailsEntity> entity = new List<MemberDetailsEntity>();
            try
            {
                //WriteToFile("GetMembers", "START", "Admin");
                ds = mem.ShowMembers();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add(new MemberDetailsEntity()
                       {
                           MemberID = Convert.ToString(ds.Tables[0].Rows[i]["MemberID"]),
                           FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                           LastName= Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                           Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                           MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                           EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                           ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                           ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]),
                           DOB = Convert.ToString(ds.Tables[0].Rows[i]["DOB"]),
                           Image = Convert.ToString(ds.Tables[0].Rows[i]["Img"]) == "" ? "/Uploads/thumbnail.png" : Convert.ToString(ds.Tables[0].Rows[i]["Img"]),

                       });
                    }
                }
                //WriteToFile("GetMembers", "END", "Admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in API ---GetMembers" + ex.Message);
                throw ex;
            }
            return entity;
        }

        public List<MemberDetailsEntity> ManageMembers(int cmd, string FirstName, string LastName, string EmailID, string Address, string MobileNo,
        string AltMobileNo, string ClusterID, string DOB, string AdminEmailID,string Image64, string MemberID = "")
        {
            List<MemberDetailsEntity> entity = new List<MemberDetailsEntity>();
            DataSet ds = new DataSet();
            clsMemberManagement mem = new clsMemberManagement();
            clsAuthentication objauth = new clsAuthentication();
            string Password = null;
            Image Image;
            string filepath = @"C:\ankurmobileappAPI-Development\Uploads\Members";
            //string filepath = @"F:\k_dev\AnkurPrathisthan\Uploads\Members\";
            string DBPath = "/Uploads/Members/";
            string ImagePath = "";
            if (FirstName == null)
                FirstName = "";
            if (LastName == null)
                LastName = "";
            if (EmailID == null)
                EmailID = "";
            if (Address == null) Address = "";
            if (MobileNo == null)
                MobileNo = "";
            if (AltMobileNo == null)
                AltMobileNo = "";
            if (ClusterID == null)
                ClusterID = "";
            try
            {
                string DBImgPath = "";
                //WriteToFile("ManageMembers", "START", EmailID);
                if (cmd == 1 || cmd == 2)
                {
                    if (Image64 != "" && Image64 != null)
                    {
                        try
                        {
                            //[START] unique thumb image id 
                            string ImageID = GenerateImageID();
                            //[END] unique thumb  image id
                            string ImageName = ImageID; //ThumbImgNAme
                            Image = Base64ToImage(Image64, filepath,ImageName);//ThumbImgNAme PNG
                            ImagePath = filepath + ImageName;////ThumbImgNAme Path;
                            DBImgPath = DBPath + ImageName;
                            //WriteToFile("ManageMembers", DBImgPath, EmailID);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                ds = mem.HandleMembers(cmd, FirstName, LastName, EmailID, Address, MobileNo, AltMobileNo, ClusterID, DOB, AdminEmailID, MemberID, DBImgPath);
                //WriteToFile("ManageMembers", "Add/update/delete", EmailID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add(new MemberDetailsEntity
                        {
                            MemberID = Convert.ToString(ds.Tables[0].Rows[i]["MemberID"]),
                            FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                            LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                            EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                            Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                            MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                            AltMobileNo = Convert.ToString(ds.Tables[0].Rows[i]["AltMobileNo"]),
                            ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                            DOB = Convert.ToString(ds.Tables[0].Rows[i]["DOB"]),
                            Image = Convert.ToString(ds.Tables[0].Rows[i]["Img"]),

                        });
                    }                   
                }
            }
            catch (Exception ex)
            {
                //WriteToFile("ManageMembers", ex.Message, EmailID);
            }
            return entity;
        }

        //[END] For Member Management

        //[START] For Approvals
        public List<RequestsDetailsEntity> GetRequests(string EmailID)
        {
            DataSet ds = new DataSet();
            List<RequestsDetailsEntity> requests = new List<RequestsDetailsEntity>();
            clsApprovals app = new clsApprovals();
            try
            {
                //WriteToFile("GetRequests", "START", "admin");
                ds = app.ShowRequests(EmailID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        requests.Add(new RequestsDetailsEntity
                        {
                            RequestID = Convert.ToString(ds.Tables[0].Rows[i]["RequestID"]),
                            RequestedDate = Convert.ToString(ds.Tables[0].Rows[i]["RequestedDate"]),
                            RequestAcceptDate = Convert.ToString(ds.Tables[0].Rows[i]["RequestAcceptDate"]),
                            RequestRetDate = Convert.ToString(ds.Tables[0].Rows[i]["RequestRetDate"]),
                            Status = Convert.ToString(ds.Tables[0].Rows[i]["Status"]),
                            RequestStatus = Convert.ToString(ds.Tables[0].Rows[i]["RequestStatus"]),                            
                            BookID = Convert.ToString(ds.Tables[0].Rows[i]["BookID"]),
                            BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]),
                            Stock = Convert.ToString(ds.Tables[0].Rows[i]["Stock"]),
                            BooksUnAvailable = Convert.ToString(ds.Tables[0].Rows[i]["Sold"]),
                            BooksAvailable = Convert.ToString(ds.Tables[0].Rows[i]["BooksAvailable"]),
                            ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                            ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]),
                            ClusterContactNo = Convert.ToString(ds.Tables[0].Rows[i]["ClusterContactNo"]),
                            MemberID = Convert.ToString(ds.Tables[0].Rows[i]["MemberID"]),
                            MemberName = Convert.ToString(ds.Tables[0].Rows[i]["MemberName"]),
                            LibrarianID = Convert.ToString(ds.Tables[0].Rows[i]["LibrarianID"]),
                            LibrarianName = Convert.ToString(ds.Tables[0].Rows[i]["LibrarianName"]),                           
                        });
                    }
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return requests;
        }

        public List<RequestsDetailsEntity> ManageRequests(int cmd, string BookID, string MemberID, string senderEmailID, string RequestID = "")
        {
            List<RequestsDetailsEntity> requests = new List<RequestsDetailsEntity>();
            List<AnkurPrathisthan.clsApprovals.FCMID> fcm = new List<AnkurPrathisthan.clsApprovals.FCMID>();
            DataSet ds = new DataSet();
            clsApprovals approvals = new clsApprovals();
            string Message =null;string messagebody=null;
            try
            {

                ds = approvals.HandleRequests(cmd, BookID, senderEmailID, MemberID, RequestID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                        requests.Add
                       (new RequestsDetailsEntity()
                        {
                            RequestID = Convert.ToString(ds.Tables[0].Rows[0]["RequestID"]),
                            Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]),                          
                        });                  
                    //string FCM = Convert.ToString(ds.Tables[0].Rows[0]["FCM"]);                                     
                }
                //[START]to send book notification
                DataSet dsUser = new DataSet(); string ClusterFCM = "", LibFCM="", MemberFCM="";
                dsUser = approvals.GetUser(MemberID);
                if (dsUser.Tables.Count>0)
                {
                    fcm.Add
                    (new AnkurPrathisthan.clsApprovals.FCMID ()
                    {
                            ClusterFCM = Convert.ToString(dsUser.Tables[0].Rows[0]["ClusterFCM"]),
                            LibFCM = Convert.ToString(dsUser.Tables[0].Rows[0]["LibrarianFCM"]),
                            MemberFCM = Convert.ToString(dsUser.Tables[0].Rows[0]["MemberFCM"]),
                    });
                    
                   
                }                
               // if (cmd==1)
               // {   
                if (cmd == 1)
                {
                    messagebody = "Book Request Sent";
                }
                else if (cmd == 3)
                {
                    messagebody = "Book Returned";
                }
                else if (cmd == 4)
                {
                    messagebody = "Book Request Approved";
                }  
                    if (ds.Tables[0].Rows[0]["Message"].ToString() == "SUCCESS" )
                    {
                        Message = approvals.SendNotification(ClusterFCM, LibFCM,MemberFCM, messagebody);
                    }
             //   }
                //[END]to send book notification
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return requests;
        }

        private static Image Base64ToImage(string base64string, string filepath,string imgname)
        {
            Image image= null ;
            string result = "";
            try
            {                               
                result = filepath + imgname;               
                var bytess = Convert.FromBase64String(base64string);
                using (var imageFile = new FileStream(result, FileMode.Create))
                {
                    imageFile.Write(bytess, 0, bytess.Length);
                    imageFile.Flush();
                }
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return image;
        }

        private static string  GenerateImageID ()
        {  
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 100000000;
            string result =String.Format("{0:D3}", random+".jpeg");;
            return result;
        }
        public string SendOTPEmailTest(string EmailID)
        {
            clsBookManagement bm = new clsBookManagement();
            string IsEmailSent = "";
            string ServerName = SMTPSERVER;
            int PORTNO = 25;//gmail port string Sender = USERNAME; string credential = PASSWORD;
            // OTP = CreateOTP(EmailID);
            SmtpClient smtpClient = new SmtpClient(ServerName, PORTNO);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(USERNAME, PASSWORD);
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(USERNAME);
                message.Subject = "Ankur Pratishthan Password Reset";
                message.Body = "Dear AnkurPratishthan User,Your One Time Password for Login::   ";
                message.IsBodyHtml = true;
                message.To.Add(EmailID);
                try
                {
                    smtpClient.Send(message);
                    message.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnSuccess;
                    IsEmailSent = message.DeliveryNotificationOptions.ToString();
                }
                catch (Exception ex)
                {
                    bm.InsertError(EmailID, "SendOTPEmail", "Message" + ex.Message + "StackTrace" + ex.StackTrace, "CreateOTP");
                }
            }
            return "Y";
        }



        //[START] For Facebook

        public List<GetLatestShayari> GetLatestShayari()
        {
            List<GetLatestShayari> entity = new List<GetLatestShayari>();
            DataSet ds = new DataSet();
            clsMemberManagement mem = new clsMemberManagement();
            try
            {
                ds = mem.GetFbMsg();
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add
                            (new GetLatestShayari()
                            {
                                Msg = Convert.ToString(ds.Tables[0].Rows[i]["Msg"]),
                                Category = Convert.ToString(ds.Tables[0].Rows[i]["Category"]),
                            });
                    }                
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        public List<GetLatestShayari> SubmitLatestShayari(string msg, string EmailID, string Category)
        {
            List<GetLatestShayari> entity = new List<GetLatestShayari>();
            DataSet ds = new DataSet();
            clsMemberManagement mem = new clsMemberManagement();
            try
            {
                ds = mem.InsertShayari(msg,EmailID,Category);
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add
                            (new GetLatestShayari()
                            {
                                Msg = Convert.ToString(ds.Tables[0].Rows[i]["Msg"]),
                                Category = Convert.ToString(ds.Tables[0].Rows[i]["Category"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        //[END] for facebook

        #region AP Donation Management System
        public List<VolunteerEntity> APLogin (string EmailID, string Password ,string FCM="")
        {
            List<VolunteerEntity> entity = new List<VolunteerEntity>();
            DataSet ds = new DataSet();
            APDonor objdonor = new APDonor();
            try
            {               
                ds = objdonor.Login(EmailID, Password, FCM);
                if (ds.Tables.Count > 0)
                {
                    entity.Add
                        (new VolunteerEntity()
                        {                           
                            EmailID = Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]),                            
                            Message= Convert.ToString(ds.Tables[0].Rows[0]["Message"]),
                            RoleID = Convert.ToString(ds.Tables[0].Rows[0]["RoleID"]),
                        });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        public List<VolunteerEntity> ManageVolunteer (string cmd, string FirstName,string LastName, string EmailID, string ContactNo, string DOB, string Address, string AdminEmailID,string Img="",string LoginID="")
        {
            List<VolunteerEntity> entity = new List<VolunteerEntity>();
            DataSet ds = new DataSet();
            clsAuthentication auth = new clsAuthentication();
            DataSet dsEmail = new DataSet();
            APDonor objdonor = new APDonor();
            try
            {
                ds = objdonor.RegisterVolunteer(cmd,FirstName, LastName, EmailID, DOB, Address, ContactNo, AdminEmailID, Img, "",LoginID);
                if (ds.Tables.Count>0)
                {
                    entity.Add
                        (new VolunteerEntity()
                        {
                            LoginID = Convert.ToString(ds.Tables[0].Rows[0]["LoginID"]),
                            FirstName = Convert.ToString(ds.Tables[0].Rows[0]["FirstName"]),
                            LastName = Convert.ToString(ds.Tables[0].Rows[0]["LastName"]),
                            EmailID = Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]),
                            DOB = Convert.ToString(ds.Tables[0].Rows[0]["DOB"]),
                            Address = Convert.ToString(ds.Tables[0].Rows[0]["Address"]),
                            ContactNo = Convert.ToString(ds.Tables[0].Rows[0]["ContactNo"]),
                            AdminEmail = Convert.ToString(ds.Tables[0].Rows[0]["CreatedBy"]),
                            Img = Convert.ToString(ds.Tables[0].Rows[0]["ImgPath"]),
                        }); 
                }

                if (cmd == "1")
                {
                    string Password = "";
                    Password = auth.SendEmail(EmailID);
                    dsEmail = objdonor.VolEmail(EmailID, Password);
                }              
           
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return entity;
        }

        public List<VolunteerEntity> GetVolunteers ()
        {
            List<VolunteerEntity> entity = new List<VolunteerEntity>();
            DataSet ds = new DataSet();
            clsAuthentication auth = new clsAuthentication();
            DataSet dsEmail = new DataSet();
            APDonor objdonor = new APDonor();
            try
            {
                ds = objdonor.FetchVolunteers();
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                             entity.Add
                            (new VolunteerEntity()
                            {
                                LoginID = Convert.ToString(ds.Tables[0].Rows[i]["LoginID"]),
                                FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                                LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                                EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                                DOB = Convert.ToString(ds.Tables[0].Rows[i]["DOB"]),
                                Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                                ContactNo = Convert.ToString(ds.Tables[0].Rows[i]["ContactNo"]),
                                AdminEmail = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]),
                                Img = Convert.ToString(ds.Tables[0].Rows[i]["ImgPath"]),
                            });
                    }                       
                }                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        public List<DonorEntity> AddDonors (string FirstName,string LastName, string EmailID, string ContactNo, string DOB, string Address,int Amount, string PaymentMode, string AdminEmailID,string Description="")
        {
            List<DonorEntity> entity = new List<DonorEntity>();
            DataSet ds = new DataSet();
            APDonor objdonor = new APDonor();
            try
            {
                ds = objdonor.SubmitDonors(FirstName,LastName, EmailID, DOB, Address, ContactNo, AdminEmailID,Amount,PaymentMode,Description);
                if (ds.Tables.Count > 0)
                {
                    entity.Add
                        (new DonorEntity()
                        {                            
                            FirstName = Convert.ToString(ds.Tables[0].Rows[0]["FirstName"]),
                            LastName = Convert.ToString(ds.Tables[0].Rows[0]["LastName"]),
                            EmailID = Convert.ToString(ds.Tables[0].Rows[0]["DonorEmailID"]),                          
                            Address = Convert.ToString(ds.Tables[0].Rows[0]["Address"]),
                            ContactNo = Convert.ToString(ds.Tables[0].Rows[0]["ContactNo"]),
                            AdminEmailID = Convert.ToString(ds.Tables[0].Rows[0]["CreatedBy"]),
                            Amount = Convert.ToInt32(ds.Tables[0].Rows[0]["Amount"]),
                            PaymentMode = Convert.ToString(ds.Tables[0].Rows[0]["PaymentMode"]),
                            Description = Convert.ToString(ds.Tables[0].Rows[0]["Description"]),
                           //DOB = Convert.ToString(ds.Tables[0].Rows[0]["DOB"]),
                        });
                }           

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        public List<DonorEntity> GetDonors (string EmailID)
        {
            List<DonorEntity> entity = new List<DonorEntity>();
            DataSet ds = new DataSet();            
            DataSet dsEmail = new DataSet();
            APDonor objdonor = new APDonor();
            try
            {
                ds = objdonor.FetchDonors(EmailID);
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add
                       (new DonorEntity()
                       {                           
                           FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                           LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                           EmailID = Convert.ToString(ds.Tables[0].Rows[i]["DonorEmailID"]),
                           Amount = Convert.ToInt32(ds.Tables[0].Rows[i]["Amount"]),
                           Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                           ContactNo = Convert.ToString(ds.Tables[0].Rows[i]["ContactNo"]),
                           AdminEmailID = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]),
                           RegDate = Convert.ToString(ds.Tables[0].Rows[i]["DateOfDonation"]),
                           PaymentMode = Convert.ToString(ds.Tables[0].Rows[i]["PaymentMode"]),
                           DOB = Convert.ToString(ds.Tables[0].Rows[i]["DOB"]),
                       });
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        public List<DonorEntity> GetDonorBirthdays()
        {
            List<DonorEntity> entity = new List<DonorEntity>();
            DataSet ds = new DataSet();
            DataSet dsEmail = new DataSet();
            APDonor objdonor = new APDonor();
            try
            {
                ds = objdonor.DonorBirthdays();
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add
                       (new DonorEntity()
                       {
                           FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                           LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                           EmailID = Convert.ToString(ds.Tables[0].Rows[i]["DonorEmailID"]),                          
                           ContactNo = Convert.ToString(ds.Tables[0].Rows[i]["ContactNo"]),
                           DOB = Convert.ToString(ds.Tables[0].Rows[i]["DOB"]),
                       });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        //[START] View Celebrate WIth US

        public List<CelebrateEntity> ManageCelebrateRequest(int cmd,string FirstName, string LastName,
            string EmailID,string Contact,string Date,string VolEmailID,int AreaID,int OccassionID,string ID)
        {
            List<CelebrateEntity> entity = new List<CelebrateEntity>();
            DataSet ds = new DataSet();
            APDonor obj = new APDonor();
            string mailsent = "";
            try
            {
                ds = obj.SubmitCelebrateReqeusts(cmd, FirstName, LastName, EmailID, Contact,Date, VolEmailID,AreaID,OccassionID,ID);
                if (ds.Tables.Count > 0)
                {
                    entity.Add
                        (new CelebrateEntity()
                        {
                            Message = Convert.ToString(ds.Tables[0].Rows[0]["Message"]),
                            ID = Convert.ToString(ds.Tables[0].Rows[0]["ID"]),
                            //FirstName = Convert.ToString(ds.Tables[0].Rows[0]["FirstName"]),
                            //LastName = Convert.ToString(ds.Tables[0].Rows[0]["LastName"]),
                            //Email = Convert.ToString(ds.Tables[0].Rows[0]["EmailID"]),
                            //Contact = Convert.ToString(ds.Tables[0].Rows[0]["DOB"]),
                            //DateOfEvent = Convert.ToString(ds.Tables[0].Rows[0]["Address"]),
                            //AreaID = Convert.ToInt32(ds.Tables[0].Rows[0]["ContactNo"]),
                            //OccassionID = Convert.ToInt32(ds.Tables[0].Rows[0]["CreatedBy"]),                            
                        });

                    mailsent = obj.SendCelebrateEmail(EmailID);
                }
                
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return entity;
        }

        public List<CelebrateEntity> GetCelebrateRequest()
        {
            List<CelebrateEntity> entity = new List<CelebrateEntity>();
            DataSet ds = new DataSet();
            APDonor obj = new APDonor();
            try
            {
                ds = obj.GetAllReqeusts();
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add
                            (new CelebrateEntity()
                            {
                                ID = Convert.ToString(ds.Tables[0].Rows[i]["ID"]),
                                FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]),
                                LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]),
                                Email = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                                Contact = Convert.ToString(ds.Tables[0].Rows[i]["ContactNo"]),
                                DateOfEvent = Convert.ToString(ds.Tables[0].Rows[i]["DateOfEvent"]),
                                AreaID = Convert.ToInt32(ds.Tables[0].Rows[i]["AreaID"]),
                                OccassionID = Convert.ToInt32(ds.Tables[0].Rows[i]["OccassionID"]),
                                Status = Convert.ToString(ds.Tables[0].Rows[i]["Status"]),
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return entity;
        }

        //[END]View Celebrate WIth US

        //public string SendReceiptMail(string EmailID, int DonorID)
        //{
        //    string message = null;
        //    try
        //    {
        //        clsPDF pdf = new clsPDF();
        //        string filename = "APReceipt" + DonorID + EmailID;
        //        APDonor ap = new APDonor();
        //        DataSet ds = null;
        //        ds = ap.ReceiptDonor(EmailID, DonorID);
        //        MailMessage mm = new MailMessage("kundan.mobileappdev@gmail.com", "kundan.mobileappdev@gmail.com");
        //        mm.Subject = "Ankur Pratishthan Donation Receipt";
        //        mm.Body = "Thank you! PFA receipt";                
        //        // mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "DonationReceipt.pdf"));
        //        mm.Attachments.Add(new Attachment(@"C:\DonationReceipt.pdf"));
        //        mm.IsBodyHtml = true;
        //        SmtpClient smtp = snew SmtpClient();
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.EnableSsl = true;
        //        NetworkCredential NetworkCred = new NetworkCredential();
        //        NetworkCred.UserName = "kundan.mobileappdev@gmail.com";
        //        NetworkCred.Password = "Kundan@2244";
        //        smtp.UseDefaultCredentials = true;
        //        smtp.Credentials = NetworkCred;
        //        smtp.Port = 587;
        //        smtp.Send(mm);
        //        message = "Receipt sent succesfully";
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return message;
        //} 

        public string DeclineEmailSMS(string EmailID)
        {
            clsBookManagement bm = new clsBookManagement();
            string status = null;
            string ServerName = SMTPSERVER; int PORTNO = 587;//25 //443 //587       
            string Sender = USERNAME; string credential = PASSWORD;            
            SmtpClient smtpClient = new SmtpClient(ServerName, PORTNO);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(USERNAME, PASSWORD);
            smtpClient.EnableSsl = true;
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(USERNAME);
                message.Subject = "Support Ankur Pratishthan";
                message.Body = "Your Donation amount is declined due to" +"cheque bounce";
                message.IsBodyHtml = true;

                message.To.Add(EmailID);
                try
                {
                    smtpClient.Send(message);
                    status = "Y";                    
                }
                catch (Exception ex)
                {
                    bm.InsertError(EmailID, "SendDEclineEmail", "Message" + ex.Message + "StackTrace" + ex.StackTrace, "SendDEclineEmail");
                }
            }
            return status;
        }

        #endregion AP Donation Management System
    }
}
