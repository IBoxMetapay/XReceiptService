using System;
using System.Threading.Tasks;
using XReceiptService.ReceiptPDF.JsonHelper;

namespace XReceiptService.ReceiptPDF
{
    public interface ICreatePdfFile
    {
        Task<byte[]> PDFFile(string payment, string HtmlExample);
    }
}