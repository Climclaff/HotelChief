using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class serviceOrder_History_Table_Remove_Constraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrderHistory_Employees_EmployeeId",
                table: "HotelServiceOrderHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrderHistory_HotelServices_HotelServiceId",
                table: "HotelServiceOrderHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrderHistory_Employees_EmployeeId",
                table: "HotelServiceOrderHistory",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrderHistory_HotelServices_HotelServiceId",
                table: "HotelServiceOrderHistory",
                column: "HotelServiceId",
                principalTable: "HotelServices",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
