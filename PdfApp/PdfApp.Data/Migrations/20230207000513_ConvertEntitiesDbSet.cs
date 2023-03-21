using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PdfApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConvertEntitiesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConverterMargins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Top = table.Column<int>(type: "int", nullable: false),
                    Right = table.Column<int>(type: "int", nullable: false),
                    Bottom = table.Column<int>(type: "int", nullable: false),
                    Left = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConverterMargins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConverterOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageColorMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageOrientation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PagePaperSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConvertMarginsId = table.Column<int>(type: "int", nullable: false),
                    ConverterMarginsId = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConverterOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConverterOptions_ConverterMargins_ConverterMarginsId",
                        column: x => x.ConverterMarginsId,
                        principalTable: "ConverterMargins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConvertJob",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HtmlInput = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConverterOptionsId = table.Column<int>(type: "int", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConvertJob", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConvertJob_ConverterOptions_ConverterOptionsId",
                        column: x => x.ConverterOptionsId,
                        principalTable: "ConverterOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConverterOptions_ConverterMarginsId",
                table: "ConverterOptions",
                column: "ConverterMarginsId");

            migrationBuilder.CreateIndex(
                name: "IX_ConvertJob_ConverterOptionsId",
                table: "ConvertJob",
                column: "ConverterOptionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConvertJob");

            migrationBuilder.DropTable(
                name: "ConverterOptions");

            migrationBuilder.DropTable(
                name: "ConverterMargins");
        }
    }
}
