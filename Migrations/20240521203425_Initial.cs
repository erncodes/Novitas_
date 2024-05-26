using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Novitas_Blog.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Blog_Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Featured_Image_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Published_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is_Visible = table.Column<bool>(type: "bit", nullable: false),
                    Is_Featured = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogArticleBlogTag",
                columns: table => new
                {
                    ArticlesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Blog_TagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogArticleBlogTag", x => new { x.ArticlesId, x.Blog_TagsId });
                    table.ForeignKey(
                        name: "FK_BlogArticleBlogTag_Articles_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogArticleBlogTag_BlogTags_Blog_TagsId",
                        column: x => x.Blog_TagsId,
                        principalTable: "BlogTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripton = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlogArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Articles_BlogArticleId",
                        column: x => x.BlogArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogArticleBlogTag_Blog_TagsId",
                table: "BlogArticleBlogTag",
                column: "Blog_TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogArticleId",
                table: "Comments",
                column: "BlogArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogArticleBlogTag");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "BlogTags");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
