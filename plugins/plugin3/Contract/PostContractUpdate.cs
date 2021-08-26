using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using plugin3.Contract.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Contract
{
    public sealed class PostContractUpdate : MyPlugin
    {
        public override void Execute(IServiceProvider serviceProvider)
        {
            var (agreement, trace, service) = GetEntityAndTraceAndOrganizationService(serviceProvider);

            try
            {
                ContactFirstAgreementDateFiller.IfNullThenAgreementDate(agreement, service);
            }
            catch (Exception e)
            {
                trace.Trace(e.ToString());
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
