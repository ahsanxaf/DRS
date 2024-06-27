﻿// <auto-generated />
using System;
using DRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DRS.Migrations
{
    [DbContext(typeof(DRSdbContext))]
    partial class DRSdbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DRS.Models.Amendment", b =>
                {
                    b.Property<int?>("AmendId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("AmendId"));

                    b.Property<string>("AmendedContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AmendmentType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("BookNo")
                        .HasColumnType("int");

                    b.Property<int?>("DocId")
                        .HasColumnType("int");

                    b.Property<string>("LineNoRange")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PageNoRange")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Remarks")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Sections")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Snumber")
                        .HasColumnType("int")
                        .HasColumnName("SNumber");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.Property<byte[]>("UpdatedTime")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int?>("VersionNo")
                        .HasColumnType("int");

                    b.HasKey("AmendId");

                    b.ToTable("Amendments", "DRS");
                });

            modelBuilder.Entity("DRS.Models.Document", b =>
                {
                    b.Property<int>("DocId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocId"));

                    b.Property<int?>("Acts")
                        .HasColumnType("int");

                    b.Property<string>("AmdtPrinciple")
                        .HasMaxLength(90)
                        .HasColumnType("nvarchar(90)");

                    b.Property<string>("BookNoNew")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("BookNoOld")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("BookPageNo")
                        .HasMaxLength(10)
                        .HasColumnType("nchar(10)")
                        .IsFixedLength();

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CommencementDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DdlLawcategory")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("ddlLawcategory");

                    b.Property<string>("DdlMinistries")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)")
                        .HasColumnName("ddlMinistries");

                    b.Property<string>("DocTitle")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int?>("GazettePageNo")
                        .HasMaxLength(10)
                        .HasColumnType("int")
                        .IsFixedLength();

                    b.Property<string>("GazettePart")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<int?>("GazetteYear")
                        .HasMaxLength(6)
                        .HasColumnType("int");

                    b.Property<string>("LatestUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Ordinance")
                        .HasColumnType("int");

                    b.Property<int?>("PageNo")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PublishingDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Remarks")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RepealedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Rules")
                        .HasColumnType("int");

                    b.Property<string>("SecurityLevel")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Srono")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasColumnName("SRONo");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SubDepartment")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("SubcategoryId")
                        .HasColumnType("int");

                    b.Property<string>("TargetSearchDocument")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("targetSearchDocument");

                    b.Property<string>("TargetSubCategory")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("targetSubCategory");

                    b.Property<string>("TitleUrl")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("titleUrl");

                    b.Property<string>("Volume")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<int?>("Year")
                        .HasColumnType("int");

                    b.HasKey("DocId")
                        .HasName("PK_Document_3214EC079E08D10F");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("Documents", "DRS");
                });

            modelBuilder.Entity("DRS.Models.DocumentCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryDetails")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CategoryName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("CategoryId")
                        .HasName("PK__Document__19093A0B141BA0FE");

                    b.ToTable("DocumentCategories", "DRS");
                });

            modelBuilder.Entity("DRS.Models.DocumentType", b =>
                {
                    b.Property<int>("DocumentTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentTypeID"));

                    b.Property<int?>("Acts")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<int?>("DocId")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<string>("DocTitle")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("Ordinance")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<int?>("Rules")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.Property<int?>("SubcategoryId")
                        .HasMaxLength(255)
                        .HasColumnType("int");

                    b.HasKey("DocumentTypeID");

                    b.ToTable("DocumentTypes", "DRS");
                });

            modelBuilder.Entity("DRS.Models.DocumentVersion", b =>
                {
                    b.Property<string>("DdlLawcategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DdlLawcategoryId")
                        .HasColumnType("int");

                    b.Property<string>("DdlMinistries")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DdlMinistriesId")
                        .HasColumnType("int");

                    b.Property<int>("DocId")
                        .HasColumnType("int");

                    b.Property<string>("DocTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DocumentDocId")
                        .HasColumnType("int");

                    b.Property<string>("DocumentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SubcategoryId")
                        .HasColumnType("int");

                    b.Property<string>("SubcategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VersionId")
                        .HasColumnType("int");

                    b.Property<int?>("VersionNo")
                        .HasColumnType("int");

                    b.Property<string>("Vnumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Vyear")
                        .HasColumnType("int");

                    b.Property<int?>("typeDocumentTypeID")
                        .HasColumnType("int");

                    b.HasIndex("DocumentDocId");

                    b.HasIndex("typeDocumentTypeID");

                    b.ToTable("DocumentVersions");
                });

            modelBuilder.Entity("DRS.Models.LawCategory", b =>
                {
                    b.Property<int>("LawCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LawCategoryId"));

                    b.Property<string>("LawCategoryName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("LawCategoryId");

                    b.ToTable("LawCategory", "DRS");
                });

            modelBuilder.Entity("DRS.Models.Ministry", b =>
                {
                    b.Property<int>("MinistryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MinistryId"));

                    b.Property<string>("MinistryName")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("MinistryId")
                        .HasName("PK_Ministries_MinistryId");

                    b.ToTable("Ministries", "DRS");
                });

            modelBuilder.Entity("DRS.Models.PasswordResetToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("PasswordResetTokens", "DRS");
                });

            modelBuilder.Entity("DRS.Models.Permission", b =>
                {
                    b.Property<int>("PermissionId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermissionId"));

                    b.Property<bool?>("AddPermission")
                        .HasColumnType("bit");

                    b.Property<bool?>("BackupModule")
                        .HasColumnType("bit");

                    b.Property<bool?>("DeletePermission")
                        .HasColumnType("bit");

                    b.Property<bool?>("DocumentCategoryModule")
                        .HasColumnType("bit");

                    b.Property<bool?>("DocumentModule")
                        .HasColumnType("bit");

                    b.Property<bool?>("LoginAuditModule")
                        .HasColumnType("bit");

                    b.Property<int>("RoleId")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<bool?>("SearchModule")
                        .HasColumnType("bit");

                    b.Property<bool?>("UserModule")
                        .HasColumnType("bit");

                    b.Property<bool?>("ViewPermission")
                        .HasColumnType("bit");

                    b.HasKey("PermissionId")
                        .HasName("PK__Permissi__D821329C94C7CBAC");

                    b.HasIndex("RoleId");

                    b.ToTable("Permissions", "DRS");
                });

            modelBuilder.Entity("DRS.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoleId")
                        .HasName("PK__Role__3214EC073909489F");

                    b.ToTable("Role", "DRS");
                });

            modelBuilder.Entity("DRS.Models.Subcategory", b =>
                {
                    b.Property<int>("SubcategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubcategoryId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("SubcategoryDetails")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SubcategoryName")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("SubcategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Subcategories", "DRS");
                });

            modelBuilder.Entity("DRS.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("AccountStatus")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasDefaultValue("Active");

                    b.Property<string>("City")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ContactNo")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateOnly?>("Dob")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("MemberId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PinCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int?>("RoleId")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserAddress")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("UserId")
                        .HasName("PK__User__1788CC4C2C72B458");

                    b.HasIndex("RoleId");

                    b.HasIndex(new[] { "MemberId" }, "UQ__User__0CF04B1977943C62")
                        .IsUnique();

                    b.HasIndex(new[] { "Email" }, "UQ__User__A9D10534E2E11CBD")
                        .IsUnique();

                    b.ToTable("User", "DRS");
                });

            modelBuilder.Entity("DRS.Models.UserLog", b =>
                {
                    b.Property<int>("LogsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogsId"));

                    b.Property<DateTime?>("LogInTime")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("LogOutTime")
                        .HasColumnType("datetime");

                    b.Property<string>("LoginStatus")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserBrowser")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserIp")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("LogsId")
                        .HasName("PK__UserLogs__C920548E931D99D1");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogs", "DRS");
                });

            modelBuilder.Entity("DRS.Models.Version", b =>
                {
                    b.Property<int>("VersionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VersionId"));

                    b.Property<DateTime?>("CommencementDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DocId")
                        .HasColumnType("int");

                    b.Property<string>("DocUrl")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DocumentName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("PublishingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Remarks")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime?>("UploadDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("ValidityDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VdocNature")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("VDocNature");

                    b.Property<int?>("VersionNo")
                        .HasColumnType("int");

                    b.Property<int?>("VgazettePage")
                        .HasMaxLength(50)
                        .HasColumnType("int")
                        .HasColumnName("VGazettePage");

                    b.Property<string>("VgazettePart")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("VGazettePart");

                    b.Property<int?>("VgazetteYear")
                        .HasColumnType("int")
                        .HasColumnName("VGazetteYear");

                    b.Property<string>("Vnumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("VNumber");

                    b.Property<string>("Vstatus")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("VStatus");

                    b.Property<int?>("Vyear")
                        .HasColumnType("int")
                        .HasColumnName("VYear");

                    b.HasKey("VersionId")
                        .HasName("PK_PdfDocum_91794C61479AD201");

                    b.HasIndex("DocId");

                    b.ToTable("Version", "DRS");
                });

            modelBuilder.Entity("DRS.Models.VisitCount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Count")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__VisitCou__3214EC07F24493A4");

                    b.ToTable("VisitCount", "DRS");
                });

            modelBuilder.Entity("DRS.Models.Document", b =>
                {
                    b.HasOne("DRS.Models.DocumentCategory", "Category")
                        .WithMany("Documents")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK__Documents__Categ__6166761E");

                    b.HasOne("DRS.Models.Subcategory", "Subcategory")
                        .WithMany("Documents")
                        .HasForeignKey("SubcategoryId")
                        .HasConstraintName("FK__Documents__Subca__625A9A57");

                    b.Navigation("Category");

                    b.Navigation("Subcategory");
                });

            modelBuilder.Entity("DRS.Models.DocumentVersion", b =>
                {
                    b.HasOne("DRS.Models.Document", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentDocId");

                    b.HasOne("DRS.Models.DocumentType", "type")
                        .WithMany()
                        .HasForeignKey("typeDocumentTypeID");

                    b.Navigation("Document");

                    b.Navigation("type");
                });

            modelBuilder.Entity("DRS.Models.Permission", b =>
                {
                    b.HasOne("DRS.Models.Role", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK__Permissions__RoleId__71D1E811");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DRS.Models.Subcategory", b =>
                {
                    b.HasOne("DRS.Models.DocumentCategory", "Category")
                        .WithMany("Subcategories")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK__Subcatego__Categ__41EDCAC5");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DRS.Models.User", b =>
                {
                    b.HasOne("DRS.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_User_Role");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DRS.Models.UserLog", b =>
                {
                    b.HasOne("DRS.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("user");
                });

            modelBuilder.Entity("DRS.Models.Version", b =>
                {
                    b.HasOne("DRS.Models.Document", "Document")
                        .WithMany("Versions")
                        .HasForeignKey("DocId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK_Version_Document");

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DRS.Models.Document", b =>
                {
                    b.Navigation("Versions");
                });

            modelBuilder.Entity("DRS.Models.DocumentCategory", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Subcategories");
                });

            modelBuilder.Entity("DRS.Models.Role", b =>
                {
                    b.Navigation("Permissions");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("DRS.Models.Subcategory", b =>
                {
                    b.Navigation("Documents");
                });
#pragma warning restore 612, 618
        }
    }
}
