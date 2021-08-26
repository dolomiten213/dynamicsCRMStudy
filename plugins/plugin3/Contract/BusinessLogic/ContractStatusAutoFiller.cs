using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Contract.BusinessLogic
{
    public static class ContractStatusAutoFiller
    {
        public static void IfPaidSumEqualsTotalAmountThenPaid(Entity contract, ITracingService trace, IOrganizationService service)
        {
            var agreementEntityWithAmountAndPaidAmount = service.Retrieve(contract.LogicalName, contract.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet("autod_summa", "autod_factsumma"));
                       
            var toPay = agreementEntityWithAmountAndPaidAmount.GetAttributeValue<Money>("autod_summa");
            var paid = agreementEntityWithAmountAndPaidAmount.GetAttributeValue<Money>("autod_factsumma");
            
            if (toPay == null || paid == null)
            {
                return;
            }

            if (paid.Value >= toPay.Value)
            {
                Entity agreementToUpdate = new Entity(contract.LogicalName, contract.Id);
                agreementToUpdate["autod_fact"] = true;
                service.Update(agreementToUpdate);
            }
        }
    }
}
