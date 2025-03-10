using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CosmicStarfrontWiki.Data.Migrations
{
    /// <inheritdoc />
    public partial class WikiPage2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("PRAGMA foreign_keys = OFF;", suppressTransaction: true);
            migrationBuilder.DropForeignKey(
                name: "FK_Content_Section_SectionId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_WikiPages_WikiPageId",
                table: "Section");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Section",
                table: "Section");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Content",
                table: "Content");

            migrationBuilder.RenameTable(
                name: "Section",
                newName: "Sections");

            migrationBuilder.RenameTable(
                name: "Content",
                newName: "Contents");

            migrationBuilder.RenameIndex(
                name: "IX_Section_WikiPageId",
                table: "Sections",
                newName: "IX_Sections_WikiPageId");

            migrationBuilder.RenameIndex(
                name: "IX_Content_SectionId",
                table: "Contents",
                newName: "IX_Contents_SectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sections",
                table: "Sections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contents",
                table: "Contents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Sections_SectionId",
                table: "Contents",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_WikiPages_WikiPageId",
                table: "Sections",
                column: "WikiPageId",
                principalTable: "WikiPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.Sql("PRAGMA foreign_keys = ON;", suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Sections_SectionId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Sections_WikiPages_WikiPageId",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sections",
                table: "Sections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contents",
                table: "Contents");

            migrationBuilder.RenameTable(
                name: "Sections",
                newName: "Section");

            migrationBuilder.RenameTable(
                name: "Contents",
                newName: "Content");

            migrationBuilder.RenameIndex(
                name: "IX_Sections_WikiPageId",
                table: "Section",
                newName: "IX_Section_WikiPageId");

            migrationBuilder.RenameIndex(
                name: "IX_Contents_SectionId",
                table: "Content",
                newName: "IX_Content_SectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Section",
                table: "Section",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Content",
                table: "Content",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Section_SectionId",
                table: "Content",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_WikiPages_WikiPageId",
                table: "Section",
                column: "WikiPageId",
                principalTable: "WikiPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
