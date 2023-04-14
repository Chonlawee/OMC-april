using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMC.Migrations
{
    public partial class CreateNewProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductTypeNumber",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductTypeNumber",
                table: "Product");
        }
    }
}
