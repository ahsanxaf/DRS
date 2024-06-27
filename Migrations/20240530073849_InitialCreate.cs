using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DRS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DRS");

            migrationBuilder.CreateTable(
                name: "Amendments",
                schema: "DRS",
                columns: table => new
                {
                    AmendId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocId = table.Column<int>(type: "int", nullable: true),
                    VersionNo = table.Column<int>(type: "int", nullable: true),
                    Sections = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SNumber = table.Column<int>(type: "int", nullable: true),
                    AmendmentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BookNo = table.Column<int>(type: "int", nullable: true),
                    PageNoRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LineNoRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AmendedContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedTime = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amendments", x => x.AmendId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentCategories",
                schema: "DRS",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CategoryDetails = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Document__19093A0B141BA0FE", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentVersions",
                columns: table => new
                {
                    DocId = table.Column<int>(type: "int", nullable: false),
                    DocTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubcategoryId = table.Column<int>(type: "int", nullable: true),
                    VersionNo = table.Column<int>(type: "int", nullable: true),
                    VersionId = table.Column<int>(type: "int", nullable: true),
                    SubcategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vyear = table.Column<int>(type: "int", nullable: true),
                    Vnumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DdlMinistries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DdlLawcategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DdlLawcategoryId = table.Column<int>(type: "int", nullable: false),
                    DdlMinistriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "LawCategory",
                schema: "DRS",
                columns: table => new
                {
                    LawCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LawCategoryName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LawCategory", x => x.LawCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Ministries",
                schema: "DRS",
                columns: table => new
                {
                    MinistryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinistryName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ministries_MinistryId", x => x.MinistryId);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                schema: "DRS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "DRS",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3214EC073909489F", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "VisitCount",
                schema: "DRS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VisitCou__3214EC07F24493A4", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                schema: "DRS",
                columns: table => new
                {
                    SubcategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubcategoryName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SubcategoryDetails = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.SubcategoryId);
                    table.ForeignKey(
                        name: "FK__Subcatego__Categ__41EDCAC5",
                        column: x => x.CategoryId,
                        principalSchema: "DRS",
                        principalTable: "DocumentCategories",
                        principalColumn: "CategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "DRS",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    UserModule = table.Column<bool>(type: "bit", nullable: true),
                    DocumentCategoryModule = table.Column<bool>(type: "bit", nullable: true),
                    LoginAuditModule = table.Column<bool>(type: "bit", nullable: true),
                    DocumentModule = table.Column<bool>(type: "bit", nullable: true),
                    SearchModule = table.Column<bool>(type: "bit", nullable: true),
                    BackupModule = table.Column<bool>(type: "bit", nullable: true),
                    AddPermission = table.Column<bool>(type: "bit", nullable: true),
                    DeletePermission = table.Column<bool>(type: "bit", nullable: true),
                    ViewPermission = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permissi__D821329C94C7CBAC", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK__Permissions__RoleId__71D1E811",
                        column: x => x.RoleId,
                        principalSchema: "DRS",
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "DRS",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Dob = table.Column<DateOnly>(type: "date", nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    State = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PinCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UserAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__1788CC4C2C72B458", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.RoleId,
                        principalSchema: "DRS",
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "DRS",
                columns: table => new
                {
                    DocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    SubcategoryId = table.Column<int>(type: "int", nullable: true),
                    DocTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ddlMinistries = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ddlLawcategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Volume = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    PageNo = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    RepealedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AmdtPrinciple = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: true),
                    BookNoNew = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BookNoOld = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BookPageNo = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    GazetteYear = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    GazettePart = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    GazettePageNo = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    SRONo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SubDepartment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PublishingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CommencementDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SecurityLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    targetSubCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    targetSearchDocument = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    titleUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LatestUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rules = table.Column<int>(type: "int", nullable: true),
                    Acts = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document_3214EC079E08D10F", x => x.DocId);
                    table.ForeignKey(
                        name: "FK__Documents__Categ__6166761E",
                        column: x => x.CategoryId,
                        principalSchema: "DRS",
                        principalTable: "DocumentCategories",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK__Documents__Subca__625A9A57",
                        column: x => x.SubcategoryId,
                        principalSchema: "DRS",
                        principalTable: "Subcategories",
                        principalColumn: "SubcategoryId");
                });

            migrationBuilder.CreateTable(
                name: "UserLogs",
                schema: "DRS",
                columns: table => new
                {
                    LogsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    LoginStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserBrowser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LogInTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    LogOutTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserLogs__C920548E931D99D1", x => x.LogsId);
                    table.ForeignKey(
                        name: "FK_UserLogs_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "DRS",
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Version",
                schema: "DRS",
                columns: table => new
                {
                    VersionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocId = table.Column<int>(type: "int", nullable: true),
                    VersionNo = table.Column<int>(type: "int", nullable: true),
                    VYear = table.Column<int>(type: "int", nullable: true),
                    VNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VGazetteYear = table.Column<int>(type: "int", nullable: true),
                    VGazettePart = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VGazettePage = table.Column<int>(type: "int", maxLength: 50, nullable: true),
                    DocumentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CommencementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VDocNature = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfDocum_91794C61479AD201", x => x.VersionId);
                    table.ForeignKey(
                        name: "FK_Version_Document",
                        column: x => x.DocId,
                        principalSchema: "DRS",
                        principalTable: "Documents",
                        principalColumn: "DocId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CategoryId",
                schema: "DRS",
                table: "Documents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SubcategoryId",
                schema: "DRS",
                table: "Documents",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_Token",
                schema: "DRS",
                table: "PasswordResetTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                schema: "DRS",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryId",
                schema: "DRS",
                table: "Subcategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                schema: "DRS",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__User__0CF04B1977943C62",
                schema: "DRS",
                table: "User",
                column: "MemberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__User__A9D10534E2E11CBD",
                schema: "DRS",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_UserId",
                schema: "DRS",
                table: "UserLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Version_DocId",
                schema: "DRS",
                table: "Version",
                column: "DocId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amendments",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "DocumentVersions");

            migrationBuilder.DropTable(
                name: "LawCategory",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "Ministries",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "UserLogs",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "Version",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "VisitCount",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "User",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "Subcategories",
                schema: "DRS");

            migrationBuilder.DropTable(
                name: "DocumentCategories",
                schema: "DRS");
        }
    }
}
