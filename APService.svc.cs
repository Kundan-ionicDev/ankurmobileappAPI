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


namespace AnkurPrathisthan
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class APService : IAPService
    {       
        //[START] FOR login & logout
        public userdetailsEntity UserLogin(string EmailID, string Password, string deviceinfo, string isnewapp)
        {
            if (deviceinfo == null)
            {
                deviceinfo = "";
            }
            if (isnewapp == null)
            {
                isnewapp = "";
            }
         //   string result = "";
            DataSet ds = new DataSet();
            clsAuthentication objGeneral = new clsAuthentication();
            
            userdetailsEntity entity = new userdetailsEntity();
            //   UserID,  FirstName,  LastName,   RoleName,
            //OrphanageName,  CreatedDate, string ModifiedDate = "", string isActive = "",  OldPassword = "";
            try
            {

                if (EmailID == null || Password == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 0";
                }
                else
                {
                    ds = objGeneral.GetUserDetails(EmailID,Password);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
                        }
                    }
                   
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 1";
                }
               
            }

            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- UserLogin" + ex.Message, EmailID, Password, deviceinfo, isnewapp);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
            }

            finally
            {
                ds.Clear();
                ds.Dispose();
                
            }

           // byte[] resultbytes = Encoding.UTF32.GetBytes(entity);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
            return entity;
          //  return new MemoryStream(entity);
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

                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 1";
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- UserLogout" + ex.Message, EmailID);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
            }
            return result;
        }

        public userdetailsEntity UserRegister(string FirstName,string LastName,string EmailID, string Password, string DOB, string MobileNo) //,string RoleName)
        {
            DataSet ds = new DataSet();
            clsAuthentication objGeneral = new clsAuthentication();
            userdetailsEntity entity = new userdetailsEntity();           
            try
            {
                if (EmailID == null || Password == null || FirstName == null ) //|| RoleName == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }

                else
                {
                    ds = objGeneral.RegisterUser(FirstName, LastName, EmailID, Password, DOB, MobileNo); //, RoleName);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //entity.Firstname = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]);
                            //entity.Lastname = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]);
                            //entity.EmailId = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);
                            //entity.Password = Convert.ToString(ds.Tables[0].Rows[i]["Password"]);
                          //  entity.Dob = Convert.ToString(ds.Tables[0].Rows[i]["DOB"]);
                          //  entity.Mobile = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                            //; entity.Rolename = Convert.ToString(ds.Tables[0].Rows[i]["RoleName"]);
                            entity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
                        }
                    }

                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 1";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- UserRegistration" + ex.Message, EmailID,Password,FirstName); //RoleName);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
            }
            return entity;
        }

        public userdetailsEntity ForgotPassword(string EmailID, string Password)
        {
            DataSet ds = new DataSet();
            clsAuthentication objGeneral = new clsAuthentication();
            userdetailsEntity entity = new userdetailsEntity();
            try
            {
                if (EmailID == null || Password == null )
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }

                else
                {
                    ds = objGeneral.PasswordReset(EmailID, Password);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                           /* entity.EmailId = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);
                            entity.Password = Convert.ToString(ds.Tables[0].Rows[i]["Password"]); */
                            entity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
                        }
                    }

                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 1";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- ForgotPassword" + ex.Message, EmailID, Password);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
            }
            return entity;
        }
        //[END] For login & logout

        //[START] For Book Management
        
        //API on book management home page
       // public BookDetailsEntity GetBooks()
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
                            entity.BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]);
                            entity.AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["AuthorName"]);
                            entity.Price = Convert.ToString(ds.Tables[0].Rows[i]["Price"]);
                            entity.Stock = Convert.ToString(ds.Tables[0].Rows[i]["Stock"]);
                            entity.CategoryName = Convert.ToString(ds.Tables[0].Rows[i]["CategoryName"]);
                            entity.Language = Convert.ToString(ds.Tables[0].Rows[i]["LanguagesName"]);
                                
                            entity.add(new BookDetailsEntity{
                                BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]),
                                AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["AuthorName"])
                            });
                        }
                    }
                       
               // }
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- GetBoks" + ex.Message);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";              
            }
            // string result = JsonConvert.SerializeObject(ds);
            return entity;
          //  return entity;
        }       

        public BookDetailsEntity ManageBooks(string BookName, string cmd, string EmailID, string Price , string Author, string Stock , string CategoryID ,
        string LanguageID , string PublisherID, string BookID = "")       
        {
            BookDetailsEntity entity = new BookDetailsEntity();
            DataSet ds = new DataSet();
            clsBookManagement bm = new clsBookManagement();
            clsQRCode  qrc = new clsQRCode();
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
                if (BookName ==null || cmd == null )
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }                

                else
                {
                    ds  =  bm.HandleBooks(BookName,cmd,EmailID,Price,Author,Stock,CategoryID,LanguageID,PublisherID,BookID); 
 
                     if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //entity.BookID = Convert.ToString(ds.Tables[0].Rows[i]["BookID"]); 
                            entity.BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]);                           
                            entity.AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["Author"]);
                            entity.Price = Convert.ToString(ds.Tables[0].Rows[i]["Price"]);
                            entity.Stock = Convert.ToString(ds.Tables[0].Rows[i]["Stock"]);
                            entity.CategoryID = Convert.ToString(ds.Tables[0].Rows[i]["CategoryID"]);
                            entity.LanguageID = Convert.ToString(ds.Tables[0].Rows[i]["LanguageID"]);
                            entity.EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);                           
                            entity.PublisherID = Convert.ToString(ds.Tables[0].Rows[i]["PublisherID"]);
                            entity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
                        }
                         if (cmd != null && cmd == "1" )
                         {
                            qrcode = qrc.GenerateQRCode(entity.BookID,entity.BookName,entity.AuthorName);                             
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
 

        public CategoryDetails ManageCategories (string cmd, string CategoryName, string Email, string CategoryID ="" )
        {
            CategoryDetails enitity = new CategoryDetails();

            clsBookManagement bm = new clsBookManagement();
            DataSet ds = new DataSet();
            if (Email == null)
                Email = "";
            
            try
            {
                if (cmd == null || CategoryName == null )
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
                                enitity.ModifiedBy = Convert.ToString(ds.Tables[0].Rows[i]["ModifiedBy"]);
                                enitity.CategoryName = Convert.ToString(ds.Tables[0].Rows[i]["CategoryName"]);
                                enitity.CreatedBy = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]);
                                enitity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
                            }                           
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- ManageCategories" + ex.Message);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
            }

            return enitity;
        }
        public LanguageDetails ManageLanguages(string cmd, string LanguageName, string Email, string LanguageID = "")
        {
            LanguageDetails entity = new LanguageDetails();
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
                    ds = bm.HandleLanguages(cmd,LanguageName,Email,LanguageID);
                    //For adding
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //if (ds.Tables[0].Rows[0]["cmd"].ToString() = "1")
                        //{

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.ModifiedBy = Convert.ToString(ds.Tables[0].Rows[i]["ModifiedBy"]);
                            entity.LanguageName= Convert.ToString(ds.Tables[0].Rows[i]["LanguageName"]);
                            entity.CreatedBy = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]);
                            entity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
                        }
                        // }
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

        public PublisherDetails ManagePublishers(string cmd, string PublisherName, string Email, string PublisherID = "")
        {
            PublisherDetails entity = new PublisherDetails();
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
                            entity.ModifiedBy = Convert.ToString(ds.Tables[0].Rows[i]["ModifiedBy"]);
                            entity.PublisherName = Convert.ToString(ds.Tables[0].Rows[i]["PublisherName"]);
                            entity.CreatedBy = Convert.ToString(ds.Tables[0].Rows[i]["CreatedBy"]);
                            entity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
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
        public string GetClusters()
         //   public ClusterDetailsEntity GetClusters()
        {
            clsClusterManagement cm = new clsClusterManagement();
            DataSet ds = new DataSet();
           // ClusterDetailsEntity entity = new ClusterDetailsEntity();
            try
            {
                ds = cm.ShowClusters();
              /*  if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]);
                        entity.Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]);
                        entity.MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                        entity.Members = Convert.ToString(ds.Tables[0].Rows[i]["Members"]);                      
                        entity.Librarian = Convert.ToString(ds.Tables[0].Rows[i]["Librarian"]);

                    }
                } */
            }
            catch (Exception ex)
            {
                Console.WriteLine("AP Service---Error in API --GetClusters" +ex.Message);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                throw;
            }
            string result = JsonConvert.SerializeObject(ds);
            return result;
        }

        public ClusterDetailsEntity ManageClusters(string ClusterName, string ClusterCode, string cmd, string EmailID, string Address, string MobileNo,
        string LibEmailID,string Members,string AdminEmailID, string ClusterID ="")

        {
            ClusterDetailsEntity entity = new ClusterDetailsEntity();
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
            if (LibEmailID == null)
                LibEmailID = "";
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
                    ds = bm.HandleClusters(ClusterName,ClusterCode,cmd,EmailID,Address,MobileNo,LibEmailID,Members,AdminEmailID,ClusterID);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]);
                            entity.ClusterCode = Convert.ToString(ds.Tables[0].Rows[i]["ClusterCode"]);
                            entity.Email = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);
                            entity.Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]);
                            entity.MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                            entity.Librarian = Convert.ToString(ds.Tables[0].Rows[i]["Librarian"]);
                            entity.Members = Convert.ToString(ds.Tables[0].Rows[i]["Members"]);                      
                            entity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
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
        public string GetLibrarians()
          //  public LibrarianDetailsEntity GetLibrarians()
        {
           // LibrarianDetailsEntity entity = new LibrarianDetailsEntity();
            DataSet ds = new DataSet();
            clsLibrarianManagement lm = new clsLibrarianManagement();
            try
            {
                ds = lm.ShowLibrarians();
               /* if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]);
                        entity.Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]);
                        entity.MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                        entity.EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);
                        entity.ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]);

                    }
                } */
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in API ---GetLibrarians" + ex.Message);
                throw ex;
            }
            string result = JsonConvert.SerializeObject(ds);
            return result;
        }
        public LibrarianDetailsEntity ManageLibrarians(int cmd, string FirstName, string LastName, string EmailID, string Address, string MobileNo, 
        string AltMobileNo, string ClusterID, string AdminEmailID, string LibrarianID = "")
        {
            LibrarianDetailsEntity entity = new LibrarianDetailsEntity();
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
                ds = lm.HandleLibrarians( cmd,  FirstName,  LastName, EmailID,  Address,  MobileNo,  AltMobileNo, ClusterID, AdminEmailID,  LibrarianID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]);
                        entity.LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]);
                        entity.EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);
                        entity.Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]);
                        entity.MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                        entity.AltMobileNo = Convert.ToString(ds.Tables[0].Rows[i]["AltMobileNo"]);                        
                        entity.ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]);
                        entity.AdminEmailID = Convert.ToString(ds.Tables[0].Rows[i]["AdminEmailID"]);
                        entity.LibrarianID = Convert.ToString(ds.Tables[0].Rows[i]["LibrarianID"]);

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
        public string GetMembers()
            //   public MemberDetailsEntity GetMembers()
        {
            //MemberDetailsEntity entity = new MemberDetailsEntity();
            DataSet ds = new DataSet();
            clsMemberManagement mem = new clsMemberManagement();
            try
            {
                ds = mem.ShowMembers();
                /*if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]);
                        entity.Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]);
                        entity.MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                        entity.EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);
                        entity.ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]);

                    }
                } */
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in API ---GetMembers" + ex.Message);
                throw ex;
            }
            string result = JsonConvert.SerializeObject(ds);
            return result;
        }

        public MemberDetailsEntity ManageMembers(int cmd, string FirstName, string LastName, string EmailID, string Address, string MobileNo,
        string AltMobileNo, string ClusterID,string DOB, string AdminEmailID,string MemberID = "")
        {
            MemberDetailsEntity entity = new MemberDetailsEntity();
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
                ds = mem.HandleMembers(cmd, FirstName, LastName, EmailID, Address, MobileNo, AltMobileNo, ClusterID, DOB, AdminEmailID,MemberID);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        entity.FirstName = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]);
                        entity.LastName = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]);
                        entity.EmailID = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);
                        entity.Address = Convert.ToString(ds.Tables[0].Rows[i]["Address"]);
                        entity.MobileNo = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                        entity.AltMobileNo = Convert.ToString(ds.Tables[0].Rows[i]["AltMobileNo"]);
                        entity.ClusterID = Convert.ToString(ds.Tables[0].Rows[i]["ClusterID"]);                       
                        entity.MemberID = Convert.ToString(ds.Tables[0].Rows[i]["MemberID"]);

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
        public string GetRequests()
        {
            DataSet ds = new DataSet();
           // RequestsDetailsEntity requests = new RequestsDetailsEntity();
            clsApprovals app = new clsApprovals();
            try
            {
                ds = app.ShowRequests();              
            }
            catch (Exception ex)
            {                
                throw ex; 
            }
            string result = JsonConvert.SerializeObject(ds);
            return result;

        }

        public RequestsDetailsEntity ManageRequests()
        {
            RequestsDetailsEntity requests = new RequestsDetailsEntity();
            DataSet ds = new DataSet();
            clsApprovals approvals = new clsApprovals();
            try
            {
                ds = approvals.HandleRequests();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
               {
                   for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                   {
                       requests.RequestedFor = Convert.ToString(ds.Tables[0].Rows[i]["RequestedFor"]);
                       requests.RequestedDate = Convert.ToString(ds.Tables[0].Rows[i]["RequestedDate "]);
                       requests.BookName = Convert.ToString(ds.Tables[0].Rows[i]["BookName"]);
                       requests.AuthorName = Convert.ToString(ds.Tables[0].Rows[i]["AuthorName"]);
                       requests.RequestedBy = Convert.ToString(ds.Tables[0].Rows[i]["RequestedBy"]);
                       requests.ClusterName = Convert.ToString(ds.Tables[0].Rows[i]["ClusterName"]);                       
                   }
               } 
            }
            catch (Exception ex) 
            {                
                throw ex;
            }
            return requests;
        }


        //[END] For Approvals
        // [START] Test jsonconvert newtonsoft
        public string Test(string name)
        {
           //name = "";
           string result = JsonConvert.SerializeObject(name);
           return result;
        }
        // [END] Test jsonconvert newtonsoft
    }
}
