using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XReceiptService.Helper
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ReceieptServiceRequest
    {
        public long? id { get; set; }
        public int? destinationResult { get; set; }
        public int? needOrderTaxNum { get; set; }
        public string receiptId { get; set; }
        public string recepient { get; set; }

        public int? onlySuccess { get; set; }
        public string requestId { get; set; }
        public int? source { get; set; }
        public int? waitWF { get; set; }
        public string command { get; set; }
        public string ip { get; set; }
        public string company { get; set; }
        public string fingerprint { get; set; }

    }
}