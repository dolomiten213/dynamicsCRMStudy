using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Contract.BusinessLogic
{
    public static class ContactFirstAgreementDateFiller
    {
        public static void IfNullThenAgreementDate(Entity agreement, IOrganizationService service)
        {
            var contactReference = agreement.GetAttributeValue<EntityReference>("autod_contact");

            var contactEntity = service.Retrieve("contact", contactReference.Id, new ColumnSet("autod_date"));

            var contactDate = contactEntity.GetAttributeValue<DateTime>("autod_date");


            if (contactDate == DateTime.MinValue)
            {
                var agreementDateEntity = service.Retrieve("autod_agreement", agreement.Id, new ColumnSet("autod_date"));
                var agreementDate = agreementDateEntity.GetAttributeValue<DateTime>("autod_date");

                contactEntity["autod_date"] = agreementDate;
                service.Update(contactEntity);

            }
        }
    }
}
