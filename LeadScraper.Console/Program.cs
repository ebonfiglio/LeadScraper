using LeadScraper.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

IConfiguration config = new ConfigurationBuilder()
    .Build();
var serviceProvider = new ServiceCollection()
    .AddDbContext<ApplicationDbContext>();