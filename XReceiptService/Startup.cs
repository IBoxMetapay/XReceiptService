using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XReceiptService.Common;
using XReceiptService.Contracts;
using XReceiptService.Helper;
using XReceiptService.Implements;
using XReceiptService.ReceiptPDF;

namespace XReceiptService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<Serilog.ILogger>(CreateLoger().CreateLogger());
            services.AddTransient<ICreatePdfFile, CreatePdfFile>();
            services.AddTransient<IGetReceipt, GetReceipt>();
            services.AddSingleton<ReceiptServiceActivator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var serviceProvider = app.ApplicationServices;
            appLifetime.ApplicationStopping.Register(() =>
            {
                string delayForShutdown = Configuration.GetValue<string>("AppSettings:delayForShutdown");
                var _delay = int.TryParse(delayForShutdown, out int _del) && _del > 400 ? _del : 400;
                var requestId = "-" + Guid.NewGuid().ToString();
                Globals.isShutDown = 1;
                DateTime dateTime = DateTime.Now;
                var logService = (Serilog.ILogger)serviceProvider.GetService(typeof(Serilog.ILogger));
                logService.Information(JsonConvert.SerializeObject(new StructurForLogs()
                {
                    Method = "shutdown",
                    Text = "Received command to shutdown application",
                    RequestId = requestId
                }, Formatting.None));
                bool _tt = true;
                while (_tt)
                {
                    if (!(Globals.utilizeMain > 0))
                    {
                        _tt = false;
                    };
                    Task.Delay(_delay).Wait();
                }

                logService.Information(JsonConvert.SerializeObject(new StructurForLogs()
                {
                    Method = "shutdown",
                    Text = "Shutdown application",
                    RequestId = requestId,
                    AdditionalParam = new AdditionalParams()
                    {
                        GlobalParamas = $"utilizeMain = {Globals.utilizeMain}"
                    }
                }, Formatting.None));
            });
            serviceProvider.GetService<ReceiptServiceActivator>().StartRecieptService().Wait();
        }

        private LoggerConfiguration CreateLoger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.Async(t => t.Console(outputTemplate: "[{Timestamp:yyyy-MMM-dd HH:mm:ss.fff}] [{Level}] {Message}{NewLine}{Exception}"));
        }
    }
}
