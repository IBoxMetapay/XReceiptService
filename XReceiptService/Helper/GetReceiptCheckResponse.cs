using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XReceiptService.Helper
{
    public class GetReceiptCheckResponse
    {
        public string receiptId { get; set; }
        public string requestId { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public Body body { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GetReceiptCheckPayment
    {
        public string sender { get; set; }
        public string recipient { get; set; }
        public long? amount { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public long? currencyCode { get; set; }
        public long? commissionRate { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Body
    {
        public List<GetReceiptCheckPayment> payments { get; set; }
        public string link { get; set; }
    }
}