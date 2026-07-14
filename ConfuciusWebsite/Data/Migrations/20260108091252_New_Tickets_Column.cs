using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfuciusWebsite.Migrations
{
    /// <inheritdoc />
    public partial class New_Tickets_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tickets",
                table: "Events",
                type: "nvarchar(30)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tickets",
                table: "Events");
        }
    }
}
