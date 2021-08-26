using Microsoft.Xrm.Sdk;
using plugin3.Invoice.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Invoice
{
    public sealed class PostInvoiceUpdate : MyPlugin
    {
        public override void Execute(IServiceProvider serviceProvider)
        {

            var (invoice, trace, service) = GetEntityAndTraceAndOrganizationService(serviceProvider);

            try
            {
                InvoiceTypeFiller.IfNullThenManual(invoice);
                AgreementFactSummaFiller.IfIsPaidThenEqualsInvoiceAmount(invoice, trace, service);
                AgreementFactSummaChecker.DispalyErrorIfInvoicesSumMoreThanAgreementAmount(invoice, trace, service);
            }
            catch (Exception e)
            {
                trace.Trace(e.ToString());
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
