using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class review_Guest_relationship_null1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDownvotes_AspNetUsers_GuestId",
                table: "ReviewDownvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDownvotes_Reviews_ReviewId",
                table: "ReviewDownvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewUpvotes_AspNetUsers_GuestId",
                table: "ReviewUpvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewUpvotes_Reviews_ReviewId",
                table: "ReviewUpvotes");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "ReviewUpvotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "ReviewUpvotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "ReviewDownvotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "ReviewDownvotes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDownvotes_AspNetUsers_GuestId",
                table: "ReviewDownvotes",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDownvotes_Reviews_ReviewId",
                table: "ReviewDownvotes",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "ReviewId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewUpvotes_AspNetUsers_GuestId",
                table: "ReviewUpvotes",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewUpvotes_Reviews_ReviewId",
                table: "ReviewUpvotes",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "ReviewId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDownvotes_AspNetUsers_GuestId",
                table: "ReviewDownvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDownvotes_Reviews_ReviewId",
                table: "ReviewDownvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewUpvotes_AspNetUsers_GuestId",
                table: "ReviewUpvotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewUpvotes_Reviews_ReviewId",
                table: "ReviewUpvotes");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "ReviewUpvotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "ReviewUpvotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "ReviewDownvotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuestId",
                table: "ReviewDownvotes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDownvotes_AspNetUsers_GuestId",
                table: "ReviewDownvotes",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDownvotes_Reviews_ReviewId",
                table: "ReviewDownvotes",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewUpvotes_AspNetUsers_GuestId",
                table: "ReviewUpvotes",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewUpvotes_Reviews_ReviewId",
                table: "ReviewUpvotes",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "ReviewId");
        }
    }
}
