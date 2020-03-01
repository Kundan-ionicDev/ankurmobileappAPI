using AnkurPrathisthan.Entity;
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


        //#region Logs
        //public static void WriteLog (string path, string EmailID="")
        //{
        //    try
        //    {
        //        TextWriter tw = new StreamWriter(@"G:\", true);
        //        tw.WriteLine(EmailID);
        //        tw.Close();
        //    }
        //    catch (Exception ex)
        //    {                
        //        throw ex; 
        //    }
        //}
        //#endregion Logs

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

                if (EmailID == null || Password == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 0";
                }
                else
                {
                    ds = objGeneral.GetUserDetails(EmailID, Password, FCMID);
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

            }

            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- UserLogin" + ex.Message, EmailID, Password, deviceinfo);                
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
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
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
                Console.WriteLine("APService----Error in API-- UserLogout" + ex.Message, EmailID);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
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
                if (EmailID == null || FirstName == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }
                else
                {
                    Password = objAuth.SendEmail(EmailID); //Email sending for new password for new user
                    ds = objAuth.RegisterUser(FirstName, LastName, EmailID, Password, ClusterCode, RoleID, MobileNo, "");
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

            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- UserRegistration" + ex.Message, EmailID, Password, FirstName); //RoleName);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
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

            try
            {
                newOTP = obj.CreateOTP();
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
                Console.WriteLine("APService----Error in API-- SendOTPEmail" + ex.Message, EmailID);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
            }
            return objIsEmailSent;
        }
        //[END] FOR FORGOT PASSWORD VIA OTP ON EMAIL        

        public string ValidateOTP(string EmailID, string OTP,string Password)
        {
            string Message = "";
            DataSet ds = new DataSet();           
            clsAuthentication obj = new clsAuthentication();
            DataSet dspwdchange = new DataSet();
            try
            {
                ds = obj.ValidateOTP(EmailID, OTP);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Message"].ToString()== "VALID")
                    {
                        dspwdchange = obj.PasswordReset(EmailID, Password);
                        if (dspwdchange.Tables.Count>0 )
                        {
                            Message = dspwdchange.Tables[0].Rows[0]["Message"].ToString(); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- ForgotPassword" + ex.Message);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
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
               // WriteLog("GetBooks","kundansakpal@gmail.com");
                ds = objbook.ShowBooks();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.Add(new BookDetailsEntity
                        {
                            BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]),
                            BookID = Convert.ToString(ds.Tables[0].Rows[i]["BookID"]),
                            AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["AuthorName"]),
                            Price = Convert.ToString(ds.Tables[0].Rows[i]["Price"]),
                            Stock = Convert.ToString(ds.Tables[0].Rows[i]["Stock"]),
                            CategoryName = Convert.ToString(ds.Tables[0].Rows[i]["CategoryName"]),
                            CategoryID = Convert.ToString(ds.Tables[0].Rows[i]["CategoryID"]),
                            Language = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesName"]),
                            LanguageID = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesID"]),
                            BookDescription = Convert.ToString(ds.Tables[0].Rows[i]["BookDescription"]),
                            PublisherID = Convert.ToString(ds.Tables[0].Rows[i]["PublisherID"]),
                            ThumbImage = Convert.ToString(ds.Tables[0].Rows[i]["ThumbImage"]),
                            PublisherName = Convert.ToString(ds.Tables[0].Rows[i]["PublisherName"]),
                            Image2 = Convert.ToString(ds.Tables[0].Rows[i]["Image2"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- GetBoks" + ex.Message);
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
                        //objLanguages.Add(new BooksData
                        //{
                        //    AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["AuthorName"]),
                        //    AuthorID = Convert.ToString(ds.Tables[0].Rows[i]["AuthorID"]),
                        //    CategoryName = Convert.ToString(ds.Tables[0].Rows[i]["CategoryName"]),
                        //    CategoryID = Convert.ToString(ds.Tables[0].Rows[i]["CategoryID"]),
                        //    LanguageID = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesID"]),
                        //    LanguageName = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesName"]),
                        //    PublisherID = Convert.ToString(ds.Tables[0].Rows[i]["PublisherID"]),
                        //    PublisherName = Convert.ToString(ds.Tables[0].Rows[i]["PublisherName"]),
                        //});
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

                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        objImages.Add(new Images
                        {
                            BookID = Convert.ToString(ds.Tables[3].Rows[i]["BookID"]),
                            BookImagePath = Convert.ToString(ds.Tables[3].Rows[i]["Image2"]),
                            BookThumbImagePath = Convert.ToString(ds.Tables[3].Rows[i]["ThumbImage"]),

                        });
                    }
                }


                objBooks.Add(new BookDetails()
                {
                    Categories = objCategories,
                    Languages = objLanguages,
                    Publishers = objPublishers,
                    Images = objImages

                });

                //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        bd.Add(new BooksData
                //        {                            
                //            AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["AuthorName"]),
                //            AuthorID = Convert.ToString(ds.Tables[0].Rows[i]["AuthorID"]),
                //            CategoryName = Convert.ToString(ds.Tables[0].Rows[i]["CategoryName"]),
                //            CategoryID = Convert.ToString(ds.Tables[0].Rows[i]["CategoryID"]),
                //            LanguageID = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesID"]),
                //            LanguageName = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesName"]),
                //            PublisherID = Convert.ToString(ds.Tables[0].Rows[i]["PublisherID"]),
                //            PublisherName = Convert.ToString(ds.Tables[0].Rows[i]["PublisherName"]),
                //        });
                //    }                    
                //}
            }
            catch (Exception ex)
            {
                throw ex;
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
                if (BookName == null || cmd == null)
                {
                   
                }
                    else
                    {
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

                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                        ds = bm.HandleBooks(BookName, cmd, EmailID, Price, Author, Stock, CategoryID, LanguageID, PublisherID, BookID,
                            BookDescription, ThumbImgPath,Image2Path, "");

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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


        public List<CategoryDetails> ManageCategories(string cmd, string CategoryName, string Email, string CategoryID = "")
        {
            List<CategoryDetails> entity = new List<CategoryDetails>();
            clsBookManagement bm = new clsBookManagement();
            DataSet ds = new DataSet();
            if (Email == null)
                Email = "";

            try
            {
                if (cmd == null || CategoryName == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }
                else
                {
                    ds = bm.HandleCategories(cmd, CategoryName, Email, CategoryID);
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

            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- ManageCategories" + ex.Message);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
            }

            return entity;
        }
        public List<LanguageDetails> ManageLanguages(string cmd, string LanguageName, string Email, string LanguageID = "")
        {
            List<LanguageDetails> entity = new List<LanguageDetails>();
            clsBookManagement bm = new clsBookManagement();
            DataSet ds = new DataSet();
            if (Email == null)
                Email = "";

            try
            {
                if (cmd == null || LanguageName == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }
                else
                {
                    ds = bm.HandleLanguages(cmd, LanguageName, Email, LanguageID);
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

            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- ManageLanguages" + ex.Message);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
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
                if (cmd == null || PublisherName == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }
                else
                {
                    ds = bm.HandlePublishers(cmd, PublisherName, Email, PublisherID);
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

            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- ManagePublishers" + ex.Message);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
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
                            Image = Convert.ToString(ds.Tables[0].Rows[i]["Img"]),
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("AP Service---Error in API --GetClusters" + ex.Message);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                throw;
            }
            return entity;
        }

        public List<ClusterDetailsEntity> ManageClusters(string ClusterName, string ClusterCode, string cmd, string EmailID, string Address, string MobileNo,
        string LibrarianID, string Members, string AdminEmailID, string Image64 = "", string ClusterID = "")
        {
            List<ClusterDetailsEntity> entity = new List<ClusterDetailsEntity>();
            DataSet ds = new DataSet();
            clsClusterManagement bm = new clsClusterManagement();
            Image Image; string ImagePath = "";
           // string filepath = @"F:\k_dev\AnkurPrathisthan\Uploads\Clusters\";
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
                if (ClusterName == null || cmd == null)
                {
                   
                }

                else
                {
                    if (cmd.Trim() == "1" || cmd.Trim() =="2")
                    {
                        if (Image64 != "" && Image64 != null && ClusterName != null && ClusterName != "")
                        {
                            try
                            {
                                //[START] unique thumb image id 
                                string ImageID = GenerateImageID();
                                //[END] unique thumb  image id
                                string ImageName = ClusterName + ImageID; //ThumbImgNAme
                                Image = Base64ToImage(Image64, filepath, ImageName);//ThumbImgNAme PNG
                                ImagePath = filepath + ImageName;////ThumbImgNAme Path;                        

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    ds = bm.HandleClusters(ClusterName, ClusterCode, cmd, EmailID, Address, MobileNo, LibrarianID, Members, AdminEmailID, ClusterID, ImagePath);

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
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                            EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"])
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

        public List<ClusterHeadEntity> ManageClusterHeads(string FirstName, string LastName, string AdminEmailID, string ClusterHeadID,
        int cmd, string EmailID, string Address, string MobileNo, string AltMobileNo, string ClusterRegionID)
        {
            List<ClusterHeadEntity> cluster = new List<ClusterHeadEntity>();
            clsClusterManagement objCluster = new clsClusterManagement();
            clsAuthentication objauth = new clsAuthentication();
            DataSet dscluster = new DataSet();
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
            if (ClusterHeadID == null)
                ClusterHeadID = "";
            if (ClusterRegionID == null)
                ClusterRegionID = "";

            try
            {
                if (cmd == 1)
                {
                    Password = objauth.SendEmail(EmailID);
                    DataSet dsUser = new DataSet();
                    dsUser = objauth.RegisterUser(FirstName, LastName, EmailID, Password, "", "2", MobileNo, "");
                }
                dscluster = objCluster.HandleClusterHead(cmd, AdminEmailID, FirstName, LastName, EmailID, ClusterHeadID, Address, MobileNo, AltMobileNo, ClusterRegionID);
                if (dscluster.Tables.Count > 0 && dscluster.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dscluster.Tables[0].Rows.Count; i++)
                    {
                        cluster.Add(new ClusterHeadEntity
                        {
                            ClusterHeadID = Convert.ToString(dscluster.Tables[0].Rows[i]["ClusterHeadID"]),
                            ClusterRegionID = Convert.ToString(dscluster.Tables[0].Rows[i]["ClusterRegionId"]),
                            FirstName = Convert.ToString(dscluster.Tables[0].Rows[i]["FirstName"]),
                            LastName = Convert.ToString(dscluster.Tables[0].Rows[i]["LastName"]),
                            Address = Convert.ToString(dscluster.Tables[0].Rows[i]["Address"]),
                            MobileNo = Convert.ToString(dscluster.Tables[0].Rows[i]["MobileNo"]),
                            AltMobileNo = Convert.ToString(dscluster.Tables[0].Rows[i]["AltMobileNo"]),
                            EmailID = Convert.ToString(dscluster.Tables[0].Rows[i]["EmailID"]),
                            AdminEmailID = Convert.ToString(dscluster.Tables[0].Rows[i]["AdminEmailID"]),
                        });
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return cluster;
        }
        ////[END]For CLusterHead ROle

        //[END] For CLuster Management

        //[START] For Librarian Management        


        public List<LibrarianDetailsEntity> GetLibrarians()
        {
            List<LibrarianDetailsEntity> entity = new List<LibrarianDetailsEntity>();
            DataSet ds = new DataSet();
            clsLibrarianManagement lm = new clsLibrarianManagement();
            try
            {
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
                            Image = Convert.ToString(ds.Tables[0].Rows[i]["Img"])
                        });

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in API ---GetLibrarians" + ex.Message);
                throw ex;
            }
            return entity;
        }
        public List<LibrarianDetailsEntity> ManageLibrarians(int cmd, string FirstName, string LastName, string EmailID,
        string Address, string MobileNo,string AltMobileNo, string ClusterID, string AdminEmailID,string Image64, string LibrarianID = "")
        {
            List<LibrarianDetailsEntity> entity = new List<LibrarianDetailsEntity>();
            DataSet ds = new DataSet();
            clsLibrarianManagement lm = new clsLibrarianManagement();
            clsAuthentication objauth = new clsAuthentication();
           string filepath = @"F:\k_dev\AnkurPrathisthan\Uploads\Librarian\";
            //string filepath = @"C:\ankurmobileappAPI-Development\Uploads\Librarian\";
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
                if (cmd == 1)
                    {
                       Password = objauth.SendEmail(EmailID);
                       DataSet dsUser = new DataSet();
                       dsUser = objauth.RegisterUser(FirstName, LastName, EmailID, Password,ClusterID, "3", MobileNo, "");
                    }

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

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                ds = lm.HandleLibrarians(cmd, FirstName, LastName, EmailID, Address, MobileNo, AltMobileNo, ClusterID, AdminEmailID, LibrarianID, ImagePath);
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
                            Image= Convert.ToString(ds.Tables[0].Rows[i]["Img"])
                        });
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in API ---ManageLibrarians" + ex.Message);
                throw ex;
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
                           Image = Convert.ToString(ds.Tables[0].Rows[i]["Img"]),

                       });
                    }
                }
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
            // string filepath = @"F:\k_dev\AnkurPrathisthan\Uploads\Members\";
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
                if (cmd == 1)
                {
                    Password = objauth.SendEmail(EmailID);
                    DataSet dsUser = new DataSet();
                    dsUser = objauth.RegisterUser(FirstName, LastName, EmailID, Password, "", "4", MobileNo, "");
                }

                if (cmd == 1 || cmd == 2)
                {
                    if (Image64 != "" && Image64 != null && FirstName != null && ClusterID != "")
                    {
                        try
                        {
                            //[START] unique thumb image id 
                            string ImageID = GenerateImageID();
                            //[END] unique thumb  image id
                            string ImageName = FirstName+ClusterID+ImageID; //ThumbImgNAme
                            Image = Base64ToImage(Image64, filepath,ImageName);//ThumbImgNAme PNG
                            ImagePath = filepath + ImageName;////ThumbImgNAme Path;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                ds = mem.HandleMembers(cmd, FirstName, LastName, EmailID, Address, MobileNo, AltMobileNo, ClusterID, DOB, AdminEmailID, MemberID, ImagePath);
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
                Console.WriteLine("Error in API ---ManageMembers" + ex.Message);
                throw ex;
            }
            return entity;
        }

        //[END] For Member Management

        //[START] For Approvals
        public List<RequestsDetailsEntity> GetRequests()
        {
            DataSet ds = new DataSet();
            List<RequestsDetailsEntity> requests = new List<RequestsDetailsEntity>();
            clsApprovals app = new clsApprovals();
            try
            {
                ds = app.ShowRequests();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        requests.Add(new RequestsDetailsEntity
                        {
                            MemberName = Convert.ToString(ds.Tables[0].Rows[i]["MemberName"]),
                            BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]),
                            AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["AuthorName"]),
                            ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]),
                            Status = Convert.ToString(ds.Tables[0].Rows[i]["Status"]),
                            RequestedDate = Convert.ToString(ds.Tables[0].Rows[i]["RequestedDate"]),
                            LibrarianName = Convert.ToString(ds.Tables[0].Rows[i]["LibrarianName"]),
                            RequestStatus = Convert.ToString(ds.Tables[0].Rows[i]["RequestStatus"])
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

        public List<RequestsDetailsEntity> ManageRequests(int cmd, string FirstName, string LastName, string BookID, string LibrarianID,
        string MemberID, string ClusterID, string RequestID, int AuthorID, string RequestStatus)
        {
            List<RequestsDetailsEntity> requests = new List<RequestsDetailsEntity>();
            DataSet ds = new DataSet();
            clsApprovals approvals = new clsApprovals();
            try
            {
                ds = approvals.HandleRequests(cmd, FirstName, LastName, BookID, LibrarianID, MemberID, ClusterID, RequestID, AuthorID, RequestStatus);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        requests.Add
                       (new RequestsDetailsEntity()
                    {
                        MemberID = ds.Tables[0].Rows[i]["MemberID"].ToString(),
                        RequestID = ds.Tables[0].Rows[i]["RequestID"].ToString(),
                        BookID = ds.Tables[0].Rows[i]["BookID"].ToString(),
                        AuthorID = ds.Tables[0].Rows[i]["AuthorID"].ToString(),
                        LibrarianID = ds.Tables[0].Rows[i]["LibrarianID"].ToString(),
                        ClusterID = ds.Tables[0].Rows[i]["ClusterID"].ToString()
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

        private static Image Base64ToImage(string base64string, string filepath,string imgname)
        {
            Image image= null ;
            string result = "";
            try
            {                               
               result = filepath + imgname + ".jpeg";
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
            return String.Format("{0:D3}", random);
        }
    }
}
