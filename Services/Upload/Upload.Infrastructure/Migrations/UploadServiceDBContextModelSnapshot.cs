﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Upload.Infrastructure.DBContext;

#nullable disable

namespace Upload.Infrastructure.Migrations
{
    [DbContext(typeof(UploadServiceDbContext))]
    partial class UploadServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("SequenceUploadIdentifier")
                .IncrementsBy(10);

            modelBuilder.Entity("Upload.Domain.Entities.UploadIdentifier", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequenceUploadIdentifier");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PseudoText")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.ToTable("UploadIdentifiers");
                });

            modelBuilder.Entity("Upload.Domain.Entities.UploadPicture", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UploadPictures");
                });

            modelBuilder.Entity("Upload.Domain.Entities.UploadVideo", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DurationHour")
                        .HasColumnType("integer");

                    b.Property<int>("DurationMinute")
                        .HasColumnType("integer");

                    b.Property<int>("DurationSecond")
                        .HasColumnType("integer");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<double?>("GpsLatitude")
                        .HasColumnType("double precision");

                    b.Property<double?>("GpsLongitude")
                        .HasColumnType("double precision");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<byte>("Status")
                        .HasColumnType("smallint");

                    b.Property<byte>("VideoType")
                        .HasColumnType("smallint");

                    b.Property<int>("Width")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UploadVideos");
                });

            modelBuilder.Entity("Upload.Domain.Entities.UploadPicture", b =>
                {
                    b.OwnsOne("Upload.Domain.ValueObjects.GoogleGeoCodingAddress", "GoogleGeoCodingAddress", b1 =>
                        {
                            b1.Property<long>("UploadPictureId")
                                .HasColumnType("bigint");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Hnr")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)");

                            b1.Property<double>("Latitude")
                                .HasColumnType("double precision");

                            b1.Property<double>("Longitude")
                                .HasColumnType("double precision");

                            b1.Property<string>("StreetName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Zip")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)");

                            b1.HasKey("UploadPictureId");

                            b1.ToTable("GoogleGeoCodingAddressesPicture", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UploadPictureId");
                        });

                    b.OwnsOne("Upload.Domain.ValueObjects.UploadPictureExifInfo", "UploadPictureExifInfo", b1 =>
                        {
                            b1.Property<long>("UploadPictureId")
                                .HasColumnType("bigint");

                            b1.Property<short?>("Contrast")
                                .HasColumnType("smallint");

                            b1.Property<DateTimeOffset?>("DdlRecorded")
                                .HasColumnType("timestamp with time zone");

                            b1.Property<short?>("ExposureMode")
                                .HasColumnType("smallint");

                            b1.Property<short?>("ExposureProgram")
                                .HasColumnType("smallint");

                            b1.Property<double?>("ExposureTime")
                                .HasMaxLength(50)
                                .HasColumnType("double precision");

                            b1.Property<double?>("FNumber")
                                .HasColumnType("double precision");

                            b1.Property<short?>("FlashMode")
                                .HasColumnType("smallint");

                            b1.Property<double?>("FocalLength")
                                .HasColumnType("double precision");

                            b1.Property<double?>("GpsLatitude")
                                .HasColumnType("double precision");

                            b1.Property<double?>("GpsLongitude")
                                .HasColumnType("double precision");

                            b1.Property<int?>("IsoSensitivity")
                                .HasColumnType("integer");

                            b1.Property<string>("Make")
                                .IsRequired()
                                .HasMaxLength(300)
                                .HasColumnType("character varying(300)");

                            b1.Property<short?>("MeteringMode")
                                .HasColumnType("smallint");

                            b1.Property<string>("Model")
                                .IsRequired()
                                .HasMaxLength(300)
                                .HasColumnType("character varying(300)");

                            b1.Property<short?>("Orientation")
                                .HasColumnType("smallint");

                            b1.Property<short?>("ResolutionUnit")
                                .HasMaxLength(100)
                                .HasColumnType("smallint");

                            b1.Property<double?>("ResolutionX")
                                .HasColumnType("double precision");

                            b1.Property<double?>("ResolutionY")
                                .HasColumnType("double precision");

                            b1.Property<short?>("Saturation")
                                .HasColumnType("smallint");

                            b1.Property<short?>("SensingMode")
                                .HasColumnType("smallint");

                            b1.Property<short?>("Sharpness")
                                .HasColumnType("smallint");

                            b1.Property<double?>("ShutterSpeed")
                                .HasColumnType("double precision");

                            b1.Property<short?>("WhiteBalanceMode")
                                .HasColumnType("smallint");

                            b1.HasKey("UploadPictureId");

                            b1.ToTable("UploadPictureExifInfos", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UploadPictureId");
                        });

                    b.Navigation("GoogleGeoCodingAddress");

                    b.Navigation("UploadPictureExifInfo");
                });

            modelBuilder.Entity("Upload.Domain.Entities.UploadVideo", b =>
                {
                    b.OwnsOne("Upload.Domain.ValueObjects.GoogleGeoCodingAddress", "GoogleGeoCodingAddress", b1 =>
                        {
                            b1.Property<long>("UploadVideoId")
                                .HasColumnType("bigint");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Hnr")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)");

                            b1.Property<double>("Latitude")
                                .HasColumnType("double precision");

                            b1.Property<double>("Longitude")
                                .HasColumnType("double precision");

                            b1.Property<string>("StreetName")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<string>("Zip")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("character varying(10)");

                            b1.HasKey("UploadVideoId");

                            b1.ToTable("GoogleGeoCodingAddressesVideo", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UploadVideoId");
                        });

                    b.Navigation("GoogleGeoCodingAddress");
                });
#pragma warning restore 612, 618
        }
    }
}
