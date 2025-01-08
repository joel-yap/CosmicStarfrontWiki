using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmicStarfrontWiki.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImagePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "WikiPages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageStyle",
                table: "WikiPages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Contents",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageStyle",
                table: "Contents",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "WikiPages");

            migrationBuilder.DropColumn(
                name: "ImageStyle",
                table: "WikiPages");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "ImageStyle",
                table: "Contents");
        }
    }
}
