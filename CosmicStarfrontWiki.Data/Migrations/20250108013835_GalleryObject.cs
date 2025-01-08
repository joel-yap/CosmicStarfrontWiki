using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmicStarfrontWiki.Data.Migrations
{
    /// <inheritdoc />
    public partial class GalleryObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Galleries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Images = table.Column<string>(type: "TEXT", nullable: true),
                    ImageStyles = table.Column<string>(type: "TEXT", nullable: true),
                    Captions = table.Column<string>(type: "TEXT", nullable: true),
                    WikiPageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Galleries_WikiPages_WikiPageId",
                        column: x => x.WikiPageId,
                        principalTable: "WikiPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_WikiPageId",
                table: "Galleries",
                column: "WikiPageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Galleries");
        }
    }
}
