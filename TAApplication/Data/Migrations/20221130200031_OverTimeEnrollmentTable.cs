using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAApplication.Data.Migrations
{
    public partial class OverTimeEnrollmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OverTimeEnrollments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTaken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EnrollmentCount = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverTimeEnrollments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OverTimeEnrollments_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverTimeEnrollments_CourseID",
                table: "OverTimeEnrollments",
                column: "CourseID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverTimeEnrollments");
        }
    }
}
