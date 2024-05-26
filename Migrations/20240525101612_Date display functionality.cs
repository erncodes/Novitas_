using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Novitas_Blog.Migrations
{
    public partial class Datedisplayfunctionality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeString",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeString",
                table: "Articles");
        }
    }
}
