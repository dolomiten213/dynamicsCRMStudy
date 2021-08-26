using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Invoice.BusinessLogic
{
    public static class AgreementFactSummaFiller
    {
        public static void IfIsPaidThenEqualsInvoiceAmount(Entity invoice, ITracingService trace, IOrganizationService service)
        {
            var isPaid = (bool)invoice.GetAttributeValue<bool>("autod_fact");
            if (isPaid)
            {
                UpdateAgreementAmount(invoice, trace, service);
            }
        }
        public static void IfDeleteThenEqualsInvoiceAmount(EntityReference invoice, ITracingService trace, IOrganizationService service)
        {
            var invoiceEntity = service.Retrieve(invoice.LogicalName, invoice.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet("autod_dogovorid", "autod_amount"));
            var agreementReference = invoiceEntity.GetAttributeValue<EntityReference>("autod_dogovorid");
            if (agreementReference == null)
            {
                return;
            }
            var money = invoiceEntity.GetAttributeValue<Money>("autod_amount");

            var updateAgreement = new Entity(agreementReference.LogicalName, agreementReference.Id);
            updateAgreement["autod_factsumma"] = money;
            service.Update(updateAgreement);
        }

        static void UpdateAgreementAmount(Entity invoice, ITracingService trace, IOrganizationService service)
        {
            var agreementReference = invoice.GetAttributeValue<EntityReference>("autod_dogovorid");
            var money = invoice.GetAttributeValue<Money>("autod_amount");
            
            if (agreementReference == null || money == null)
            {
                var invoiceEntity = service.Retrieve(invoice.LogicalName, invoice.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet("autod_dogovorid", "autod_amount"));
                agreementReference = invoiceEntity.GetAttributeValue<EntityReference>("autod_dogovorid");
                money = invoiceEntity.GetAttributeValue<Money>("autod_amount");
            }

            var updateAgreement = new Entity(agreementReference.LogicalName, agreementReference.Id);
            updateAgreement["autod_factsumma"] = money;
            service.Update(updateAgreement);
        }
    }
}
