using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class link_Guest_Employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GuestId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "Employees");
        }
    }
}
