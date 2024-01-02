using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class room_Cleaning1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomCleaning_Employees_EmployeeId",
                table: "RoomCleaning");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomCleaning_Rooms_RoomNumber",
                table: "RoomCleaning");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomCleaning_Employees_EmployeeId",
                table: "RoomCleaning",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomCleaning_Rooms_RoomNumber",
                table: "RoomCleaning",
                column: "RoomNumber",
                principalTable: "Rooms",
                principalColumn: "RoomNumber",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomCleaning_Employees_EmployeeId",
                table: "RoomCleaning");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomCleaning_Rooms_RoomNumber",
                table: "RoomCleaning");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomCleaning_Employees_EmployeeId",
                table: "RoomCleaning",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomCleaning_Rooms_RoomNumber",
                table: "RoomCleaning",
                column: "RoomNumber",
                principalTable: "Rooms",
                principalColumn: "RoomNumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
