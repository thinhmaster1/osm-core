using Microsoft.EntityFrameworkCore.Migrations;

namespace OSM.Data.EF.Migrations
{
    public partial class updatecarttouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cart",
                table: "AppUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cart",
                table: "AppUsers");
        }
    }
}
