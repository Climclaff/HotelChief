using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class architecture_Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId",
                table: "HotelServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_Guest_GuestId",
                table: "HotelServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_HotelServices_ServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Guest_GuestId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Rooms_RoomNumber",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_RoomNumber",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_HotelServiceOrders_ServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.AlterColumn<int>(
                name: "RoomNumber",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomNumber1",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HotelServiceId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RoomNumber1",
                table: "Reservations",
                column: "RoomNumber1");

            migrationBuilder.CreateIndex(
                name: "IX_HotelServiceOrders_HotelServiceId",
                table: "HotelServiceOrders",
                column: "HotelServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId",
                table: "HotelServiceOrders",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_Guest_GuestId",
                table: "HotelServiceOrders",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_HotelServices_HotelServiceId",
                table: "HotelServiceOrders",
                column: "HotelServiceId",
                principalTable: "HotelServices",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Guest_GuestId",
                table: "Reservations",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Rooms_RoomNumber1",
                table: "Reservations",
                column: "RoomNumber1",
                principalTable: "Rooms",
                principalColumn: "RoomNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId",
                table: "HotelServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_Guest_GuestId",
                table: "HotelServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_HotelServices_HotelServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Guest_GuestId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Rooms_RoomNumber1",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_RoomNumber1",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_HotelServiceOrders_HotelServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.DropColumn(
                name: "RoomNumber1",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "HotelServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.AlterColumn<int>(
                name: "RoomNumber",
                table: "Reservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "Reservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RoomNumber",
                table: "Reservations",
                column: "RoomNumber");

            migrationBuilder.CreateIndex(
                name: "IX_HotelServiceOrders_ServiceId",
                table: "HotelServiceOrders",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId",
                table: "HotelServiceOrders",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_Guest_GuestId",
                table: "HotelServiceOrders",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_HotelServices_ServiceId",
                table: "HotelServiceOrders",
                column: "ServiceId",
                principalTable: "HotelServices",
                principalColumn: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Guest_GuestId",
                table: "Reservations",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Rooms_RoomNumber",
                table: "Reservations",
                column: "RoomNumber",
                principalTable: "Rooms",
                principalColumn: "RoomNumber");
        }
    }
}
