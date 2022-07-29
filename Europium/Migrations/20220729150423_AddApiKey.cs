using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Europium.Migrations
{
    public partial class AddApiKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "ApisToMonitor",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "ApisToMonitor");
        }
    }
}
