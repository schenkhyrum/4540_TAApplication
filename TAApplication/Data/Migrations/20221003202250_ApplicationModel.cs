using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAApplication.Data.Migrations
{
    public partial class ApplicationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DegreePursuing = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPA = table.Column<double>(type: "float", nullable: false),
                    AvailableHours = table.Column<int>(type: "int", nullable: false),
                    AvailableWeekBefore = table.Column<bool>(type: "bit", nullable: false),
                    SemestersCompleted = table.Column<int>(type: "int", nullable: false),
                    PersonalStatement = table.Column<string>(type: "nvarchar(max)", maxLength: 50000, nullable: true),
                    TransferSchool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedInURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeFilename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TAUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Applications_AspNetUsers_TAUserId",
                        column: x => x.TAUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_TAUserId",
                table: "Applications",
                column: "TAUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
