using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mail.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "SequenceMail",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Mails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SenderAdress = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    SenderName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    ReceiverAdress = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    ReceiverName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsBodyHTML = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mails");

            migrationBuilder.DropSequence(
                name: "SequenceMail");
        }
    }
}
