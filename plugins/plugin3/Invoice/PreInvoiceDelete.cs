using Microsoft.Xrm.Sdk;
using plugin3.Invoice.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Invoice
{
    public sealed class PreInvoiceDelete : MyPlugin
    {
        public override void Execute(IServiceProvider serviceProvider)
        {

            var (invoiceRef, trace, service)  = GetEntityRefAndTraceAndOrganizationService (  serviceProvider  );

            try
            {
                AgreementFactSummaFiller.IfDeleteThenEqualsInvoiceAmount(  invoiceRef, trace, service  );
            }
            catch (Exception e)
            {
                trace.Trace(e.ToString());
                throw new InvalidPluginExecutionException(  e.Message  );
            }
        }
    }
}
