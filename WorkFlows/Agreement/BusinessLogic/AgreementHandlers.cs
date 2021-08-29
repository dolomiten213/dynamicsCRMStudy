using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlows.Agreement.BusinessLogic
{
    public static class AgreementHadlers
    {
        public static bool HasLinkedInvoices(EntityReference agreementRef, IOrganizationService service)
        {
            var query = new QueryExpression("autod_invoice");
            query.ColumnSet.AddColumns("autod_name");
            query.Criteria.AddCondition("autod_dogovorid", ConditionOperator.Equal, agreementRef.Id.ToString());

            var res = service.RetrieveMultiple(query);
            return res.Entities.Count > 0;
        }

        public static bool HasLinkedManualOrPaidInvocies(EntityReference agreementRef, IOrganizationService service)
        {
            var query_0_0_autod_dogovorid = $"{agreementRef.Id}";
            var query_0_0_autod_fact = true;
            var query_0_1_autod_dogovorid = $"{agreementRef.Id}";
            var query_0_1_autod_type = 415210000;

            var query = new QueryExpression("autod_invoice");

            query.ColumnSet.AddColumns("autod_name");

            var query_Criteria_0 = new FilterExpression();
            query.Criteria.AddFilter(query_Criteria_0);

            query_Criteria_0.FilterOperator = LogicalOperator.Or;

            var query_Criteria_0_0 = new FilterExpression();
            query_Criteria_0.AddFilter(query_Criteria_0_0);
            query_Criteria_0_0.AddCondition("autod_dogovorid", ConditionOperator.Equal, query_0_0_autod_dogovorid);
            query_Criteria_0_0.AddCondition("autod_fact", ConditionOperator.Equal, query_0_0_autod_fact);

            var query_Criteria_0_1 = new FilterExpression();
            query_Criteria_0.AddFilter(query_Criteria_0_1);
            query_Criteria_0_1.AddCondition("autod_dogovorid", ConditionOperator.Equal, query_0_1_autod_dogovorid);
            query_Criteria_0_1.AddCondition("autod_type", ConditionOperator.Equal, query_0_1_autod_type);

            var res = service.RetrieveMultiple(query);

            return res.Entities.Count > 0;
        }
    
        public static void DeleteLinkedAutoInvoices(EntityReference agreementRef, IOrganizationService service)
        {
            var query_autod_dogovorid = $"{agreementRef.Id}";
            var query_autod_type = 415210001;

            var query = new QueryExpression("autod_invoice");

            query.ColumnSet.AddColumns("autod_name");

            query.Criteria.AddCondition("autod_dogovorid", ConditionOperator.Equal, query_autod_dogovorid);
            query.Criteria.AddCondition("autod_type", ConditionOperator.Equal, query_autod_type);

            var res = service.RetrieveMultiple(query);

            foreach (var a in res.Entities)
            {
                service.Delete(a.LogicalName, a.Id);
            }

        }
    
        public static void CreateSchedule(EntityReference agreementRef, IOrganizationService service)
        {
            var response = service.Retrieve(agreementRef.LogicalName, agreementRef.Id, new ColumnSet("autod_creditperiod", "autod_creditamount", "autod_date" , "autod_name"));
            var creditAmount = response.GetAttributeValue<Money>("autod_creditamount");
            
            if (creditAmount == null)
            {
                return;
            }


            var creditPeriod = response.GetAttributeValue<int>("autod_creditperiod");

            if (creditPeriod == 0)
            {
                return;
            }

            var agreementName = response.GetAttributeValue<string>("autod_name");

            if (agreementName == null)
            {
                agreementName = "";
            }

            var amount = creditAmount.Value;
            var months = 12 * creditPeriod;
            var payment = amount / months;
            var date = response.GetAttributeValue<DateTime>("autod_date");
            date = date == DateTime.MinValue ? DateTime.Now : date;

            for (int i = 0; i < months; i++)
            {
                Entity newInvoice = new Entity("autod_invoice");
                newInvoice["autod_amount"] = new Money(payment);
                newInvoice["autod_type"] = new OptionSetValue(415210001);
                newInvoice["autod_dogovorid"] = agreementRef;
                newInvoice["autod_date"] = date.AddMonths(i);
                newInvoice["autod_name"] = $"{i + 1}-й платеж по договору \"{agreementName}\"";

                service.Create(newInvoice);
            }

            var updateAgreement = new Entity(agreementRef.LogicalName, agreementRef.Id);

            updateAgreement["autod_paymentplandate"] = DateTime.Now.AddDays(1);
            service.Update(updateAgreement);
        }
        
    }
}
