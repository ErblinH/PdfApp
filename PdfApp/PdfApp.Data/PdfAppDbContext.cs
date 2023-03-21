using Microsoft.EntityFrameworkCore;
using PdfApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Data
{
    public class PdfAppDbContext : DbContext
    {
        public PdfAppDbContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<ConverterMargins> ConverterMargins { get; set; }
        public DbSet<ConverterOptions> ConverterOptions { get; set; }
        public DbSet<ConvertJob> ConvertJob { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            SetBaseEntityData();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetBaseEntityData()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (e.State is EntityState.Added or EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdateDate = DateTime.UtcNow;

                if (entityEntry.State is EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).InsertDate = DateTime.UtcNow;
                }
            }
        }
    }
}
