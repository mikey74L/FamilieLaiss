﻿// <auto-generated />
using System;
using Catalog.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catalog.Infrastructure.Migrations
{
    [DbContext(typeof(CatalogServiceDbContext))]
    partial class CatalogServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("SequenceCategory")
                .IncrementsBy(10);

            modelBuilder.HasSequence("SequenceCategoryValue")
                .IncrementsBy(10);

            modelBuilder.HasSequence("SequenceMediaGroup")
                .IncrementsBy(10);

            modelBuilder.HasSequence("SequenceMediaItem")
                .IncrementsBy(10);

            modelBuilder.HasSequence("SequenceMediaItemCategoryValue")
                .IncrementsBy(10);

            modelBuilder.Entity("Catalog.Domain.Aggregates.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequenceCategory");

                    b.Property<byte>("CategoryType")
                        .HasColumnType("smallint");

                    b.Property<DateTimeOffset?>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("NameGerman")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryType", "NameEnglish")
                        .IsUnique();

                    b.HasIndex("CategoryType", "NameGerman")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.CategoryValue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequenceCategoryValue");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("NameGerman")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId", "NameEnglish")
                        .IsUnique();

                    b.HasIndex("CategoryId", "NameGerman")
                        .IsUnique();

                    b.ToTable("CategoryValues");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.MediaGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequenceMediaGroup");

                    b.Property<DateTimeOffset?>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DescriptionEnglish")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("character varying(3000)");

                    b.Property<string>("DescriptionGerman")
                        .IsRequired()
                        .HasMaxLength(3000)
                        .HasColumnType("character varying(3000)");

                    b.Property<DateTimeOffset>("EventDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("NameGerman")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.HasKey("Id");

                    b.HasIndex("NameEnglish")
                        .IsUnique();

                    b.HasIndex("NameGerman")
                        .IsUnique();

                    b.ToTable("MediaGroups");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.MediaItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequenceMediaItem");

                    b.Property<DateTimeOffset?>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DescriptionEnglish")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<string>("DescriptionGerman")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<long>("MediaGroupId")
                        .HasColumnType("bigint");

                    b.Property<byte>("MediaType")
                        .HasColumnType("smallint");

                    b.Property<string>("NameEnglish")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("NameGerman")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<bool>("OnlyFamily")
                        .HasColumnType("boolean");

                    b.Property<long?>("UploadPictureId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UploadVideoId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MediaGroupId");

                    b.HasIndex("MediaType", "NameEnglish")
                        .IsUnique();

                    b.HasIndex("MediaType", "NameGerman")
                        .IsUnique();

                    b.ToTable("MediaItems");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.MediaItemCategoryValue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequenceMediaItemCategoryValue");

                    b.Property<long>("CategoryValueId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("MediaItemId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CategoryValueId");

                    b.HasIndex("MediaItemId");

                    b.ToTable("MediaItemCategoryValues");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.UploadPicture", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsAssigned")
                        .HasColumnType("boolean");

                    b.Property<long?>("MediaItemId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MediaItemId")
                        .IsUnique();

                    b.ToTable("UploadPictures");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.UploadVideo", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsAssigned")
                        .HasColumnType("boolean");

                    b.Property<long?>("MediaItemId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MediaItemId")
                        .IsUnique();

                    b.ToTable("UploadVideos");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.CategoryValue", b =>
                {
                    b.HasOne("Catalog.Domain.Aggregates.Category", "Category")
                        .WithMany("CategoryValues")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.MediaItem", b =>
                {
                    b.HasOne("Catalog.Domain.Aggregates.MediaGroup", "MediaGroup")
                        .WithMany("MediaItems")
                        .HasForeignKey("MediaGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MediaGroup");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.MediaItemCategoryValue", b =>
                {
                    b.HasOne("Catalog.Domain.Aggregates.CategoryValue", "CategoryValue")
                        .WithMany("MediaItemCategoryValues")
                        .HasForeignKey("CategoryValueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Catalog.Domain.Aggregates.MediaItem", "MediaItem")
                        .WithMany("MediaItemCategoryValues")
                        .HasForeignKey("MediaItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryValue");

                    b.Navigation("MediaItem");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.UploadPicture", b =>
                {
                    b.HasOne("Catalog.Domain.Aggregates.MediaItem", "MediaItem")
                        .WithOne("UploadPicture")
                        .HasForeignKey("Catalog.Domain.Entities.UploadPicture", "MediaItemId");

                    b.Navigation("MediaItem");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.UploadVideo", b =>
                {
                    b.HasOne("Catalog.Domain.Aggregates.MediaItem", "MediaItem")
                        .WithOne("UploadVideo")
                        .HasForeignKey("Catalog.Domain.Entities.UploadVideo", "MediaItemId");

                    b.Navigation("MediaItem");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.Category", b =>
                {
                    b.Navigation("CategoryValues");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.CategoryValue", b =>
                {
                    b.Navigation("MediaItemCategoryValues");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.MediaGroup", b =>
                {
                    b.Navigation("MediaItems");
                });

            modelBuilder.Entity("Catalog.Domain.Aggregates.MediaItem", b =>
                {
                    b.Navigation("MediaItemCategoryValues");

                    b.Navigation("UploadPicture");

                    b.Navigation("UploadVideo");
                });
#pragma warning restore 612, 618
        }
    }
}
