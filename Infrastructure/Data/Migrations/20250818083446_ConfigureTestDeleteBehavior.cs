using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureTestDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Tests_TestId",
                table: "TestResults");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Tests_TestId",
                table: "TestResults",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Tests_TestId",
                table: "TestResults");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Tests_TestId",
                table: "TestResults",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id");
        }
    }
}
