using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tiktok.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitVideoAudio_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Tiktok");

            migrationBuilder.CreateTable(
                name: "Audios",
                schema: "Tiktok",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false),
                    Url = table.Column<string>(type: "varchar(255)", nullable: true),
                    TotalUsed = table.Column<long>(type: "bigint", nullable: false),
                    MsDuration = table.Column<long>(type: "bigint", nullable: false),
                    SourceId = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                schema: "Tiktok",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(50)", nullable: false),
                    Title = table.Column<string>(type: "varchar(1024)", nullable: true),
                    Url = table.Column<string>(type: "varchar(512)", nullable: true),
                    TotalLove = table.Column<long>(type: "bigint", nullable: false),
                    OwnerId = table.Column<string>(type: "varchar(50)", nullable: false),
                    ExternalAudioId = table.Column<string>(type: "varchar(50)", nullable: true),
                    MsDuration = table.Column<long>(type: "bigint", nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "varchar(512)", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_Audios_ExternalAudioId",
                        column: x => x.ExternalAudioId,
                        principalSchema: "Tiktok",
                        principalTable: "Audios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Videos_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audios_SourceId",
                schema: "Tiktok",
                table: "Audios",
                column: "SourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_ExternalAudioId",
                schema: "Tiktok",
                table: "Videos",
                column: "ExternalAudioId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_OwnerId",
                schema: "Tiktok",
                table: "Videos",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audios_Videos_SourceId",
                schema: "Tiktok",
                table: "Audios",
                column: "SourceId",
                principalSchema: "Tiktok",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audios_Videos_SourceId",
                schema: "Tiktok",
                table: "Audios");

            migrationBuilder.DropTable(
                name: "Videos",
                schema: "Tiktok");

            migrationBuilder.DropTable(
                name: "Audios",
                schema: "Tiktok");
        }
    }
}
