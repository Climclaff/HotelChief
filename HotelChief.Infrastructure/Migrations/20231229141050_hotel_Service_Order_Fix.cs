using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class hotel_Service_Order_Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelServiceOrders_HotelServices_HotelServiceServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.DropIndex(
                name: "IX_HotelServiceOrders_HotelServiceServiceId",
                table: "HotelServiceOrders");

            migrationBuilder.DropColumn(
                name: "HotelServiceServiceId",
                table: "HotelServiceOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelServiceServiceId",
                table: "HotelServiceOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelServiceOrders_HotelServiceServiceId",
                table: "HotelServiceOrders",
                column: "HotelServiceServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelServiceOrders_HotelServices_HotelServiceServiceId",
                table: "HotelServiceOrders",
                column: "HotelServiceServiceId",
                principalTable: "HotelServices",
                principalColumn: "ServiceId");
        }
    }
}
