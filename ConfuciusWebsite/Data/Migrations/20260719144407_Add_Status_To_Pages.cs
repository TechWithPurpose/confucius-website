using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfuciusWebsite.Migrations
{
    /// <inheritdoc />
    public partial class Add_Status_To_Pages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Draft");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pages");
        }
    }
}
