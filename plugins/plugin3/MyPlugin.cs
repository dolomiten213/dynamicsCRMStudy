using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3
{
    abstract public class MyPlugin : IPlugin
    {
        
        public abstract void Execute(IServiceProvider serviceProvider);

        
        protected (Entity, ITracingService, IOrganizationService) GetEntityAndTraceAndOrganizationService(IServiceProvider serviceProvider)
        {
            var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetEntity = (Entity)pluginContext.InputParameters["Target"];

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var serviceOrganization = serviceFactory.CreateOrganizationService(Guid.Empty);

            return (targetEntity, traceService, serviceOrganization);
        }
        protected (Entity, ITracingService) GetEntityAndTrace(IServiceProvider serviceProvider)
        {
            var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetEntity = (Entity)pluginContext.InputParameters["Target"];

            return (targetEntity, traceService);
        }
        protected Entity GetEntity(IServiceProvider serviceProvider)
        {
            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetEntity = (Entity)pluginContext.InputParameters["Target"];

            return targetEntity;
        }

        protected (EntityReference, ITracingService, IOrganizationService) GetEntityRefAndTraceAndOrganizationService(IServiceProvider serviceProvider)
        {
            var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetEntity = (EntityReference)pluginContext.InputParameters["Target"];

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var serviceOrganization = serviceFactory.CreateOrganizationService(Guid.Empty);

            return (targetEntity, traceService, serviceOrganization);
        }
        protected (EntityReference, ITracingService) GetEntityRefAndTrace(IServiceProvider serviceProvider)
        {
            var traceService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetEntity = (EntityReference)pluginContext.InputParameters["Target"];

            return (targetEntity, traceService);
        }
        protected EntityReference GetEntityRef(IServiceProvider serviceProvider)
        {
            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var targetEntity = (EntityReference)pluginContext.InputParameters["Target"];

            return targetEntity;
        }

    }
}
