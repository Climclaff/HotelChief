using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class review_Upvotes_namefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpVotes",
                table: "Reviews",
                newName: "Upvotes");

            migrationBuilder.RenameColumn(
                name: "DownVotes",
                table: "Reviews",
                newName: "Downvotes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Upvotes",
                table: "Reviews",
                newName: "UpVotes");

            migrationBuilder.RenameColumn(
                name: "Downvotes",
                table: "Reviews",
                newName: "DownVotes");
        }
    }
}
