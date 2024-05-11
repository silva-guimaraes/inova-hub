using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aspnet2.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarIdeiaImagens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "image_post_id_fkey",
                table: "image");

            migrationBuilder.DropPrimaryKey(
                name: "image_pkey",
                table: "image");

            migrationBuilder.RenameColumn(
                name: "post_id",
                table: "image",
                newName: "idea_id");

            migrationBuilder.RenameIndex(
                name: "IX_image_post_id",
                table: "image",
                newName: "IX_image_idea_id");

            migrationBuilder.AddPrimaryKey(
                name: "image_pkey",
                table: "image",
                column: "url");

            migrationBuilder.AddForeignKey(
                name: "FK_image_idea_idea_id",
                table: "image",
                column: "idea_id",
                principalTable: "idea",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_image_idea_idea_id",
                table: "image");

            migrationBuilder.DropPrimaryKey(
                name: "image_pkey",
                table: "image");

            migrationBuilder.RenameColumn(
                name: "idea_id",
                table: "image",
                newName: "post_id");

            migrationBuilder.RenameIndex(
                name: "IX_image_idea_id",
                table: "image",
                newName: "IX_image_post_id");

            migrationBuilder.AddPrimaryKey(
                name: "image_pkey",
                table: "image",
                columns: new[] { "url", "post_id" });

            migrationBuilder.AddForeignKey(
                name: "image_post_id_fkey",
                table: "image",
                column: "post_id",
                principalTable: "post",
                principalColumn: "id");
        }
    }
}
