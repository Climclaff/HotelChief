using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class serviceOrder_State : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "HotelServiceOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "HotelServiceOrders");
        }
    }
}
