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

    }
}
