using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMC.Migrations
{
    public partial class RecipeNewRelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recipe_ProductID",
                table: "Recipe");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_ProductID",
                table: "Recipe",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recipe_ProductID",
                table: "Recipe");

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_ProductID",
                table: "Recipe",
                column: "ProductID",
                unique: true);
        }
    }
}
