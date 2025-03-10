using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserInteraction.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "SequenceComment",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceFavorite",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceMediaItem",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "SequenceRating",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInteractionInfos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RatingCount = table.Column<int>(type: "integer", nullable: false),
                    CommentCount = table.Column<int>(type: "integer", nullable: false),
                    FavoriteCount = table.Column<int>(type: "integer", nullable: false),
                    UserAccountId = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInteractionInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInteractionInfos_UserAccounts_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserInteractionInfoId = table.Column<long>(type: "bigint", nullable: false),
                    MediaItemId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_UserInteractionInfos_UserInteractionInfoId",
                        column: x => x.UserInteractionInfoId,
                        principalTable: "UserInteractionInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserInteractionInfoId = table.Column<long>(type: "bigint", nullable: false),
                    MediaItemId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorites_UserInteractionInfos_UserInteractionInfoId",
                        column: x => x.UserInteractionInfoId,
                        principalTable: "UserInteractionInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserInteractionInfoId = table.Column<long>(type: "bigint", nullable: false),
                    MediaItemId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<int>(type: "integer", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_MediaItems_MediaItemId",
                        column: x => x.MediaItemId,
                        principalTable: "MediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_UserInteractionInfos_UserInteractionInfoId",
                        column: x => x.UserInteractionInfoId,
                        principalTable: "UserInteractionInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MediaItemId",
                table: "Comments",
                column: "MediaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserInteractionInfoId",
                table: "Comments",
                column: "UserInteractionInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_MediaItemId",
                table: "Favorites",
                column: "MediaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserInteractionInfoId",
                table: "Favorites",
                column: "UserInteractionInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MediaItemId",
                table: "Ratings",
                column: "MediaItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserInteractionInfoId",
                table: "Ratings",
                column: "UserInteractionInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInteractionInfos_UserAccountId",
                table: "UserInteractionInfos",
                column: "UserAccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.DropTable(
                name: "UserInteractionInfos");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropSequence(
                name: "SequenceComment");

            migrationBuilder.DropSequence(
                name: "SequenceFavorite");

            migrationBuilder.DropSequence(
                name: "SequenceMediaItem");

            migrationBuilder.DropSequence(
                name: "SequenceRating");
        }
    }
}
