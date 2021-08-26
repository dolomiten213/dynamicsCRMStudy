using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace plugin3.Invoice.BusinessLogic
{
    static class InvoiceTypeFiller
    {
        public static void IfNullThenManual(Entity invoice)
        {
            var type = invoice.GetAttributeValue<OptionSetValue>("autod_type");
            if (type == null)
            {
                invoice["autod_type"] = new OptionSetValue(415210000);
            }
        }
    }
}
