using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Domain.Entities
{
    public abstract class BaseEntity : ISoftDeletedEntity
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid InsertedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
