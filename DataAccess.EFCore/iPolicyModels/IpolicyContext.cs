﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataAccess.EFCore.iPolicyModels
{
    public partial class IpolicyContext : DbContext
    {
        public IpolicyContext()
        {
        }

        public IpolicyContext(DbContextOptions<IpolicyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DirectPolicyKyc> DirectPolicyKyc { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Thai_CI_AS");

            modelBuilder.Entity<DirectPolicyKyc>(entity =>
            {
                entity.HasKey(e => e.Kycno);

                entity.ToTable("DirectPolicy_KYC");

                entity.Property(e => e.Kycno)
                    .ValueGeneratedNever()
                    .HasColumnName("KYCNo");

                entity.Property(e => e.CurrentAddrIsHomeAddr)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.CurrentBuildingName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Current_BuildingName");

                entity.Property(e => e.CurrentCityId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Current_CityID");

                entity.Property(e => e.CurrentHmo)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Current_HMO");

                entity.Property(e => e.CurrentHouseNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Current_HouseNo");

                entity.Property(e => e.CurrentProvinceId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Current_ProvinceID");

                entity.Property(e => e.CurrentRoad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Current_Road");

                entity.Property(e => e.CurrentSoi)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Current_Soi");

                entity.Property(e => e.CurrentTumbolId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Current_TumbolID");

                entity.Property(e => e.CurrentZipcode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Current_Zipcode");

                entity.Property(e => e.DateofBirth).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fname)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.HashData)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.HomeBuildingName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Home_BuildingName");

                entity.Property(e => e.HomeCityId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Home_CityID");

                entity.Property(e => e.HomeHmo)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Home_HMO");

                entity.Property(e => e.HomeHouseNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Home_HouseNo");

                entity.Property(e => e.HomeProvinceId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Home_ProvinceID");

                entity.Property(e => e.HomeRoad)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Home_Road");

                entity.Property(e => e.HomeSoi)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Home_Soi");

                entity.Property(e => e.HomeTumbolId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("Home_TumbolID");

                entity.Property(e => e.HomeZipcode)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("Home_Zipcode");

                entity.Property(e => e.IdcardLaserCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IDCardLaserCode");

                entity.Property(e => e.IdcardNo)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("IDCardNo");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdateIp)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LastUpdateIP");

                entity.Property(e => e.LineId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LineID");

                entity.Property(e => e.Lname)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Prefix)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}