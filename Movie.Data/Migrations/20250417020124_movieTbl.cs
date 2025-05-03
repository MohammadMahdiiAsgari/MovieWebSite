using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.Data.Migrations
{
    /// <inheritdoc />
    public partial class movieTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieSubGroups_MovieGroups_GroupId",
                table: "MovieSubGroups");

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    SubGrouoId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(350)", maxLength: 350, nullable: false),
                    Language = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Genres = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    ContentRating = table.Column<int>(type: "int", nullable: false),
                    PubliceDate = table.Column<int>(type: "int", nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Visit = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movies_MovieGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "MovieGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movies_MovieSubGroups_SubGrouoId",
                        column: x => x.SubGrouoId,
                        principalTable: "MovieSubGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movieGalleries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movieGalleries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_movieGalleries_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movieGalleries_MovieId",
                table: "movieGalleries",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_GroupId",
                table: "Movies",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_SubGrouoId",
                table: "Movies",
                column: "SubGrouoId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSubGroups_MovieGroups_GroupId",
                table: "MovieSubGroups",
                column: "GroupId",
                principalTable: "MovieGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieSubGroups_MovieGroups_GroupId",
                table: "MovieSubGroups");

            migrationBuilder.DropTable(
                name: "movieGalleries");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSubGroups_MovieGroups_GroupId",
                table: "MovieSubGroups",
                column: "GroupId",
                principalTable: "MovieGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
