using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadScraper.Infrastructure.Migrations
{
    public partial class SearchSettings : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SearchSetting");
        }
    }
}
