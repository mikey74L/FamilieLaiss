using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLBackEnd.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SettingsSeekTimeChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoTimeSeekForward",
                table: "UserSettings");

            migrationBuilder.RenameColumn(
                name: "VideoTimeSeekBackward",
                table: "UserSettings",
                newName: "VideoTimeSeekForwardRewind");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoTimeSeekForwardRewind",
                table: "UserSettings",
                newName: "VideoTimeSeekBackward");

            migrationBuilder.AddColumn<int>(
                name: "VideoTimeSeekForward",
                table: "UserSettings",
                type: "integer",
                nullable: false,
                defaultValue: 30);
        }
    }
}
