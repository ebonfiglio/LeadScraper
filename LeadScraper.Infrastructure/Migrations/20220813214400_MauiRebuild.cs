using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeadScraper.Infrastructure.Migrations
{
    public partial class MauiRebuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WhoIsServers",
                table: "WhoIsServers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WhoIsServers");

            migrationBuilder.AlterColumn<string>(
                name: "Tld",
                table: "WhoIsServers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhoIsServers",
                table: "WhoIsServers",
                column: "Tld");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WhoIsServers",
                table: "WhoIsServers");

            migrationBuilder.DeleteData(
                table: "WhoIsServers",
                keyColumn: "Tld",
                keyValue: ".biz");

            migrationBuilder.DeleteData(
                table: "WhoIsServers",
                keyColumn: "Tld",
                keyValue: ".com");

            migrationBuilder.DeleteData(
                table: "WhoIsServers",
                keyColumn: "Tld",
                keyValue: ".net");

            migrationBuilder.AlterColumn<string>(
                name: "Tld",
                table: "WhoIsServers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WhoIsServers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WhoIsServers",
                table: "WhoIsServers",
                column: "Id");

            migrationBuilder.InsertData(
                table: "WhoIsServers",
                columns: new[] { "Id", "Server", "Tld" },
                values: new object[] { 1, "whois.verisign-grs.com", ".com" });

            migrationBuilder.InsertData(
                table: "WhoIsServers",
                columns: new[] { "Id", "Server", "Tld" },
                values: new object[] { 2, "whois.biz", ".biz" });

            migrationBuilder.InsertData(
                table: "WhoIsServers",
                columns: new[] { "Id", "Server", "Tld" },
                values: new object[] { 3, "whois.verisign-grs.com", ".net" });
        }
    }
}
