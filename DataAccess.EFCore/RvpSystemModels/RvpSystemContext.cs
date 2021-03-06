// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Data.Entity.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess.EFCore.RvpSystemModels
{
    public partial class RvpSystemContext : DbContext
    {
        public RvpSystemContext()
        {
        }

        public RvpSystemContext(DbContextOptions<RvpSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EDocDetail> EDocDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EDocDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("eDocDetail");

                entity.Property(e => e.CancelDate).HasColumnType("datetime");

                entity.Property(e => e.CancelIp)
                    .HasColumnName("CancelIP")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Cancelby)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.CreateIp)
                    .HasColumnName("CreateIP")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Createby)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentId)
                    .HasColumnName("DocumentID")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Paths).HasColumnType("text");

                entity.Property(e => e.RefId)
                    .HasColumnName("RefID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StatusDoc)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SystemId)
                    .HasColumnName("SystemID")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.TemplateId)
                    .HasColumnName("TemplateID")
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}