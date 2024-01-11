using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class guest_Employee_Relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Employees_GuestId",
                table: "Employees",
                column: "GuestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_GuestId",
                table: "Employees",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_GuestId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_GuestId",
                table: "Employees");
        }
    }
}
