using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadScraper.Infrastructure.Migrations
{
    public partial class MauiRebuildInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SearchSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SearchTerm = table.Column<string>(type: "TEXT", nullable: true),
                    ResultsPerPage = table.Column<int>(type: "INTEGER", nullable: false),
                    StartingPage = table.Column<int>(type: "INTEGER", nullable: false),
                    Pages = table.Column<int>(type: "INTEGER", nullable: false),
                    CountryCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WhiteListTlds = table.Column<string>(type: "TEXT", nullable: true),
                    BlackListTerms = table.Column<string>(type: "TEXT", nullable: true),
                    BingKey = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhoIsServers",
                columns: table => new
                {
                    Tld = table.Column<string>(type: "TEXT", nullable: false),
                    Server = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhoIsServers", x => x.Tld);
                });

            migrationBuilder.InsertData(
                table: "WhoIsServers",
                columns: new[] { "Tld", "Server" },
                values: new object[] { ".biz", "whois.biz" });

            migrationBuilder.InsertData(
                table: "WhoIsServers",
                columns: new[] { "Tld", "Server" },
                values: new object[] { ".com", "whois.verisign-grs.com" });

            migrationBuilder.InsertData(
                table: "WhoIsServers",
                columns: new[] { "Tld", "Server" },
                values: new object[] { ".net", "whois.verisign-grs.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchSetting");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "WhoIsServers");
        }
    }
}
