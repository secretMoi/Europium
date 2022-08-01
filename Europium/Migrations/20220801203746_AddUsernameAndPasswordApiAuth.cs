using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Europium.Migrations
{
    public partial class AddUsernameAndPasswordApiAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "ApisToMonitor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ApisToMonitor",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "ApisToMonitor");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ApisToMonitor");
        }
    }
}
