using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Enums
{
    public enum PodStatus
    {
        [Description("NotFound")]
        NotFound = 0,
        [Description("Pending")]
        Pending = 1,
        [Description("Running")]
        Running = 2,
        [Description("Succeeded")]
        Succeeded = 3,
        [Description("Failed")]
        Failed = 4,
        [Description("Unknown")]
        Unknown = 5,
        [Description("Error")]
        Error = 6
    }
}
