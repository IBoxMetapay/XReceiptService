using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XReceiptService.Contracts;
using XReceiptService.Helper;
using XReceiptService.Implements;
using XReceiptService.ReceiptPDF;
using XReceiptService.ReceiptPDF.JsonHelper;

namespace XReceiptService.Controllers
{
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Serilog.ILogger logService;
        public ReceiptController(IServiceProvider serviceProvider, Serilog.ILogger logService)
        {
            this.serviceProvider = serviceProvider;
            this.logService = logService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("api/receipt")]
        public async Task<HttpResponseMessage> Process()
        {
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

            logService.Information(JsonConvert.SerializeObject(new StructurForLogs()
            {
                Method = "GetReceipt",
                Text = requestBody,
                Status = 200,
                RequestId = JsonConvert.DeserializeObject<ReceiptResp>(requestBody).requestId
            }, Formatting.None));

            CancellationTokenSource source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(28));
            var tcs = new TaskCompletionSource<(ReceipServicetResponse, int)>();
            source.Token.Register(() =>
            {
                tcs.SetCanceled();
            });
            var services = serviceProvider.GetServices<IGetReceipt>();

            Task<(ReceipServicetResponse, int)> flowRespons = Task.Run(() => services.FirstOrDefault().AcceptGetReceiptAsync(requestBody, Properties.Resources.RECEIPT_BODY_1_0_2_0));
            var tasks = new Task[]
            {
                flowRespons, tcs.Task
            };
            await Task.WhenAny(tasks);
            if (flowRespons.IsCompleted)
            {
                Response.Headers.Add("requestId", flowRespons.Result.Item1.requestId);
                var content = JsonConvert.SerializeObject(flowRespons.Result.Item1);
                logService.Information(JsonConvert.SerializeObject(new StructurForLogs()
                {
                    Method = "GetReceipt",
                    Text = content,
                    Status = 200,
                    RequestId = flowRespons.Result.Item1.requestId
                }, Formatting.None));

                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                };
            }
            else
            {
                var requestToRecepient = JsonConvert.DeserializeObject<ReceieptServiceRequest>(requestBody);
                Response.Headers.Add("requestId", requestToRecepient.requestId);
                logService.Information(JsonConvert.SerializeObject(new StructurForLogs()
                {
                    Method = "GetReceipt",
                    Text = "Время ожидания флоу истекло",
                    Status = 408,
                    RequestId = requestToRecepient.requestId,

                }, Formatting.None));                
                return new HttpResponseMessage()
                {
                    StatusCode = (HttpStatusCode)StatusCode(408).StatusCode,
                };
            }
        }

        [HttpGet]
        [Route("api/receiptVersion")]
        public IActionResult GetVersion()
        {
            var _version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            return Ok(_version);
        }
    }
}