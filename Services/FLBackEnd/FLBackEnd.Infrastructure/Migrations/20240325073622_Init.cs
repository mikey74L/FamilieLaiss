using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FLBackEnd.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "SequenceBlogEntry",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceCategory",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceCategoryValue",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceMediaGroup",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceMediaItem",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceMediaItemCategoryValue",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequencePictureConvertStatus",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceUploadIdentifier",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceVideoConvertStatus",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "BlogEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    HeaderGerman = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    HeaderEnglish = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TextGerman = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false),
                    TextEnglish = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CategoryType = table.Column<byte>(type: "smallint", nullable: false),
                    NameGerman = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    NameGerman = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    DescriptionGerman = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    DescriptionEnglish = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: false),
                    EventDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UploadIdentifiers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PseudoText = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadIdentifiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false),
                    NameGerman = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryValues_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MediaGroupId = table.Column<long>(type: "bigint", nullable: false),
                    MediaType = table.Column<byte>(type: "smallint", nullable: false),
                    NameGerman = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DescriptionGerman = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    OnlyFamily = table.Column<bool>(type: "boolean", nullable: false),
                    UploadPictureId = table.Column<long>(type: "bigint", nullable: true),
                    UploadVideoId = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItems_MediaGroups_MediaGroupId",
                        column: x => x.MediaGroupId,
                        principalTable: "MediaGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaItemCategoryValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MediaItemId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryValueId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItemCategoryValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItemCategoryValues_CategoryValues_CategoryValueId",
                        column: x => x.CategoryValueId,
                        principalTable: "CategoryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaItemCategoryValues_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    MediaItemId = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadPictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadPictures_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id");
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
                    MediaItemId = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadVideos_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id");
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
                name: "PictureConvertStatusEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    StartDateInfo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateInfo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateExif = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateExif = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StartDateConvert = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishDateConvert = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UploadPictureId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureConvertStatusEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PictureConvertStatusEntries_UploadPictures_UploadPictureId",
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

            migrationBuilder.CreateTable(
                name: "VideoConvertStatusEntries",
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
                    table.PrimaryKey("PK_VideoConvertStatusEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VideoConvertStatusEntries_UploadVideos_UploadVideoId",
                        column: x => x.UploadVideoId,
                        principalTable: "UploadVideos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryType_NameEnglish",
                table: "Category",
                columns: new[] { "CategoryType", "NameEnglish" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryType_NameGerman",
                table: "Category",
                columns: new[] { "CategoryType", "NameGerman" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryValues_CategoryId_NameEnglish",
                table: "CategoryValues",
                columns: new[] { "CategoryId", "NameEnglish" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryValues_CategoryId_NameGerman",
                table: "CategoryValues",
                columns: new[] { "CategoryId", "NameGerman" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaGroups_NameEnglish",
                table: "MediaGroups",
                column: "NameEnglish",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaGroups_NameGerman",
                table: "MediaGroups",
                column: "NameGerman",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaItemCategoryValues_CategoryValueId",
                table: "MediaItemCategoryValues",
                column: "CategoryValueId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItemCategoryValues_MediaItemId",
                table: "MediaItemCategoryValues",
                column: "MediaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_MediaGroupId",
                table: "MediaItems",
                column: "MediaGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_MediaType_NameEnglish",
                table: "MediaItems",
                columns: new[] { "MediaType", "NameEnglish" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_MediaType_NameGerman",
                table: "MediaItems",
                columns: new[] { "MediaType", "NameGerman" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PictureConvertStatusEntries_UploadPictureId",
                table: "PictureConvertStatusEntries",
                column: "UploadPictureId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UploadPictures_MediaItemId",
                table: "UploadPictures",
                column: "MediaItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UploadVideos_MediaItemId",
                table: "UploadVideos",
                column: "MediaItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VideoConvertStatusEntries_UploadVideoId",
                table: "VideoConvertStatusEntries",
                column: "UploadVideoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogEntries");

            migrationBuilder.DropTable(
                name: "GoogleGeoCodingAddressesPicture");

            migrationBuilder.DropTable(
                name: "GoogleGeoCodingAddressesVideo");

            migrationBuilder.DropTable(
                name: "MediaItemCategoryValues");

            migrationBuilder.DropTable(
                name: "PictureConvertStatusEntries");

            migrationBuilder.DropTable(
                name: "UploadIdentifiers");

            migrationBuilder.DropTable(
                name: "UploadPictureExifInfos");

            migrationBuilder.DropTable(
                name: "VideoConvertStatusEntries");

            migrationBuilder.DropTable(
                name: "CategoryValues");

            migrationBuilder.DropTable(
                name: "UploadPictures");

            migrationBuilder.DropTable(
                name: "UploadVideos");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.DropTable(
                name: "MediaGroups");

            migrationBuilder.DropSequence(
                name: "SequenceBlogEntry");

            migrationBuilder.DropSequence(
                name: "SequenceCategory");

            migrationBuilder.DropSequence(
                name: "SequenceCategoryValue");

            migrationBuilder.DropSequence(
                name: "SequenceMediaGroup");

            migrationBuilder.DropSequence(
                name: "SequenceMediaItem");

            migrationBuilder.DropSequence(
                name: "SequenceMediaItemCategoryValue");

            migrationBuilder.DropSequence(
                name: "SequencePictureConvertStatus");

            migrationBuilder.DropSequence(
                name: "SequenceUploadIdentifier");

            migrationBuilder.DropSequence(
                name: "SequenceVideoConvertStatus");
        }
    }
}
