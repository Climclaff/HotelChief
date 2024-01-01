using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelChief.Infrastructure.Migrations
{
    public partial class review_Guest_relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewDownvotes",
                columns: table => new
                {
                    DownvoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: false),
                    ReviewId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewDownvotes", x => x.DownvoteId);
                    table.ForeignKey(
                        name: "FK_ReviewDownvotes_AspNetUsers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewDownvotes_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "ReviewId");
                    table.ForeignKey(
                        name: "FK_ReviewDownvotes_Reviews_ReviewId1",
                        column: x => x.ReviewId1,
                        principalTable: "Reviews",
                        principalColumn: "ReviewId");
                });

            migrationBuilder.CreateTable(
                name: "ReviewUpvotes",
                columns: table => new
                {
                    UpvoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: false),
                    ReviewId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewUpvotes", x => x.UpvoteId);
                    table.ForeignKey(
                        name: "FK_ReviewUpvotes_AspNetUsers_GuestId",
                        column: x => x.GuestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewUpvotes_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "ReviewId");
                    table.ForeignKey(
                        name: "FK_ReviewUpvotes_Reviews_ReviewId1",
                        column: x => x.ReviewId1,
                        principalTable: "Reviews",
                        principalColumn: "ReviewId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDownvotes_GuestId",
                table: "ReviewDownvotes",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDownvotes_ReviewId",
                table: "ReviewDownvotes",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewDownvotes_ReviewId1",
                table: "ReviewDownvotes",
                column: "ReviewId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewUpvotes_GuestId",
                table: "ReviewUpvotes",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewUpvotes_ReviewId",
                table: "ReviewUpvotes",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewUpvotes_ReviewId1",
                table: "ReviewUpvotes",
                column: "ReviewId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewDownvotes");

            migrationBuilder.DropTable(
                name: "ReviewUpvotes");
        }
    }
}
