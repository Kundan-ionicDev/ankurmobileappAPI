using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gma.QrCodeNet.Encoding;
using System.IO;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.Drawing.Imaging;

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


    }
}