using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAApplication.Data.Migrations
{
    public partial class AddImageFilename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFilename",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFilename",
                table: "Applications");
        }
    }
}
