using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class review_Guest_Relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDownvotes_AspNetUsers_GuestId",
                table: "ReviewDownvotes",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewUpvotes_AspNetUsers_GuestId",
                table: "ReviewUpvotes",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDownvotes_AspNetUsers_GuestId",
                table: "ReviewDownvotes",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewUpvotes_AspNetUsers_GuestId",
                table: "ReviewUpvotes",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
