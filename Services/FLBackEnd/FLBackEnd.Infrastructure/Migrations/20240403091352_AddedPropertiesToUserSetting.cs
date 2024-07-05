using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLBackEnd.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertiesToUserSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowZoomingWithMouseWheel",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowButtonForward",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowButtonRewind",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowMirrorButton",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowPlayRateMenu",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowQualityMenu",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowTooltipForCurrentPlaytime",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowTooltipForPlaytimeOnMouseCursor",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowZoomInfo",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowZoomMenu",
                table: "UserSettings",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowZoomingWithMouseWheel",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ShowButtonForward",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ShowButtonRewind",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ShowMirrorButton",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ShowPlayRateMenu",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ShowQualityMenu",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ShowTooltipForCurrentPlaytime",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ShowTooltipForPlaytimeOnMouseCursor",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ShowZoomInfo",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "ShowZoomMenu",
                table: "UserSettings");
        }
    }
}
