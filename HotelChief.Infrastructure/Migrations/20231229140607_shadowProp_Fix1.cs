using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class shadowProp_Fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId1",
                table: "HotelServiceOrders");

            migrationBuilder.DropIndex(
                name: "IX_HotelServiceOrders_EmployeeId1",
                table: "HotelServiceOrders");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "HotelServiceOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "HotelServiceOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelServiceOrders_EmployeeId1",
                table: "HotelServiceOrders",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId1",
                table: "HotelServiceOrders",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }
    }
}
