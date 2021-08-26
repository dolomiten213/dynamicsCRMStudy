using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Invoice
{
    public sealed class PostInvoiceCreate : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            throw new Exception("My Exception");
        }
    }
}
