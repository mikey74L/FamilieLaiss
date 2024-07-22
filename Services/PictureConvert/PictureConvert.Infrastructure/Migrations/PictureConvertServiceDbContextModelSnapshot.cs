﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PictureConvert.Infrastructure.DBContext;

#nullable disable

namespace PictureConvert.Infrastructure.Migrations
{
    [DbContext(typeof(PictureConvertServiceDbContext))]
    partial class PictureConvertServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("SequencePictureConvertStatus")
                .IncrementsBy(10);

            modelBuilder.Entity("PictureConvert.Domain.Entities.PictureConvertStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequencePictureConvertStatus");

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<DateTimeOffset?>("FinishDateConvert")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("FinishDateExif")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("FinishDateInfo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("StartDateConvert")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("StartDateExif")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("StartDateInfo")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.Property<long>("UploadPictureId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UploadPictureId")
                        .IsUnique();

                    b.ToTable("ConvertStatusEntries");
                });

            modelBuilder.Entity("PictureConvert.Domain.Entities.UploadPicture", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("UploadPictures");
                });

            modelBuilder.Entity("PictureConvert.Domain.Entities.PictureConvertStatus", b =>
                {
                    b.HasOne("PictureConvert.Domain.Entities.UploadPicture", "UploadPicture")
                        .WithOne("Status")
                        .HasForeignKey("PictureConvert.Domain.Entities.PictureConvertStatus", "UploadPictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UploadPicture");
                });

            modelBuilder.Entity("PictureConvert.Domain.Entities.UploadPicture", b =>
                {
                    b.Navigation("Status");
                });
#pragma warning restore 612, 618
        }
    }
}
