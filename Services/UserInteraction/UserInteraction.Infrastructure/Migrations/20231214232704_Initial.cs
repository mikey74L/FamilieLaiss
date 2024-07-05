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
                name: "SequenceRating",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    RatingCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CommentCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    FavoriteCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
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
                    RatingCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    AverageRating = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    CommentCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    FavoriteCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ChangeDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInteractionInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserInteractionInfoID = table.Column<long>(type: "bigint", nullable: false),
                    UserAccountID = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_UserAccounts_UserAccountID",
                        column: x => x.UserAccountID,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_UserInteractionInfos_UserInteractionInfoID",
                        column: x => x.UserInteractionInfoID,
                        principalTable: "UserInteractionInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserInteractionInfoID = table.Column<long>(type: "bigint", nullable: false),
                    UserAccountID = table.Column<string>(type: "text", nullable: true),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_UserAccounts_UserAccountID",
                        column: x => x.UserAccountID,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favorites_UserInteractionInfos_UserInteractionInfoID",
                        column: x => x.UserInteractionInfoID,
                        principalTable: "UserInteractionInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    UserInteractionInfoID = table.Column<long>(type: "bigint", nullable: false),
                    UserAccountID = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_UserAccounts_UserAccountID",
                        column: x => x.UserAccountID,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ratings_UserInteractionInfos_UserInteractionInfoID",
                        column: x => x.UserInteractionInfoID,
                        principalTable: "UserInteractionInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserAccountID",
                table: "Comments",
                column: "UserAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserInteractionInfoID",
                table: "Comments",
                column: "UserInteractionInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserAccountID",
                table: "Favorites",
                column: "UserAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserInteractionInfoID",
                table: "Favorites",
                column: "UserInteractionInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserAccountID",
                table: "Ratings",
                column: "UserAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserInteractionInfoID",
                table: "Ratings",
                column: "UserInteractionInfoID");
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
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserInteractionInfos");

            migrationBuilder.DropSequence(
                name: "SequenceComment");

            migrationBuilder.DropSequence(
                name: "SequenceFavorite");

            migrationBuilder.DropSequence(
                name: "SequenceRating");
        }
    }
}
