using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Entities
{
    public partial interface ISoftDeletedEntity
    {
        bool Deleted { get; set; }
    }
}
