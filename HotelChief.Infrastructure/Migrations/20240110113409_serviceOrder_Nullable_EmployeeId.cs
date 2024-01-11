using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class serviceOrder_Nullable_EmployeeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId",
                table: "HotelServiceOrders");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId",
                table: "HotelServiceOrders",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId",
                table: "HotelServiceOrders");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_Employees_EmployeeId",
                table: "HotelServiceOrders",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
