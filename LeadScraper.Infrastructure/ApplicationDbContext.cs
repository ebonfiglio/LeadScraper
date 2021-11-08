using LeadScraper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Infrastructure
{
    public class ApplicationDbContext :DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<WhoIsServer>().HasData(new WhoIsServer { Id= 0, Tld = ".com", Server = "whois.verisign-grs.com" },
            //    new WhoIsServer { Id = 1, Tld = ".biz", Server = "whois.biz" },
            //    new WhoIsServer { Id = 2, Tld = ".net", Server = "whois.verisign-grs.com" });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
              => options.UseSqlite("Filename=LeadScraperDb.db");

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<SearchSetting> SearchSetting { get; set; }

    public virtual DbSet<WhoIsServer> WhoIsServers { get; set; }

    }
}
