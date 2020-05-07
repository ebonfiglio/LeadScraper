using LeadScraper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Infrastructure
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
              => options.UseSqlite("Data Source=LeadScraperDb.db");

    public virtual DbSet<Setting> Settings { get; set; }

    }
}
