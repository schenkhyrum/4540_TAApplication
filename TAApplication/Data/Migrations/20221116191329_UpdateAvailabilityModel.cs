using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAApplication.Data.Migrations
{
    public partial class UpdateAvailabilityModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Availability",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    TAUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availability", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Availability_AspNetUsers_TAUserId",
                        column: x => x.TAUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Availability_TAUserId",
                table: "Availability",
                column: "TAUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Availability");
        }
    }
}
