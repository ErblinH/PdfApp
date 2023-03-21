using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PdfApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixDependencyInConverterOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConvertMarginsId",
                table: "ConverterOptions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConvertMarginsId",
                table: "ConverterOptions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
