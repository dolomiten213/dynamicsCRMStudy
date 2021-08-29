using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using WorkFlows.Agreement.BusinessLogic;

namespace WorkFlows.Agreement
{
    public class CheckLinkedInvoicesActivity : CodeActivity
    {
        [Input("Agreement")]
        [RequiredArgument]
        [ReferenceTarget("autod_agreement")]
        public InArgument<EntityReference> AgreementRef { get; set; }

        [Output("Has Linked Invoice")]
        public OutArgument<bool> hasLinkedInvoice { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(null);
            var agreementRef = AgreementRef.Get(context);

            hasLinkedInvoice.Set(context, AgreementHadlers.HasLinkedInvoices(agreementRef, service));
        }
    }
}
