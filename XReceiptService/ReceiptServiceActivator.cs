using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XReceiptService.Common;
using XReceiptService.Helper;

namespace XReceiptService
{
    public class ReceiptServiceActivator
    {
        protected ILogger _logger { get; set; }
        IServiceProvider serviceProvider { get; set; }
        IConfiguration configuration { get; set; }
        public ReceiptServiceActivator(IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger)
        {
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
            _logger = logger;
        }

        public async Task StartRecieptService()
        {
            Globals.Version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            Dictionary<string, string> notValidConfiguration = new Dictionary<string, string>();

            _logger.Information(JsonConvert.SerializeObject(new StructurForLogs()
            {
                Method = "application",
                Text = "информация о старте приложения",
                Version = Globals.Version
            }, Formatting.None));
            Globals.RaizeFinishStartApp(this);
        }
    }
}