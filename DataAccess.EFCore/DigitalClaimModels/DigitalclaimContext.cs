﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.EFCore.DigitalClaimModels
{
    public partial class DigitalclaimContext : DbContext
    {
        public DigitalclaimContext()
        {
        }

        public DigitalclaimContext(DbContextOptions<DigitalclaimContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HosApproval> HosApproval { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HosApproval>(entity =>
            {
                entity.HasKey(e => new { e.AccNo, e.VictimNo, e.AppNo });

                entity.Property(e => e.AccNo)
                    .HasColumnName("AccNO")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AcceptBy)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.AcceptDate).HasColumnType("datetime");

                entity.Property(e => e.BranchId)
                    .HasColumnName("BranchID")
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ClaimNo)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreate).HasColumnType("datetime");

                entity.Property(e => e.DocumentNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.GetRecordStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HosId)
                    .HasColumnName("HosID")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InvoiceId)
                    .HasColumnName("InvoiceID")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.InvoiceNo)
                    .HasColumnName("InvoiceNO")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.PayMore)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Pt4id)
                    .HasColumnName("PT4ID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RecFname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecLname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecPrefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecRelate)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.RecSocNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RegDate).HasColumnType("datetime");

                entity.Property(e => e.RegNoClaim).HasColumnName("RegNo_Claim");

                entity.Property(e => e.RegType)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.RevFname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RevLname)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RevPrefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RevRelate)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VictimNoClaim).HasColumnName("VictimNo_Claim");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}