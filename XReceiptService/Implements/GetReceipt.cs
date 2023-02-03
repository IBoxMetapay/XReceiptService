using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XReceiptService.Contracts;
using XReceiptService.Helper;
using XReceiptService.ReceiptPDF;
using XReceiptService.ReceiptPDF.JsonHelper;

namespace XReceiptService.Implements
{
    public class GetReceipt : IGetReceipt
    {
        private readonly Serilog.ILogger logService;
        public GetReceipt(Serilog.ILogger LogService)
        {
            logService = LogService;
        }

        public async Task<(ReceipServicetResponse, int)> AcceptGetReceiptAsync(string command, string htmltemplate)
        {
            try
            {
                var req = JsonConvert.DeserializeObject<ReceiptResp>(command);
                var pdf = new CreatePdfFile().PDFFile(command, htmltemplate).ConfigureAwait(false).GetAwaiter().GetResult();
                var resp = new ReceipServicetResponse()
                {
                    status = 200,
                    requestId = req.requestId,
                    recepient = req.receiptId,
                    body = Convert.ToBase64String(pdf),
                    bodyFileName = $"receipt.pdf"
                };

                return (resp, 200);
            }
            catch (Exception _ex)
            {
                logService.Information(JsonConvert.SerializeObject(new StructurForLogs()
                {
                    Method = "GetReceipt",
                    Text = $"{_ex}",
                    Status = 400,
                    RequestId = JsonConvert.DeserializeObject<ReceiptResp>(command).requestId
                }, Formatting.None));
                var _response = new ReceipServicetResponse
                {
                    status = 400
                };
                return (_response, 400);
            }
        }
    }
}