using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfuciusWebsite.Migrations
{
    /// <inheritdoc />
    public partial class WHAT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_EventId",
                table: "Images",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Events_EventId",
                table: "Images",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Events_EventId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_EventId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Images");
        }
    }
}
