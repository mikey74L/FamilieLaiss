using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Upload.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "SequenceUploadIdentifier",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "UploadIdentifiers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PseudoText = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadIdentifiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadPictures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadPictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadVideos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    VideoType = table.Column<byte>(type: "smallint", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    DurationHour = table.Column<int>(type: "integer", nullable: false),
                    DurationMinute = table.Column<int>(type: "integer", nullable: false),
                    DurationSecond = table.Column<int>(type: "integer", nullable: false),
                    GpsLongitude = table.Column<double>(type: "double precision", nullable: true),
                    GpsLatitude = table.Column<double>(type: "double precision", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoogleGeoCodingAddressesPicture",
                columns: table => new
                {
                    UploadPictureId = table.Column<long>(type: "bigint", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    StreetName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Hnr = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Zip = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleGeoCodingAddressesPicture", x => x.UploadPictureId);
                    table.ForeignKey(
                        name: "FK_GoogleGeoCodingAddressesPicture_UploadPictures_UploadPictur~",
                        column: x => x.UploadPictureId,
                        principalTable: "UploadPictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UploadPictureExifInfos",
                columns: table => new
                {
                    UploadPictureId = table.Column<long>(type: "bigint", nullable: false),
                    Make = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Model = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    ResolutionX = table.Column<double>(type: "double precision", nullable: true),
                    ResolutionY = table.Column<double>(type: "double precision", nullable: true),
                    ResolutionUnit = table.Column<short>(type: "smallint", maxLength: 100, nullable: true),
                    Orientation = table.Column<short>(type: "smallint", nullable: true),
                    DdlRecorded = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExposureTime = table.Column<double>(type: "double precision", maxLength: 50, nullable: true),
                    ExposureProgram = table.Column<short>(type: "smallint", nullable: true),
                    ExposureMode = table.Column<short>(type: "smallint", nullable: true),
                    FNumber = table.Column<double>(type: "double precision", nullable: true),
                    IsoSensitivity = table.Column<int>(type: "integer", nullable: true),
                    ShutterSpeed = table.Column<double>(type: "double precision", nullable: true),
                    MeteringMode = table.Column<short>(type: "smallint", nullable: true),
                    FlashMode = table.Column<short>(type: "smallint", nullable: true),
                    FocalLength = table.Column<double>(type: "double precision", nullable: true),
                    SensingMode = table.Column<short>(type: "smallint", nullable: true),
                    WhiteBalanceMode = table.Column<short>(type: "smallint", nullable: true),
                    Sharpness = table.Column<short>(type: "smallint", nullable: true),
                    GpsLongitude = table.Column<double>(type: "double precision", nullable: true),
                    GpsLatitude = table.Column<double>(type: "double precision", nullable: true),
                    Contrast = table.Column<short>(type: "smallint", nullable: true),
                    Saturation = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadPictureExifInfos", x => x.UploadPictureId);
                    table.ForeignKey(
                        name: "FK_UploadPictureExifInfos_UploadPictures_UploadPictureId",
                        column: x => x.UploadPictureId,
                        principalTable: "UploadPictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoogleGeoCodingAddressesVideo",
                columns: table => new
                {
                    UploadVideoId = table.Column<long>(type: "bigint", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    StreetName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Hnr = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Zip = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleGeoCodingAddressesVideo", x => x.UploadVideoId);
                    table.ForeignKey(
                        name: "FK_GoogleGeoCodingAddressesVideo_UploadVideos_UploadVideoId",
                        column: x => x.UploadVideoId,
                        principalTable: "UploadVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoogleGeoCodingAddressesPicture");

            migrationBuilder.DropTable(
                name: "GoogleGeoCodingAddressesVideo");

            migrationBuilder.DropTable(
                name: "UploadIdentifiers");

            migrationBuilder.DropTable(
                name: "UploadPictureExifInfos");

            migrationBuilder.DropTable(
                name: "UploadVideos");

            migrationBuilder.DropTable(
                name: "UploadPictures");

            migrationBuilder.DropSequence(
                name: "SequenceUploadIdentifier");
        }
    }
}
