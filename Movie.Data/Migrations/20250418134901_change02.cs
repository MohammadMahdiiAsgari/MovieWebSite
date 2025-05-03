using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.Data.Migrations
{
    /// <inheritdoc />
    public partial class change02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_MovieSubGroups_SubGrouoId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_SubGrouoId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "SubGrouoId",
                table: "Movies");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_SubGroupId",
                table: "Movies",
                column: "SubGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_MovieSubGroups_SubGroupId",
                table: "Movies",
                column: "SubGroupId",
                principalTable: "MovieSubGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_MovieSubGroups_SubGroupId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_SubGroupId",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "SubGrouoId",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_SubGrouoId",
                table: "Movies",
                column: "SubGrouoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_MovieSubGroups_SubGrouoId",
                table: "Movies",
                column: "SubGrouoId",
                principalTable: "MovieSubGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
