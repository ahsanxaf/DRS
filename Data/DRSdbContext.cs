using System;
using System.Collections.Generic;
using System.Xml.Linq;
using DRS.Models;
using Microsoft.EntityFrameworkCore;

namespace DRS
{
    public partial class DRSdbContext(DbContextOptions<DRSdbContext> options) : DbContext(options)
    {
        public virtual DbSet<Amendment> Amendments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public virtual DbSet<DocumentCategory> DocumentCategories { get; set; }
        public virtual DbSet<DocumentVersion> DocumentVersions { get; set; }

        public DbSet<Log> Logs { get; set; }

        public virtual DbSet<LawCategory> LawCategories { get; set; }

        public virtual DbSet<Ministry>? Ministries { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Subcategory> Subcategories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserLog> UserLogs { get; set; }

        public virtual DbSet<Models.Version> Versions { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public virtual DbSet<VisitCount> VisitCounts { get; set; }
        //public object Document { get; internal set; }

        //Sir Tariq String: Server=DESKTOP-2EFF6QP;Database=DRS;Trusted_Connection=True;  TrustServerCertificate=true;
        //Ahmed Rameez String: Server=localhost;Database=DRS;User Id=sa;Password=ms$QL123;Encrypt=false;TrustServerCertificate=true;MultipleActiveResultSets=true;
        //"Server=192.168.30.2\\MSSQLSERVER,1433;Database=DRS;User ID=sa;Password=HAM@@@123ham;"
        //"Server=DESKTOP-C09T4KL\\SQLEXPRESS;Database=DRS;Trusted_Connection=True;  TrustServerCertificate=true;"
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=DESKTOP-C09T4KL\\SQLEXPRESS;Database=DRS;Trusted_Connection=True; TrustServerCertificate=true;");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amendment>(entity =>
            {
                entity.HasKey(e => e.AmendId);

                entity.ToTable("Amendments", "DRS");

                entity.Property(e => e.AmendmentType).HasMaxLength(50);
                entity.Property(e => e.SubSection).HasMaxLength(50);
                entity.Property(e => e.Clause).HasMaxLength(50);            
                entity.Property(e => e.SubClause).HasMaxLength(50);
                entity.Property(e => e.Entry).HasMaxLength(150);
                entity.Property(e => e.BookNo).HasMaxLength(150);
                entity.Property(e => e.AmendedContent).HasMaxLength(20);
                entity.Property(e => e.Sections).HasMaxLength(50);
                entity.Property(e => e.Snumber).HasMaxLength(50);
                entity.Property(e => e.UpdatedTime)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.ToTable("PasswordResetTokens", "DRS");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired().HasMaxLength(450);
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(450);
                entity.Property(e => e.ExpiryDate).IsRequired();
                entity.HasIndex(e => e.Token).IsUnique();
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.DocId).HasName("PK_Document_3214EC079E08D10F");

                entity.ToTable("Documents", "DRS");

                entity.Property(e => e.AmdtPrinciple).HasMaxLength(90);
                entity.Property(e => e.BookNoNew).HasMaxLength(10);
                entity.Property(e => e.BookNoOld).HasMaxLength(10);
                entity.Property(e => e.BookPageNo)
                    .HasMaxLength(10)
                    .IsFixedLength();
                //entity.Property(e => e.City).HasMaxLength(50);
                entity.Property(e => e.CommencementDate).HasColumnType("datetime");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.DdlLawcategory)
                    .HasMaxLength(50)
                    .HasColumnName("ddlLawcategory");
                entity.Property(e => e.DdlMinistries)
                    .HasMaxLength(150)
                    .HasColumnName("ddlMinistries");
                entity.Property(e => e.DocTitle).HasMaxLength(250);
                entity.Property(e => e.GazettePageNo)
                    .HasMaxLength(10)
                    .IsFixedLength();
                entity.Property(e => e.GazettePart).HasMaxLength(6);
                entity.Property(e => e.GazetteYear).HasMaxLength(6);
                entity.Property(e => e.PublishingDate).HasColumnType("datetime");
                entity.Property(e => e.Remarks).HasMaxLength(100);
                entity.Property(e => e.RepealedBy).HasMaxLength(50);
                entity.Property(e => e.SecurityLevel).HasMaxLength(50);
                entity.Property(e => e.Srono)
                    .HasMaxLength(10)
                    .HasColumnName("SRONo");
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.SubDepartment).HasMaxLength(50);
                entity.Property(e => e.TargetSearchDocument)
                    .HasMaxLength(50)
                    .HasColumnName("targetSearchDocument");
                entity.Property(e => e.TargetSubCategory)
                    .HasMaxLength(50)
                    .HasColumnName("targetSubCategory");
                entity.Property(e => e.TitleUrl).HasColumnName("titleUrl");
                //entity.Property(e => e.Validity).HasColumnType("datetime");
                entity.Property(e => e.Volume).HasMaxLength(5);
                //entity.Property(e => e.VolumePageNo).HasMaxLength(10);

                entity.HasOne(d => d.Category).WithMany(p => p.Documents)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Documents__Categ__6166761E");

