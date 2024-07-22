using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PictureConvert.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "SequencePictureConvertStatus",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "UploadPictures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Filename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadPictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConvertStatusEntries",
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
                    table.PrimaryKey("PK_ConvertStatusEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvertStatusEntries_UploadPictures_UploadPictureId",
                        column: x => x.UploadPictureId,
                        principalTable: "UploadPictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConvertStatusEntries_UploadPictureId",
                table: "ConvertStatusEntries",
                column: "UploadPictureId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvertStatusEntries");

            migrationBuilder.DropTable(
                name: "UploadPictures");

            migrationBuilder.DropSequence(
                name: "SequencePictureConvertStatus");
        }
    }
}
