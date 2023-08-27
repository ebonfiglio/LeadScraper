using LeadScraper.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeadScraper.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public string DbPath { get; }
        public ApplicationDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "LeadScraperDb.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WhoIsServer>().HasData(new WhoIsServer { Tld = ".com", Server = "whois.verisign-grs.com" },
                new WhoIsServer { Tld = ".biz", Server = "whois.biz" },
                new WhoIsServer { Tld = ".net", Server = "whois.verisign-grs.com" });
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
              => options.UseSqlite($"Data Source={DbPath}");

        public virtual DbSet<Setting> Settings { get; set; }

        public virtual DbSet<SearchSetting> SearchSetting { get; set; }

        public virtual DbSet<WhoIsServer> WhoIsServers { get; set; }

    }
}