                entity.HasOne(d => d.Subcategory).WithMany(p => p.Documents)
                    .HasForeignKey(d => d.SubcategoryId)
                    .HasConstraintName("FK__Documents__Subca__625A9A57");
            });

            modelBuilder.Entity<DocumentType>(entity =>
            {
                entity.ToTable("DocumentTypes", "DRS");

                entity.HasKey(e => e.DocumentTypeID);

                // Configure other properties as needed
                entity.Property(e => e.DocId).HasMaxLength(255);
                entity.Property(e => e.DocTitle).HasMaxLength(255);
                entity.Property(e => e.SubcategoryId).HasMaxLength(255);
                entity.Property(e => e.Rules).HasMaxLength(255);
                entity.Property(e => e.Ordinance).HasMaxLength(255);
                entity.Property(e => e.Acts).HasMaxLength(255);

            });

            modelBuilder.Entity<DocumentCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Document__19093A0B141BA0FE");

                entity.ToTable("DocumentCategories", "DRS");

                entity.Property(e => e.CategoryDetails).HasMaxLength(50);
                entity.Property(e => e.CategoryName).HasMaxLength(30);
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<LawCategory>(entity =>
            {
                entity.ToTable("LawCategory", "DRS");

                entity.Property(e => e.LawCategoryName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Ministry>(entity =>
            {
                entity.HasKey(e => e.MinistryId).HasName("PK_Ministries_MinistryId");

                entity.ToTable("Ministries", "DRS");

                entity.Property(e => e.MinistryName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__D821329C94C7CBAC");

                entity.ToTable("Permissions", "DRS");

                entity.Property(e => e.PermissionId).HasMaxLength(50);
                entity.Property(e => e.RoleId).HasMaxLength(50);

                entity.HasOne(d => d.Role).WithMany(p => p.Permissions)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Permissions__RoleId__71D1E811");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__Role__3214EC073909489F");

                entity.ToTable("Role", "DRS");

                entity.Property(e => e.RoleId).HasMaxLength(50);
                entity.Property(e => e.CreateAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.RoleName).HasMaxLength(50);
            });


            modelBuilder.Entity<Subcategory>(entity =>
            {
                entity.ToTable("Subcategories", "DRS");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.SubcategoryDetails).HasMaxLength(50);
                entity.Property(e => e.SubcategoryName).HasMaxLength(30);

                entity.HasOne(d => d.Category).WithMany(p => p.Subcategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Subcatego__Categ__41EDCAC5");
            });



            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C2C72B458");

                entity.ToTable("User", "DRS");

                entity.HasIndex(e => e.MemberId, "UQ__User__0CF04B1977943C62").IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__A9D10534E2E11CBD").IsUnique();

                entity.Property(e => e.AccountStatus)
                    .HasMaxLength(20)
                    .HasDefaultValue("Active");
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.ContactNo).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.MemberId).HasMaxLength(20);
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Password).HasMaxLength(255);
                entity.Property(e => e.PinCode).HasMaxLength(10);
                entity.Property(e => e.RoleId).HasMaxLength(50);
                entity.Property(e => e.State).HasMaxLength(100);
                entity.Property(e => e.UserAddress).HasMaxLength(255);

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.HasKey(e => e.LogsId).HasName("PK__UserLogs__C920548E931D99D1");

                entity.ToTable("UserLogs", "DRS");

                entity.Property(e => e.LogInTime).HasColumnType("datetime");
                entity.Property(e => e.LogOutTime).HasColumnType("datetime");
                entity.Property(e => e.LoginStatus).HasMaxLength(50);
                entity.Property(e => e.UserBrowser).HasMaxLength(450);
                entity.Property(e => e.UserIp).HasMaxLength(50);
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.LogId).HasName("PK__Logs__C920548E931D99D1");

                entity.ToTable("Logs", "DRS");

                entity.Property(e => e.Action).HasMaxLength(255);
                entity.Property(e => e.Timestamp).HasColumnType("datetime");
                entity.Property(e => e.UserId).HasMaxLength(50);
                entity.Property(e => e.DocumentId).HasMaxLength(450);
            });


            modelBuilder.Entity<Models.Version>(entity =>
            {
                entity.HasKey(e => e.VersionId).HasName("PK_PdfDocum_91794C61479AD201");

                entity.ToTable("Version", "DRS");

                entity.Property(e => e.DocId); // Add the property for DocId

                // Configure the properties as needed
                entity.Property(e => e.DocUrl).HasMaxLength(100);
                entity.Property(e => e.DocumentName).HasMaxLength(100);
                entity.Property(e => e.Remarks).HasMaxLength(150);
                entity.Property(e => e.UploadDate).HasColumnType("datetime");
                entity.Property(e => e.VdocNature).HasMaxLength(20).HasColumnName("VDocNature");
                entity.Property(e => e.VgazettePage).HasMaxLength(50).HasColumnName("VGazettePage");
                entity.Property(e => e.VgazettePart).HasMaxLength(50).HasColumnName("VGazettePart");
                entity.Property(e => e.VgazetteYear).HasColumnName("VGazetteYear");
                entity.Property(e => e.Vnumber).HasMaxLength(50).HasColumnName("VNumber");
                entity.Property(e => e.Vstatus).HasMaxLength(20).HasColumnName("VStatus");
                entity.Property(e => e.Vyear).HasColumnName("VYear");

                // Configure the foreign key relationship
                entity.HasOne(d => d.Document)
                    .WithMany(p => p.Versions)
                    .HasForeignKey(d => d.DocId)
                    .OnDelete(DeleteBehavior.Cascade) // Specify the desired delete behavior
                    .HasConstraintName("FK_Version_Document"); // Optionally specify the constraint name
            });


            modelBuilder.Entity<VisitCount>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__VisitCou__3214EC07F24493A4");

                entity.ToTable("VisitCount", "DRS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}