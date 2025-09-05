using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_project.Migrations
{
    /// <inheritdoc />
    public partial class op : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Companyid",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Company_Id",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Companyid",
                table: "Companies",
                column: "Companyid");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Company_Id",
                table: "AspNetUsers",
                column: "Company_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Companies_Company_Id",
                table: "AspNetUsers",
                column: "Company_Id",
                principalTable: "Companies",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Companies_Companyid",
                table: "Companies",
                column: "Companyid",
                principalTable: "Companies",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Companies_Company_Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Companies_Companyid",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Companyid",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Company_Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Companyid",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Company_Id",
                table: "AspNetUsers");
        }
    }
}
