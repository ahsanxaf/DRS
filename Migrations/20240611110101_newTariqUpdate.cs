using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DRS.Migrations
{
    /// <inheritdoc />
    public partial class newTariqUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocumentDocId",
                table: "DocumentVersions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "typeDocumentTypeID",
                table: "DocumentVersions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GazetteYear",
                schema: "DRS",
                table: "Documents",
                type: "int",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GazettePageNo",
                schema: "DRS",
                table: "Documents",
                type: "int",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ordinance",
                schema: "DRS",
                table: "Documents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                schema: "DRS",
                columns: table => new
                {
                    DocumentTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DocId = table.Column<int>(type: "int", maxLength: 255, nullable: true),
                    SubcategoryId = table.Column<int>(type: "int", maxLength: 255, nullable: true),
                    Acts = table.Column<int>(type: "int", maxLength: 255, nullable: true),
                    Rules = table.Column<int>(type: "int", maxLength: 255, nullable: true),
                    Ordinance = table.Column<int>(type: "int", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.DocumentTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVersions_DocumentDocId",
                table: "DocumentVersions",
                column: "DocumentDocId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVersions_typeDocumentTypeID",
                table: "DocumentVersions",
                column: "typeDocumentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_DocumentTypes_typeDocumentTypeID",
                table: "DocumentVersions",
                column: "typeDocumentTypeID",
                principalSchema: "DRS",
                principalTable: "DocumentTypes",
                principalColumn: "DocumentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentDocId",
                table: "DocumentVersions",
                column: "DocumentDocId",
                principalSchema: "DRS",
                principalTable: "Documents",
                principalColumn: "DocId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_DocumentTypes_typeDocumentTypeID",
                table: "DocumentVersions");

            migrationBuilder.DropForeignKey(
                name: "FK_DocumentVersions_Documents_DocumentDocId",
                table: "DocumentVersions");

            migrationBuilder.DropTable(
                name: "DocumentTypes",
                schema: "DRS");

            migrationBuilder.DropIndex(
                name: "IX_DocumentVersions_DocumentDocId",
                table: "DocumentVersions");

            migrationBuilder.DropIndex(
                name: "IX_DocumentVersions_typeDocumentTypeID",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "DocumentDocId",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "typeDocumentTypeID",
                table: "DocumentVersions");

            migrationBuilder.DropColumn(
                name: "Ordinance",
                schema: "DRS",
                table: "Documents");

            migrationBuilder.AlterColumn<string>(
                name: "GazetteYear",
                schema: "DRS",
                table: "Documents",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 6,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GazettePageNo",
                schema: "DRS",
                table: "Documents",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
