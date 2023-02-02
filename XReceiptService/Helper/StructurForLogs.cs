using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XReceiptService.Helper
{
    public class StructurForLogs
    {
        public string Method { get; set; }
        public string Text { get; set; }
        public int? Status { get; set; }
        public string RequestId { get; set; }
        public string Version { get; set; }
        public AdditionalParams AdditionalParam { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AdditionalParams
    {
        public string CacheName { get; set; }
        public string OracleMethod { get; set; }
        public string GlobalParamas { get; set; }
    }
}