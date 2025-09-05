using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_project.Migrations
{
    /// <inheritdoc />
    public partial class opp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Companies_Companyid",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Companyid",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Companyid",
                table: "Companies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Companyid",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Companyid",
                table: "Companies",
                column: "Companyid");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Companies_Companyid",
                table: "Companies",
                column: "Companyid",
                principalTable: "Companies",
                principalColumn: "id");
        }
    }
}
