﻿// <auto-generated />
using System;
using EFSamurai.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EFSamurai.DataAccess.Migrations
{
    [DbContext(typeof(SamuraiDbContext))]
    [Migration("20231204084736_BattleAndSamuraiBattleAdded")]
    partial class BattleAndSamuraiBattleAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EFSamurai.Domain.Entities.Battle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsBrutal")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Battle");
                });

            modelBuilder.Entity("EFSamurai.Domain.Entities.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("SamuraiId")
                        .HasColumnType("int");

                    b.Property<int?>("Style")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SamuraiId");

                    b.ToTable("Quote");
                });

            modelBuilder.Entity("EFSamurai.Domain.Entities.Samurai", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("HairStyle")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Samurai");
                });

            modelBuilder.Entity("EFSamurai.Domain.Entities.SamuraiBattle", b =>
                {
                    b.Property<int>("SamuraiId")
                        .HasColumnType("int");

                    b.Property<int>("BattleId")
                        .HasColumnType("int");

                    b.HasKey("SamuraiId", "BattleId");

                    b.HasIndex("BattleId");

                    b.ToTable("samuraiBattles");
                });

            modelBuilder.Entity("EFSamurai.Domain.Entities.SecretIdentity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RealName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SamuraiID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SamuraiID")
                        .IsUnique();

                    b.ToTable("SecretIdentity");
                });

            modelBuilder.Entity("EFSamurai.Domain.Entities.Quote", b =>
                {
                    b.HasOne("EFSamurai.Domain.Entities.Samurai", "Samurai")
                        .WithMany("Quotes")
                        .HasForeignKey("SamuraiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Samurai");
                });

            modelBuilder.Entity("EFSamurai.Domain.Entities.SamuraiBattle", b =>
                {
                    b.HasOne("EFSamurai.Domain.Entities.Battle", "Battle")
                        .WithMany("SamuraiBattles")
                        .HasForeignKey("BattleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EFSamurai.Domain.Entities.Samurai", "Samurai")
                        .WithMany("SamuraiBattles")
                        .HasForeignKey("SamuraiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Battle");

                    b.Navigation("Samurai");
                });

            modelBuilder.Entity("EFSamurai.Domain.Entities.SecretIdentity", b =>
                {
                    b.HasOne("EFSamurai.Domain.Entities.Samurai", "Samurai")
                        .WithOne("SecretIdentity")
                        .HasForeignKey("EFSamurai.Domain.Entities.SecretIdentity", "SamuraiID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Samurai");
                });

            modelBuilder.Entity("EFSamurai.Domain.Entities.Battle", b =>
                {
                    b.Navigation("SamuraiBattles");
                });

            modelBuilder.Entity("EFSamurai.Domain.Entities.Samurai", b =>
                {
                    b.Navigation("Quotes");

                    b.Navigation("SamuraiBattles");

                    b.Navigation("SecretIdentity");
                });
#pragma warning restore 612, 618
        }
    }
}
