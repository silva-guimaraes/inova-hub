using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace aspnet2.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "comment_id_seq",
                maxValue: 2147483647L);

            migrationBuilder.CreateSequence(
                name: "idea_id_seq",
                maxValue: 2147483647L);

            migrationBuilder.CreateSequence(
                name: "post_id_seq",
                maxValue: 2147483647L);

            migrationBuilder.CreateSequence(
                name: "user_id_seq",
                maxValue: 2147483647L);

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    email = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    password = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "idea",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    id_user = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("idea_pk", x => x.id);
                    table.ForeignKey(
                        name: "user_fk",
                        column: x => x.id_user,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "favorite",
                columns: table => new
                {
                    id_user = table.Column<int>(type: "integer", nullable: false),
                    id_idea = table.Column<int>(type: "integer", nullable: false),
                    favorite_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE", comment: "vai automaticamente gerar data atual quando linha for inserida")
                },
                constraints: table =>
                {
                    table.PrimaryKey("favorite_pk", x => new { x.id_user, x.id_idea });
                    table.ForeignKey(
                        name: "idea_fk",
                        column: x => x.id_idea,
                        principalTable: "idea",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "user_fk",
                        column: x => x.id_user,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "post",
                columns: table => new
                {
                    idea_id = table.Column<int>(type: "integer", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    creation_date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("post_pkey", x => new { x.idea_id, x.id });
                    table.UniqueConstraint("AK_post_id", x => x.id);
                    table.ForeignKey(
                        name: "post_idea_id_fkey",
                        column: x => x.idea_id,
                        principalTable: "idea",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "upvote",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    id_idea = table.Column<int>(type: "integer", nullable: false),
                    upvote_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "CURRENT_DATE", comment: "vai automaticamente gerar data atual quando linha for inserida")
                },
                constraints: table =>
                {
                    table.PrimaryKey("upvote_pk", x => new { x.user_id, x.id_idea });
                    table.ForeignKey(
                        name: "idea_fk",
                        column: x => x.id_idea,
                        principalTable: "idea",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "upvote_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    post_id = table.Column<int>(type: "integer", nullable: false),
                    text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("comment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "comment_post_id_fkey",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "comment_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    post_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("image_pkey", x => new { x.url, x.post_id });
                    table.ForeignKey(
                        name: "image_post_id_fkey",
                        column: x => x.post_id,
                        principalTable: "post",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_comment_post_id",
                table: "comment",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_user_id",
                table: "comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_id_idea",
                table: "favorite",
                column: "id_idea");

            migrationBuilder.CreateIndex(
                name: "IX_idea_id_user",
                table: "idea",
                column: "id_user");

            migrationBuilder.CreateIndex(
                name: "IX_image_post_id",
                table: "image",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "post_id_key",
                table: "post",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_upvote_id_idea",
                table: "upvote",
                column: "id_idea");

            migrationBuilder.CreateIndex(
                name: "user_name_key",
                table: "user",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "favorite");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropTable(
                name: "upvote");

            migrationBuilder.DropTable(
                name: "post");

            migrationBuilder.DropTable(
                name: "idea");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropSequence(
                name: "comment_id_seq");

            migrationBuilder.DropSequence(
                name: "idea_id_seq");

            migrationBuilder.DropSequence(
                name: "post_id_seq");

            migrationBuilder.DropSequence(
                name: "user_id_seq");
        }
    }
}
