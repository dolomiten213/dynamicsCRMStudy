using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Invoice.BusinessLogic
{
    public static class AgreementFactSummaChecker
    {
        public static void DispalyErrorIfInvoicesSumMoreThanAgreementAmount(Entity invoice, ITracingService trace, IOrganizationService service)
        {
            var isPaid = (bool)invoice.GetAttributeValue<bool>("autod_fact");
            if (isPaid)
            {
                var invoiceEntityWithContractId = service.Retrieve("autod_invoice", invoice.Id, new ColumnSet("autod_dogovorid"));
                var contractEntityRef = invoiceEntityWithContractId.GetAttributeValue<EntityReference>("autod_dogovorid");
                trace.Trace("1");
                var query_autod_dogovorid = contractEntityRef.Id;
                trace.Trace("2");

                var query = new QueryExpression("autod_invoice");
                query.ColumnSet.AddColumns("autod_amount");
                query.Criteria.AddCondition("autod_dogovorid", ConditionOperator.Equal, query_autod_dogovorid);
                trace.Trace("3");

                var a = service.RetrieveMultiple(query);
                Decimal sum = 0;
                trace.Trace("4");
                foreach (var b in a.Entities)
                {
                    var invoiceAmount = b.GetAttributeValue<Money>("autod_amount");
                    var amount = invoiceAmount == null ? 0 : invoiceAmount.Value;
                    trace.Trace($"{amount}");
                    sum += amount;
                }
                trace.Trace("5");
                trace.Trace($"{contractEntityRef.Id}");
                var contractEntityWithAmount = service.Retrieve("autod_agreement", contractEntityRef.Id, new ColumnSet("autod_summa"));
                var contractAmount = contractEntityWithAmount.GetAttributeValue<Money>("autod_summa");
                trace.Trace("6");
                if (contractAmount == null)
                {
                    return;
                }
                trace.Trace(contractAmount.Value.ToString());
                if (contractAmount.Value < sum)
                {
                    throw new InvalidPluginExecutionException("Сумма всех счетов не может превышать итоговой суммы в договоре");
                } 
                else
                {
                    var updateInvoiceEntity = new Entity(invoice.LogicalName, invoice.Id);
                    updateInvoiceEntity["autod_paydate"] = DateTime.Now;
                    service.Update(updateInvoiceEntity);
                }
            }

        } 
    }
}
