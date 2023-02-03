using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XReceiptService.ReceiptPDF.JsonHelper
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ReceiptResp
    {
        public int? agentId { get; set; }
        public int? businesOperType { get; set; }
        public string currencyAll { get; set; }
        public string operationSumAll { get; set; }
        public string receiptDate { get; set; }
        public string receiptId { get; set; }
        public string receiptRepostIdAll { get; set; }
        public string requestId { get; set; }
        public int? resultError { get; set; }
        public int? salePointId { get; set; }
        public string serviceName { get; set; }
        public string serviceSumAll { get; set; }
        public string statusAll { get; set; }
        public string taxSumAll { get; set; }
        public List<Payment> payment { get; set; }
        public int? receiptTemplateTypeId { get; set; }     //10208 - 2. Добавление параметра
    }
}