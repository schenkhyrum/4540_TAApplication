using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAApplication.Data.Migrations
{
    public partial class StartEndTimeSeparate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOffered",
                table: "Courses");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "TimeOffered",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
