using AnkurPrathisthan.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
//using System.Text.Encoding;

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
            clsGeneral objGeneral = new clsGeneral();
            
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
            clsGeneral objGeneral = new clsGeneral();
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

        public userdetailsEntity UserRegister(string FirstName,string LastName,string EmailID, string Password, string DOB, string MobileNo,string RoleName)
        {
            DataSet ds = new DataSet();
            clsGeneral objGeneral = new clsGeneral();
            userdetailsEntity entity = new userdetailsEntity();           
            try
            {
                if (EmailID == null || Password == null || FirstName == null || RoleName == null)
                {
                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
                }

                else
                {
                    ds = objGeneral.RegisterUser(FirstName, LastName, EmailID, Password, DOB, MobileNo, RoleName);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            entity.Firstname = Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]);
                            entity.Lastname = Convert.ToString(ds.Tables[0].Rows[i]["LastName"]);
                            entity.EmailId = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);
                            entity.Password = Convert.ToString(ds.Tables[0].Rows[i]["Password"]);
                            entity.Dob = Convert.ToString(ds.Tables[0].Rows[i]["DOB"]);
                            entity.Mobile = Convert.ToString(ds.Tables[0].Rows[i]["MobileNo"]);
                            entity.Rolename = Convert.ToString(ds.Tables[0].Rows[i]["RoleName"]);
                            //entity.Message = Convert.ToString(ds.Tables[0].Rows[i]["Message"]);
                        }
                    }

                    WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 1";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in API-- UserRegistration" + ex.Message, EmailID,Password,FirstName,RoleName);
                WebOperationContext.Current.OutgoingResponse.ContentType = "Flag, 2";
            }
            return entity;
        }

        public userdetailsEntity ForgotPassword(string EmailID, string Password)
        {
            DataSet ds = new DataSet();
            clsGeneral objGeneral = new clsGeneral();
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
                            entity.EmailId = Convert.ToString(ds.Tables[0].Rows[i]["EmailID"]);
                            entity.Password = Convert.ToString(ds.Tables[0].Rows[i]["Password"]);                           
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
        
        public BookDetailsEntity GetBooks (string BookName)
        {
            //string result = "";
            BookDetailsEntity entity = new BookDetailsEntity();
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
            return entity;
        }
        //[END] For Book Management 
    }
}
