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
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 1";
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- UserLogin" + ex.Message, EmailID, Password, deviceinfo);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
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
            string Message = ""; 
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
                        string objIsEmailSent = obj.SendOTPEmail(EmailID, newOTP);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- SendOTPEmail" + ex.Message, EmailID);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
            }
            return Message;
        }
        //[END] FOR FORGOT PASSWORD VIA OTP ON EMAIL
        //[END] For login & logout

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
                            PublisherName = Convert.ToString(ds.Tables[0].Rows[i]["PublisherName"])
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

                objBooks.Add(new BookDetails()
                {
                    Categories = objCategories,
                    Languages = objLanguages,
                    Publishers = objPublishers
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
        string LanguageID, string PublisherID, string BookID = "")
        {
            List<BookDetailsEntity> entity = new List<BookDetailsEntity>();
            DataSet ds = new DataSet();
            clsBookManagement bm = new clsBookManagement();
            clsQRCode qrc = new clsQRCode();
            string qrcode = "";
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
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }

                else
                {
                    ds = bm.HandleBooks(BookName, cmd, EmailID, Price, Author, Stock, CategoryID, LanguageID, PublisherID, BookID);

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
                            });

                        }
                        if (BookID != "" && BookID != null)
                        {
                            qrcode = qrc.GenerateQRCode(BookID, BookName, Author);
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
        // public string GetClusters()
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
                            LibrarianID = Convert.ToString(ds.Tables[0].Rows[i]["Librarian"])
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
        string LibrarianID, string Members, string AdminEmailID, string ClusterID = "")
        {
            List<ClusterDetailsEntity> entity = new List<ClusterDetailsEntity>();
            DataSet ds = new DataSet();
            clsClusterManagement bm = new clsClusterManagement();
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
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }

                else
                {
                    ds = bm.HandleClusters(ClusterName, ClusterCode, cmd, EmailID, Address, MobileNo, LibrarianID, Members, AdminEmailID, ClusterID);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            // entity.Add (new ClusterDetailsEntity)
                            entity.Add(new ClusterDetailsEntity
                            {
                                ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                                ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]),
                                ClusterCode = Convert.ToString(ds.Tables[0].Rows[i]["ClusterCode"]),
                                Email = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                                Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                                MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                                Members = Convert.ToString(ds.Tables[0].Rows[i]["Members"]),
                                //  LibrarianID = Convert.ToString(ds.Tables[0].Rows[i]["LibrarianID"]),
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
                            LibrarianName = Convert.ToString(ds.Tables[0].Rows[i]["LibrarianName"]),
                            ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                            ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]),
                            Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                            MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                            EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"])
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
            string Address, string MobileNo,
        string AltMobileNo, string ClusterID, string AdminEmailID, string LibrarianID = "")
        {
            List<LibrarianDetailsEntity> entity = new List<LibrarianDetailsEntity>();
            DataSet ds = new DataSet();
            clsLibrarianManagement lm = new clsLibrarianManagement();
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
                ds = lm.HandleLibrarians(cmd, FirstName, LastName, EmailID, Address, MobileNo, AltMobileNo, ClusterID, AdminEmailID, LibrarianID);
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
                           MemberName = Convert.ToString(ds.Tables[0].Rows[i]["MemberName"]),
                           Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]),
                           MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]),
                           EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]),
                           ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]),
                           ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"])

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
        string AltMobileNo, string ClusterID, string DOB, string AdminEmailID, string MemberID = "")
        {
            List<MemberDetailsEntity> entity = new List<MemberDetailsEntity>();
            DataSet ds = new DataSet();
            clsMemberManagement mem = new clsMemberManagement();
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
                ds = mem.HandleMembers(cmd, FirstName, LastName, EmailID, Address, MobileNo, AltMobileNo, ClusterID, DOB, AdminEmailID, MemberID);
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
    }
}
