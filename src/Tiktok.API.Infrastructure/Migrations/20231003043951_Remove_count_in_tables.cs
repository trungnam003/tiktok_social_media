using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tiktok.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Remove_count_in_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalLove",
                schema: "Tiktok",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "FollowerCount",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FollowingCount",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TotalUsed",
                schema: "Tiktok",
                table: "Audios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TotalLove",
                schema: "Tiktok",
                table: "Videos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FollowerCount",
                schema: "Identity",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FollowingCount",
                schema: "Identity",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TotalUsed",
                schema: "Tiktok",
                table: "Audios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
