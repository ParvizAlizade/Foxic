using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foxic.Migrations
{
    public partial class UpdateSliderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdvName",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Sliders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvName",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Sliders");
        }
    }
}
