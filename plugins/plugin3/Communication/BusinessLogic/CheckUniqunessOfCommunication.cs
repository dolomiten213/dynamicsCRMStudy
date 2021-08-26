using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Communication.BusinessLogic
{
    public static class CheckUniqunessOfCommunication
    {
        public static void ThrowExceptionIfMainPhoneAlreadyExist(Entity com, ITracingService trace, IOrganizationService service)
        {
            var t = service.Retrieve(com.LogicalName, com.Id, new ColumnSet("autod_type", "autod_main"));
            var main = t.GetAttributeValue<bool>("autod_main");
            if (!main)
            {
                return;
            }
            var type = t.GetAttributeValue<OptionSetValue>("autod_type");
            if (type == null)
            {
                return;
            }

            if (HasMainCommunication(com, trace, service, type.Value))
            {
                throw new InvalidPluginExecutionException("already have main comms this type");
            }
        }

        static bool HasMainCommunication(Entity com, ITracingService trace, IOrganizationService service, int type)
        {
            var a = service.Retrieve(com.LogicalName, com.Id, new ColumnSet("autod_contactid"));
            var contactReference = a.GetAttributeValue<EntityReference>("autod_contactid");
            if (contactReference == null)
            {
                trace.Trace("Не удалось получить contact_id");
            }

            var query_autod_type = type;
            var query_autod_main = true;
            var query_autod_contactid = $"{contactReference.Id}";
            var query_autod_communicationid = $"{com.Id}";

            trace.Trace($"{query_autod_type} ||| {query_autod_main} ||| {query_autod_contactid}");


            // Instantiate QueryExpression query
            var query = new QueryExpression("autod_communication");

            // Add columns to query.ColumnSet
            query.ColumnSet.AddColumns("autod_name");

            // Define filter query.Criteria
            query.Criteria.AddCondition("autod_type", ConditionOperator.Equal, query_autod_type);
            query.Criteria.AddCondition("autod_contactid", ConditionOperator.Equal, query_autod_contactid);
            query.Criteria.AddCondition("autod_main", ConditionOperator.Equal, query_autod_main);
            query.Criteria.AddCondition("autod_communicationid", ConditionOperator.NotEqual, query_autod_communicationid);

            var result = service.RetrieveMultiple(query);

            foreach (var b in result.Entities)
            {
                trace.Trace($"{b.Id} ||| {b.LogicalName} ||| {b.GetAttributeValue<string>("autod_name")}");
            }

            return result.Entities.Count > 0;

        }
    }
}
