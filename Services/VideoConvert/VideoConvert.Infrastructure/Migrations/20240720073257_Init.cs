using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoConvert.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "SequenceVideoConvertStatus",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "UploadVideos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConvertStatusEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    VideoType = table.Column<byte>(type: "smallint", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    ConvertHour = table.Column<int>(type: "integer", nullable: true),
                    ConvertMinute = table.Column<int>(type: "integer", nullable: true),
                    ConvertSecond = table.Column<int>(type: "integer", nullable: true),
                    ConvertRestHour = table.Column<int>(type: "integer", nullable: true),
                    ConvertRestMinute = table.Column<int>(type: "integer", nullable: true),
                    ConvertRestSecond = table.Column<int>(type: "integer", nullable: true),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Progress = table.Column<int>(type: "integer", nullable: true),
                    StartDateMp4 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateMp4 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateMp4360 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateMp4360 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateMp4480 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateMp4480 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateMp4720 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateMp4720 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateMp41080 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateMp41080 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateMp42160 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateMp42160 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateHls = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateHls = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateThumbnail = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateThumbnail = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateMediaInfo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateMediaInfo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateVtt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateVtt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateCopyConverted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateCopyConverted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateDeleteTemp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateDeleteTemp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateDeleteOriginal = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateDeleteOriginal = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateConvertPicture = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateConvertPicture = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UploadVideoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvertStatusEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvertStatusEntries_UploadVideos_UploadVideoId",
                        column: x => x.UploadVideoId,
                        principalTable: "UploadVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConvertStatusEntries_UploadVideoId",
                table: "ConvertStatusEntries",
                column: "UploadVideoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvertStatusEntries");

            migrationBuilder.DropTable(
                name: "UploadVideos");

            migrationBuilder.DropSequence(
                name: "SequenceVideoConvertStatus");
        }
    }
}
