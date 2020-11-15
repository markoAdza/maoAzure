using Microsoft.EntityFrameworkCore.Migrations;

namespace web.Migrations
{
    public partial class RatingsWithClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Rating",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ClientId",
                table: "Rating",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_AspNetUsers_ClientId",
                table: "Rating",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_AspNetUsers_ClientId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_ClientId",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Rating");
        }
    }
}
