using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.Data.Migrations
{
    /// <inheritdoc />
    public partial class TVShowsTbl02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tVShowsSubGroups_tVShowsGroups_GroupId",
                table: "tVShowsSubGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tVShowsSubGroups",
                table: "tVShowsSubGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tVShowsGroups",
                table: "tVShowsGroups");

            migrationBuilder.RenameTable(
                name: "tVShowsSubGroups",
                newName: "TVShowsSubGroups");

            migrationBuilder.RenameTable(
                name: "tVShowsGroups",
                newName: "TVShowsGroups");

            migrationBuilder.RenameIndex(
                name: "IX_tVShowsSubGroups_GroupId",
                table: "TVShowsSubGroups",
                newName: "IX_TVShowsSubGroups_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TVShowsSubGroups",
                table: "TVShowsSubGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TVShowsGroups",
                table: "TVShowsGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TVShowsSubGroups_TVShowsGroups_GroupId",
                table: "TVShowsSubGroups",
                column: "GroupId",
                principalTable: "TVShowsGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TVShowsSubGroups_TVShowsGroups_GroupId",
                table: "TVShowsSubGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TVShowsSubGroups",
                table: "TVShowsSubGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TVShowsGroups",
                table: "TVShowsGroups");

            migrationBuilder.RenameTable(
                name: "TVShowsSubGroups",
                newName: "tVShowsSubGroups");

            migrationBuilder.RenameTable(
                name: "TVShowsGroups",
                newName: "tVShowsGroups");

            migrationBuilder.RenameIndex(
                name: "IX_TVShowsSubGroups_GroupId",
                table: "tVShowsSubGroups",
                newName: "IX_tVShowsSubGroups_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tVShowsSubGroups",
                table: "tVShowsSubGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tVShowsGroups",
                table: "tVShowsGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tVShowsSubGroups_tVShowsGroups_GroupId",
                table: "tVShowsSubGroups",
                column: "GroupId",
                principalTable: "tVShowsGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
