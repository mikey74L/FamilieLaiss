using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "CategoryValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CategoryID = table.Column<long>(type: "bigint", nullable: false),
                    NameGerman = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryValues_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MediaGroupID = table.Column<long>(type: "bigint", nullable: false),
                    MediaType = table.Column<byte>(type: "smallint", nullable: false),
                    NameGerman = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NameEnglish = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DescriptionGerman = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    DescriptionEnglish = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    OnlyFamily = table.Column<bool>(type: "boolean", nullable: false),
                    UploadPictureID = table.Column<long>(type: "bigint", nullable: true),
                    UploadVideoID = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItems_MediaGroups_MediaGroupID",
                        column: x => x.MediaGroupID,
                        principalTable: "MediaGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaItemCategoryValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    MediaItemID = table.Column<long>(type: "bigint", nullable: false),
                    CategoryValueID = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItemCategoryValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItemCategoryValues_CategoryValues_CategoryValueID",
                        column: x => x.CategoryValueID,
                        principalTable: "CategoryValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaItemCategoryValues_MediaItems_MediaItemID",
                        column: x => x.MediaItemID,
                        principalTable: "MediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UploadPictures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Filename = table.Column<string>(type: "text", nullable: false),
                    MediaItemID = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadPictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadPictures_MediaItems_MediaItemID",
                        column: x => x.MediaItemID,
                        principalTable: "MediaItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UploadVideos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Filename = table.Column<string>(type: "text", nullable: false),
                    MediaItemID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadVideos_MediaItems_MediaItemID",
                        column: x => x.MediaItemID,
                        principalTable: "MediaItems",
                        principalColumn: "Id");
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
                name: "IX_CategoryValues_CategoryID_NameEnglish",
                table: "CategoryValues",
                columns: new[] { "CategoryID", "NameEnglish" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryValues_CategoryID_NameGerman",
                table: "CategoryValues",
                columns: new[] { "CategoryID", "NameGerman" },
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
                name: "IX_MediaItemCategoryValues_CategoryValueID",
                table: "MediaItemCategoryValues",
                column: "CategoryValueID");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItemCategoryValues_MediaItemID",
                table: "MediaItemCategoryValues",
                column: "MediaItemID");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_MediaGroupID",
                table: "MediaItems",
                column: "MediaGroupID");

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
                name: "IX_UploadPictures_MediaItemID",
                table: "UploadPictures",
                column: "MediaItemID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UploadVideos_MediaItemID",
                table: "UploadVideos",
                column: "MediaItemID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaItemCategoryValues");

            migrationBuilder.DropTable(
                name: "UploadPictures");

            migrationBuilder.DropTable(
                name: "UploadVideos");

            migrationBuilder.DropTable(
                name: "CategoryValues");

            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "MediaGroups");

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
        }
    }
}
