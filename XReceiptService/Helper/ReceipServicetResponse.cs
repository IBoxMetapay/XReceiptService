using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XReceiptService.Helper
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ReceipServicetResponse
    {
        public string body { get; set; }
        public byte[] bodyForControler { get; set; }
        public string bodyFileName { get; set; }
        public int status { get; set; }
        public string recepient { get; set; }
        public string requestId { get; set; }
        public int? size { get; set; }
    }
}