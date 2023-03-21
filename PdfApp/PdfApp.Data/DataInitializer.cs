using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfApp.Data
{
    public class DataInitializer
    {
        private readonly PdfAppDbContext _dbContext;

        public DataInitializer(PdfAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void EnsureMigrated()
        {
            _dbContext.Database.Migrate();
        }
    }
}
