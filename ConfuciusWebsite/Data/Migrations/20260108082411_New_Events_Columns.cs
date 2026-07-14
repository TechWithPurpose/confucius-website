using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConfuciusWebsite.Migrations
{
    /// <inheritdoc />
    public partial class New_Events_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Events",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");
        }
    }
}
