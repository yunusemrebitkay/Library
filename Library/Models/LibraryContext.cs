﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Library.Models
{
    public partial class LibraryContext : DbContext
    {
        public LibraryContext()
        {
        }

        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblBooks> TblBooks { get; set; }
        public virtual DbSet<TblNotifications> TblNotifications { get; set; }
        public virtual DbSet<TblReturnBooks> TblReturnBooks { get; set; }
        public virtual DbSet<TblSettings> TblSettings { get; set; }
        public virtual DbSet<TblTakenBooks> TblTakenBooks { get; set; }
        public virtual DbSet<TblTeacherApprovals> TblTeacherApprovals { get; set; }
        public virtual DbSet<TblTryLogin> TblTryLogin { get; set; }
        public virtual DbSet<TblUserTypes> TblUserTypes { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-KS9GM7G;Initial Catalog=Library;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblBooks>(entity =>
            {
                entity.HasKey(e => e.BookId);

                entity.ToTable("tblBooks");

                entity.Property(e => e.BookId).HasColumnName("BookID");

                entity.Property(e => e.Author)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<TblNotifications>(entity =>
            {
                entity.HasKey(e => e.NotificationsId);

                entity.ToTable("tblNotifications");

                entity.Property(e => e.NotificationsId).HasColumnName("NotificationsID");

                entity.Property(e => e.DisplayMessage)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Time).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblReturnBooks>(entity =>
            {
                entity.HasKey(e => e.ReturnBooksId);

                entity.ToTable("tblReturnBooks");

                entity.Property(e => e.BookName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TblSettings>(entity =>
            {
                entity.HasKey(e => e.SettingsId)
                    .HasName("PK_tblSettings_1");

                entity.ToTable("tblSettings");

                entity.Property(e => e.SettingsId).HasColumnName("SettingsID");

                entity.Property(e => e.SettingName)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<TblTakenBooks>(entity =>
            {
                entity.HasKey(e => e.TakenBooksId);

                entity.ToTable("tblTakenBooks");

                entity.Property(e => e.BookName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ReturnDate).HasColumnType("datetime");

                entity.Property(e => e.TakenDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblTeacherApprovals>(entity =>
            {
                entity.HasKey(e => e.TeacherApprovalsId);

                entity.ToTable("tblTeacherApprovals");

                entity.Property(e => e.DateOfRegistration).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<TblTryLogin>(entity =>
            {
                entity.HasKey(e => new { e.TriedIp, e.DateOfTry });

                entity.ToTable("tblTryLogin");

                entity.Property(e => e.TriedIp)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.DateOfTry).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblUserTypes>(entity =>
            {
                entity.HasKey(e => e.UserTypeId);

                entity.ToTable("tblUserTypes");

                entity.Property(e => e.UserTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("UserTypeID");

                entity.Property(e => e.UserTypeNames)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("tblUsers");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Firstname)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Lastname)
                    .IsRequired()
                    .HasMaxLength(64);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SaltOfPassword)
                    .IsRequired()
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}