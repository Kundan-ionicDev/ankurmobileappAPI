using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gma.QrCodeNet.Encoding;
using System.IO;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Data.SqlClient;
using AnkurPrathisthan.Entity;

namespace AnkurPrathisthan
{
    public class clsQRCode
    {
        //To generate QRCode
         string qrimg = null;

        public string GenerateQRCode(string BookID, string BookName)
        {
            string qr_id = "";
            try
            {
                var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                var qrCode = qrEncoder.Encode(BookID + BookName);
                long i = 1;
                foreach (byte b in Guid.NewGuid().ToByteArray())
                {
                    i *= ((int)b + 1);
                }
                qr_id = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
                var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                string path = "F:/k_dev/QRCodes/" + BookID + ".png";
                using (var stream = new FileStream(path, FileMode.Create))
                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);

                qrimg = qr_id + ".png";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return qrimg;
        }        
        

        //protected void GeneratePDF(object sender, EventArgs e)
        //{
        //    string base64 = Convert.ToBase64String(byteImage);
        //    byte[] imageBytes = Convert.FromBase64String(base64);
        //    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
        //    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
        //    {
        //        Document document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
        //        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
        //        document.Open();
        //        document.Add(image);
        //        document.Close();
        //        byte[] bytes = memoryStream.ToArray();
        //        memoryStream.Close();
        //        Response.Clear();
        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("Content-Disposition", "attachment; filename=Image.pdf");
        //        Response.ContentType = "application/pdf";
        //        Response.Buffer = true;
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.BinaryWrite(bytes);
        //        Response.End();
        //    }
        //}

        public DataSet GetCode(string EmailID)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.GetQRCodes";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[1];
                oParam[0] = new SqlParameter("@EmailID", EmailID);
              //  oParam[1] = new SqlParameter("@Mode", Mode);
                //oParam[2] = new SqlParameter("@BookID", Mode);
                ds = AnkurPrathisthan.clsSQL.SqlHelper.ExecuteDataset(AnkurPrathisthan.clsSQL.SqlHelper.ConnectionString(1), CommandType.StoredProcedure, ProcName, oParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;

        }

        public DataSet UpdateQrStatus(string EmailID)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.ManageQRCodes";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[1];
                oParam[0] = new SqlParameter("@EmailID", EmailID);
                //  oParam[1] = new SqlParameter("@Mode", Mode);
                //oParam[2] = new SqlParameter("@BookID", Mode);
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