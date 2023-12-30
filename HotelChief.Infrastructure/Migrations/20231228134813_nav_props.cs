using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class nav_props : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "HotelServiceOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HotelServiceServiceId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelServiceOrders_EmployeeId1",
                table: "HotelServiceOrders",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_HotelServiceOrders_HotelServiceServiceId",
                table: "HotelServiceOrders",
                column: "HotelServiceServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId1",
                table: "HotelServiceOrders",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_HotelServices_HotelServiceServiceId",
                table: "HotelServiceOrders",
                column: "HotelServiceServiceId",
                principalTable: "HotelServices",
                principalColumn: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId1",
                table: "HotelServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_HotelServices_HotelServiceServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.DropIndex(
                name: "IX_HotelServiceOrders_EmployeeId1",
                table: "HotelServiceOrders");

            migrationBuilder.DropIndex(
                name: "IX_HotelServiceOrders_HotelServiceServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "HotelServiceOrders");

            migrationBuilder.DropColumn(
                name: "HotelServiceServiceId",
                table: "HotelServiceOrders");
        }
    }
}
