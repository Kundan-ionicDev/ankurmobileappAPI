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

namespace AnkurPrathisthan
{
    public class clsQRCode
    {
        //To generate QRCode
        public string GenerateQRCode(string BookID, string BookName)
        {
            string qr_id = "";
            try
            {               
                var qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                var qrCode = qrEncoder.Encode(BookID+BookName);
                long i= 1;
                foreach (byte b in Guid.NewGuid().ToByteArray())
                {
                    i *= ((int)b+1);
                }
                 qr_id = String.Format("{0:d9}", (DateTime.Now.Ticks/10)%1000000000);
                var renderer = new GraphicsRenderer(new FixedModuleSize(5, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                string path = "F:/k_dev/QRCodes/" + BookID + ".png";
                using (var stream = new FileStream(path, FileMode.Create))
                renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);               
            }
            catch (Exception ex)
            {               
                throw ex;
            }
            return qr_id + ".png";
        }

        public DataSet GetCode(string EmailID)
        {
            DataSet ds = new DataSet();
            try
            {
                string ProcName = "ankurmobileapp.GetQRCodes";
                SqlParameter[] oParam = null;
                oParam = new SqlParameter[1];
                oParam[0] = new SqlParameter("@EmailID", EmailID);
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