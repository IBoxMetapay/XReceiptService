using HtmlAgilityPack;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Font;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XReceiptService.ReceiptPDF.JsonHelper;

namespace XReceiptService.ReceiptPDF
{
    public class CreatePdfFile : ICreatePdfFile
    {
        public List<string> HtmlExamples = new List<string>();
        /// <summary>
        /// Rebuild data to PDF
        /// </summary>
        /// <param name="payment">class object with data</param>
        /// <param name="HtmlExample">example of html to input</param>
        /// <returns></returns>
        public async Task<byte[]> PDFFile(string payment, string HtmlExample)
        {
            if (string.IsNullOrEmpty(HtmlExample))
                throw new ArgumentNullException();

            var _payment = JsonConvert.DeserializeObject<ReceiptResp>(payment);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var htmlText = HtmlExample;
            byte[] fileContents = null;
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(htmlText);
            UniversalMacrosReplace universalMacrosReplace = new UniversalMacrosReplace();
            using (MemoryStream ms = new MemoryStream())
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                if (_payment?.payment != null && _payment.payment.Count != 0)
                {
                    var HtmlWithRightFields = universalMacrosReplace.DataToPdf(_payment, HtmlExample);
                    var operInfText = universalMacrosReplace.OperInfoDataToPdf(_payment);
                    if (operInfText != null)
                    {
                        if (operInfText.Count != 0)
                        {
                            foreach (var v in operInfText)
                            {
                                if (v != null && !string.IsNullOrEmpty(v))
                                    HtmlWithRightFields.Add(v);
                            }
                        }
                    }

                    ConverterProperties properties = new ConverterProperties();
                    PdfWriter writer = new PdfWriter(ms);
                    iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                    Document document = new Document(pdf);
                    PdfMerger pdfMerger = new PdfMerger(pdf);

                    //adding corporative font to pdf
                    FontProvider fontProvider = new iText.Html2pdf.Resolver.Font.DefaultFontProvider(false, false, false);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_Bold);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_Regular);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_BoldItalic);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_Italic);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_Light);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_LightItalic);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_MedItalic);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_Medium);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_Thin);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_ThinItalic);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_XBlack);
                    fontProvider.AddFont(Properties.Resources.PFSquareSansPro_XBlackItal);
                    properties.SetFontProvider(fontProvider);
                    //adding corporative font to pdf

                    foreach (var t in HtmlWithRightFields)
                    {
                        if (t != null && !string.IsNullOrEmpty(t))
                        {
                            MemoryStream baos = new MemoryStream();
                            iText.Kernel.Pdf.PdfDocument temp = new iText.Kernel.Pdf.PdfDocument(new PdfWriter(baos));
                            HtmlConverter.ConvertToPdf(t, temp, properties);
                            ReaderProperties rp = new ReaderProperties();
                            baos = new MemoryStream(baos.ToArray());
                            temp = new iText.Kernel.Pdf.PdfDocument(new iText.Kernel.Pdf.PdfReader(baos, rp));
                            pdfMerger.Merge(temp, 1, temp.GetNumberOfPages());
                            temp.Close();
                        }
                    }
                    document.Close();
                    pdfMerger.Close();
                    fileContents = ms.ToArray();
                }
                File.WriteAllBytes("nolayoutPage.pdf", fileContents); //------ uncomment when debug on Windows
                return await Task.FromResult(fileContents);
            }
        }
    }
}