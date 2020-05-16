using Microsoft.EntityFrameworkCore.Migrations;

namespace LeadScraper.Infrastructure.Migrations
{
    public partial class WhoIs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhoIsServers",
                columns: table => new
                {
                    Tld = table.Column<string>(nullable: false),
                    Server = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhoIsServers", x => x.Tld);
                });

            migrationBuilder.InsertData(
                table: "WhoIsServers",
                columns: new[] { "Tld", "Server" },
                values: new object[] { "com", "whois.verisign-grs.com" });

            migrationBuilder.InsertData(
                table: "WhoIsServers",
                columns: new[] { "Tld", "Server" },
                values: new object[] { "biz", "whois.biz" });

            migrationBuilder.InsertData(
                table: "WhoIsServers",
                columns: new[] { "Tld", "Server" },
                values: new object[] { "net", "whois.verisign-grs.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhoIsServers");
        }
    }
}
