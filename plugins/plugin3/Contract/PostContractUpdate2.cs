using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using plugin3.Contract.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Contract
{
    public sealed class PostContractUpdate2 : MyPlugin
    {
        public override void Execute(IServiceProvider serviceProvider)
        {
            var (agreement, trace, service) = GetEntityAndTraceAndOrganizationService(serviceProvider);

            try
            {
                ContractStatusAutoFiller.IfPaidSumEqualsTotalAmountThenPaid(agreement, trace, service);
            }
            catch (Exception e)
            {
                trace.Trace(e.ToString());
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
