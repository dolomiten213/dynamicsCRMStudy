using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using plugin3.Communication.BusinessLogic;
using System.Text;

namespace plugin3.Communication
{
    public sealed class PostCommunicationCreate : MyPlugin
    {
        public override void Execute(IServiceProvider serviceProvider)
        {

            var (comms, trace, service) = GetEntityAndTraceAndOrganizationService(serviceProvider);

            try
            {
                CheckUniqunessOfCommunication.ThrowExceptionIfMainPhoneAlreadyExist(comms, trace, service);
            }
            catch (Exception e)
            {
                trace.Trace(e.ToString());
                throw new InvalidPluginExecutionException(e.Message);
            }
        }
    }
}
