using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Europium.Migrations
{
    public partial class AddMonitorURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiUrls",
                columns: table => new
                {
                    ApiUrlId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiToMonitorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiUrls", x => x.ApiUrlId);
                    table.ForeignKey(
                        name: "FK_ApiUrls_ApisToMonitor_ApiToMonitorId",
                        column: x => x.ApiToMonitorId,
                        principalTable: "ApisToMonitor",
                        principalColumn: "ApiToMonitorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiUrls_ApiToMonitorId",
                table: "ApiUrls",
                column: "ApiToMonitorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiUrls");
        }
    }
}
