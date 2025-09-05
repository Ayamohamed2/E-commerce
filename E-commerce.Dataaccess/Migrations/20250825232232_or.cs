using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce_project.Migrations
{
    /// <inheritdoc />
    public partial class or : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StreetAddress",
                table: "OrderHeader",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "State",
                table: "OrderHeader");

            migrationBuilder.DropColumn(
                name: "StreetAddress",
                table: "OrderHeader");
        }
    }
}
