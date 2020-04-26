using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System.Data;
using System.IO;

namespace AnkurPrathisthan
{
    public class clsPDF
    {
       //public string CreatePDFReceipt (DataSet ds, string EmailID, string Filename,string DonorID)
       // {
       //    string tempfile="", Filepath=string.Empty;
       //    DataTable dtreceipt = new DataTable();
       //    DataRow DEmailID;

       //    if ((ds.Tables["DonorEmailID"].Rows.Count>0))
       //    {
       //        DEmailID = ds.Tables["DonorEmailID"].Rows[0]; 
       //    }
       //    Document doc = new Document(PageSize.A4, 0, 0, 0, 0);
       //    using (MemoryStream mem = new MemoryStream())
       //    {
       //        try
       //        {
       //            if (!Directory.Exists((AppDomain.CurrentDomain.BaseDirectory + "Temp_Files\\")))
       //            {
       //                Directory.CreateDirectory((AppDomain.CurrentDomain.BaseDirectory + "Temp_Files\\"));
       //            }
       //            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream((AppDomain.CurrentDomain.BaseDirectory + ("Temp_Files\\" +
       //                (tempfile + ".pdf"))), FileMode.Create));
       //            PdfContentByte cb; PdfContentByte white; PdfContentByte red;
       //            ColumnText ct; ColumnText wt; ColumnText tr;
       //            doc.Open();
       //            cb = writer.DirectContent;
       //            red = writer.DirectContent; white = writer.DirectContent;
       //            ct = new ColumnText(cb); wt = new ColumnText(white); tr = new ColumnText(red);
       //            int mrec = 0;
       //            float mbot = 570;
       //            float mleft = 25;
       //            float mBottName = 760;
       //            float mheght = 65;
       //            float mLeftdiff = 100;
       //            float mLeftAdd = 25;
       //            int intPage = 0;
       //            DataTable dt2 = new DataTable();
       //          //  DataRow DataRow DataRow DataRow DataRow DataRow DataRow 
       //            string mfooter;
       //            ct.SetSimpleColumn(new Phrase(new Chunk((Convert.ToString(Convert.ToString("AnkurPratishthan")+mfooter)+"To"))+mfooter),FontFactory.GetFont("Arial",8,Font.BOLD))),160,788,700,36,25,(Element.ALIGN_CENTER|Element.ALIGN_TOP));
       //             ct.Go();

                  
                   







 

       //        }
       //        catch (Exception)
       //        {
                   
       //            throw;
       //        }
       //    }
       // }
    }
}
