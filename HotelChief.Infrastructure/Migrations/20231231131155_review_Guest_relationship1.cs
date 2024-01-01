using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class review_Guest_relationship1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDownvotes_Reviews_ReviewId1",
                table: "ReviewDownvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewUpvotes_Reviews_ReviewId1",
                table: "ReviewUpvotes");

            migrationBuilder.DropIndex(
                name: "IX_ReviewUpvotes_ReviewId1",
                table: "ReviewUpvotes");

            migrationBuilder.DropIndex(
                name: "IX_ReviewDownvotes_ReviewId1",
                table: "ReviewDownvotes");

            migrationBuilder.DropColumn(
                name: "ReviewId1",
                table: "ReviewUpvotes");

            migrationBuilder.DropColumn(
                name: "ReviewId1",
                table: "ReviewDownvotes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewId1",
                table: "ReviewUpvotes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewId1",
                table: "ReviewDownvotes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReviewUpvotes_ReviewId1",
                table: "ReviewUpvotes",
                column: "ReviewId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDownvotes_ReviewId1",
                table: "ReviewDownvotes",
                column: "ReviewId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDownvotes_Reviews_ReviewId1",
                table: "ReviewDownvotes",
                column: "ReviewId1",
                principalTable: "Reviews",
                principalColumn: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewUpvotes_Reviews_ReviewId1",
                table: "ReviewUpvotes",
                column: "ReviewId1",
                principalTable: "Reviews",
                principalColumn: "ReviewId");
        }
    }
}
