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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WhoIsServer>().HasData(new WhoIsServer { Tld = "com", Server = "whois.verisign-grs.com" },
                new WhoIsServer { Tld = "biz", Server = "whois.biz" },
                new WhoIsServer { Tld = "net", Server = "whois.verisign-grs.com" });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
              => options.UseSqlite("Data Source=LeadScraperDb.db");

    public virtual DbSet<Setting> Settings { get; set; }
    public virtual DbSet<WhoIsServer> WhoIsServers { get; set; }

    }
}
