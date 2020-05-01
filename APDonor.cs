using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace AnkurPrathisthan
{
    public class APDonor
    {
        //[START] For Email sending         
        public string SMTPSERVER = System.Configuration.ConfigurationManager.AppSettings["SMTPSERVER"].ToString();
        public static string USERNAME = System.Configuration.ConfigurationManager.AppSettings["USERNAME"].ToString();
        public static string PASSWORD = System.Configuration.ConfigurationManager.AppSettings["PASSWORD"].ToString();
        //[END] For Email sending

        public DataSet Login(string Email,string Password,string FCM="")
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.UserLogin";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[3];
                oParam[0] = new SqlParameter("@Email", Email);
                oParam[1] = new SqlParameter("@Password", Password);
                oParam[2] = new SqlParameter("@Fcm", FCM);

                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in Login" + ex.Message);
            }
            return ds;
        }
        public DataSet RegisterVolunteer(string cmd, string FirstName, string LastName, string EmailID, string DOB,
            string Address, string Contact, string AdminEmailID,string Img="",string ImgPath="",string LoginID = "")
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.VolunteerLogin";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[11];
                oParam[0] = new SqlParameter("@FirstName", FirstName);
                oParam[1] = new SqlParameter("@LastName", LastName);
                oParam[2] = new SqlParameter("@EmailID", EmailID);
                oParam[3] = new SqlParameter("@DOB", DOB);
                oParam[4] = new SqlParameter("@Address", Address);
                oParam[5] = new SqlParameter("@ContactNo", Contact);
                oParam[6] = new SqlParameter("@Admin", AdminEmailID);
                oParam[7] = new SqlParameter("@Img", Img);
                oParam[8] = new SqlParameter("@ImgPath", ImgPath);
                oParam[9] = new SqlParameter("@cmd",cmd);
                oParam[10] = new SqlParameter("@LoginID", LoginID);
                
                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in RegisterVolunteer" + ex.Message, EmailID, LastName, FirstName);
            }
            return ds;
        }

        public DataSet VolEmail (string Email, string Password)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.VolCredential";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[2];
                oParam[0] = new SqlParameter("@EmailID", Email);
                oParam[1] = new SqlParameter("@Password", Password);                

                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in VolEmail" + ex.Message);
            }
            return ds;
        }

        public DataSet FetchVolunteers()
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.GetVolunteers";
                SqlParameter[] oParam = null; 
                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in FetchVolunteers" + ex.Message);
            }
            return ds;
        }

        public DataSet SubmitDonors (string FullName, string Inthenameof, string EmailID, string DOB,
            string Address, string Contact, string AdminEmailID, int Amount, string PaymentMode, string DonationTowards, string PAN, string AmountInWords, string Description = "")
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.RegisterDonation";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[13];
                oParam[0] = new SqlParameter("@Donatedby", FullName);
                oParam[1] = new SqlParameter("@Inthenameof", Inthenameof);
                oParam[2] = new SqlParameter("@Contact", Contact);
                oParam[3] = new SqlParameter("@Address", Address);
                oParam[4] = new SqlParameter("@Email", EmailID);
                oParam[5] = new SqlParameter("@Amount", Amount);
                oParam[6] = new SqlParameter("@PaymentMode", PaymentMode);
                oParam[7] = new SqlParameter("@Description", Description);
                oParam[8] = new SqlParameter("@AddedBy", AdminEmailID);
                oParam[9] = new SqlParameter("@DOB", DOB);
                oParam[10] = new SqlParameter("@Donationtowards", DonationTowards);
                oParam[11] = new SqlParameter("@PAN", PAN);
                oParam[12] = new SqlParameter("@Amountwords", AmountInWords);    

                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet handleDonors(string FullName, string Inthenameof, string EmailID, string DOB,
            string Address, string Contact, string AdminEmailID, int Amount, string PaymentMode, string DonationTowards, string PAN, 
            string AmountInWords,int cmd, int DonorID, string Description = "")
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.ManageDonors";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[15];
                oParam[0] = new SqlParameter("@Donatedby", FullName);
                oParam[1] = new SqlParameter("@Inthenameof", Inthenameof);
                oParam[2] = new SqlParameter("@Contact", Contact);
                oParam[3] = new SqlParameter("@Address", Address);
                oParam[4] = new SqlParameter("@Email", EmailID);
                oParam[5] = new SqlParameter("@Amount", Amount);
                oParam[6] = new SqlParameter("@PaymentMode", PaymentMode);
                oParam[7] = new SqlParameter("@Description", Description);
                oParam[8] = new SqlParameter("@AddedBy", AdminEmailID);
                oParam[9] = new SqlParameter("@DOB", DOB);
                oParam[10] = new SqlParameter("@Donationtowards", DonationTowards);
                oParam[11] = new SqlParameter("@PAN", PAN);
                oParam[12] = new SqlParameter("@Amountwords", AmountInWords);
                oParam[13] = new SqlParameter("@cmd", cmd);
                oParam[14] = new SqlParameter("@DonorID", DonorID);

                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet FetchDonors(string VolEmailID,int RoleID)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.GetDonors";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[2];
                oParam[0] = new SqlParameter("@EmailID", VolEmailID);
                oParam[1] = new SqlParameter("@RoleID", RoleID);
                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in FetchDonors" + ex.Message);
            }
            return ds;
        }
        public DataSet DonorBirthdays(string EmailID, int RoleID)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.GetDonorBirthdays";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[2];
                oParam[0] = new SqlParameter("@EmailID", EmailID);
                oParam[1] = new SqlParameter("@RoleID", RoleID);
                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in DonorBirthdays" + ex.Message);
            }
            return ds;
        }

        public DataSet SubmitCelebrateReqeusts(int cmd,string FirstName, string LastName, string EmailID, string Contact,string Date,string VolEmailID,int AreaID,int OccassionID, string ID)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.RegisterRequest";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[10];
                oParam[0] = new SqlParameter("@cmd", cmd);
                oParam[1] = new SqlParameter("@FirstName", FirstName);
                oParam[2] = new SqlParameter("@LastName", LastName);
                oParam[3] = new SqlParameter("@EmailID", EmailID);
                oParam[4] = new SqlParameter("@Contact", Contact);
                oParam[5] = new SqlParameter("@Date", Date);
                oParam[6] = new SqlParameter("@VolEmailID", VolEmailID);
                oParam[7] = new SqlParameter("@AreaID", AreaID);
                oParam[8] = new SqlParameter("@OccassionID", OccassionID);
                oParam[9] = new SqlParameter("@ID", ID);

                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in SubmitCelebrateRequests" + ex.Message, EmailID, LastName, FirstName);
            }
            return ds;
        }

        public DataSet GetAllReqeusts(string EmailID, int RoleID)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.GetCelebrateRequest";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[2];
                oParam[0] = new SqlParameter("@EmailID", EmailID);
                oParam[1] = new SqlParameter("@RoleID", RoleID);
                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
            return ds;
        }

        public string SendCelebrateEmail(string EmailID)
        {
            clsBookManagement bm = new clsBookManagement();
            string status = ""; 
            string ServerName = SMTPSERVER;
            int PORTNO = 587;//25 //443 //587       
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
                message.Body = "Hello!Your celebrate with us request added succesfully. For further details our Admin will contact you on your provided contact details";
                message.IsBodyHtml = true;

                message.To.Add(EmailID);
                try
                {
                    smtpClient.Send(message);
                    status = "Y";
                }
                catch (Exception ex)
                {
                    bm.InsertError(EmailID, "SendCelebrateEmail", "Message" + ex.Message + "StackTrace" + ex.StackTrace, "SendCelebrateEmail");
                }
            }
            return status;
        }

        public DataSet GetImages()
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.GetSplashImages";
                SqlParameter[] oParam = null;
                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in GetSplashImages" + ex.Message);
            }
            return ds;
        }

        public DataSet ReceiptDonor(string EmailID,int DonorID)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.RegisterRequest";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[10];
                oParam[0] = new SqlParameter("@EmailID", EmailID);
                oParam[1] = new SqlParameter("@DonorID", DonorID);                              

                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in ReceiptDonor" + ex.Message, EmailID, DonorID);
            }
            return ds;
        }

        public DataSet CheckUser(string EmailID, string otp)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.CheckUser";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[2];
                oParam[0] = new SqlParameter("@EmailID", EmailID);
                oParam[1] = new SqlParameter("@OTP", otp);

                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in ReceiptDonor" + ex.Message, EmailID, otp);
            }
            return ds;
        }

        public DataSet ChangePassword(string EmailID, string Password, int Otp)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.uspChangePassword";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[3];
                oParam[0] = new SqlParameter("@EmailID", EmailID);
                oParam[1] = new SqlParameter("@Password", Password);
                oParam[2] = new SqlParameter("@OTP", Otp);

                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("APService----Error in ChangePassword" + ex.Message, EmailID, Otp);
            }
            return ds;
        }

        public DataSet Contactus(string Fullname,string EmailID, string Query, int contact)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.uspContactus";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[4];
                oParam[0] = new SqlParameter("@Fullname", Fullname);
                oParam[1] = new SqlParameter("@EmailID", EmailID);
                oParam[2] = new SqlParameter("@contact", contact);
                oParam[3] = new SqlParameter("@Query", Query);

                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

    }
}