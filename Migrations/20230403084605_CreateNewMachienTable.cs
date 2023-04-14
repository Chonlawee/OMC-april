using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMC.Migrations
{
    public partial class CreateNewMachienTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Machine",
                columns: table => new
                {
                    MachineID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MachineStaus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machine", x => x.MachineID);
                });

            migrationBuilder.CreateTable(
                name: "machineStocks",
                columns: table => new
                {
                    MachineStockID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineID = table.Column<int>(type: "int", nullable: false),
                    MilkStock = table.Column<int>(type: "int", nullable: false),
                    WaterStock = table.Column<int>(type: "int", nullable: false),
                    SyrubStock = table.Column<int>(type: "int", nullable: false),
                    CoffeeStock = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "getdate()"),
                    Deleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_machineStocks", x => x.MachineStockID);
                    table.ForeignKey(
                        name: "FK_machineStocks_Machine_MachineID",
                        column: x => x.MachineID,
                        principalTable: "Machine",
                        principalColumn: "MachineID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_machineStocks_MachineID",
                table: "machineStocks",
                column: "MachineID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "machineStocks");

            migrationBuilder.DropTable(
                name: "Machine");
        }
    }
}
