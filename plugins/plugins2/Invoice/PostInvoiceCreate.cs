using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugins2.Invoice
{
    public sealed class PostInvoiceCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            traceService.Trace("Get ITracingService");

            //var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            //var targetInvoice = (Entity)pluginContext.InputParameters["Target"];

            //var name = targetInvoice.GetAttributeValue<string>("autod_name");
            //traceService.Trace($"Имя счета: {name}");
            

            throw new InvalidPluginExecutionException("My Exception");
        }
    }
}
