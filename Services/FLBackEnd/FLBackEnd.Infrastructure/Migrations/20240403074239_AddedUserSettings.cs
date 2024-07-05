using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLBackEnd.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QuestionKeepUploadWhenDelete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DefaultKeepUploadWhenDelete = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    VideoAutoPlay = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    VideoVolume = table.Column<int>(type: "integer", nullable: false, defaultValue: 100),
                    VideoLoop = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    VideoAutoPlayOtherVideos = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    VideoTimeToPlayNextVideo = table.Column<int>(type: "integer", nullable: false, defaultValue: 5),
                    VideoTimeSeekForward = table.Column<int>(type: "integer", nullable: false, defaultValue: 30),
                    VideoTimeSeekBackward = table.Column<int>(type: "integer", nullable: false, defaultValue: 10),
                    GalleryCloseEsc = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    GalleryCloseDimmer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    GalleryMouseWheelChangeSlide = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    GalleryShowThumbnails = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    GalleryShowFullScreen = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    GalleryTransitionDuration = table.Column<int>(type: "integer", nullable: false, defaultValue: 600),
                    GalleryTransitionType = table.Column<string>(type: "text", nullable: false, defaultValue: "lg-fade"),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
