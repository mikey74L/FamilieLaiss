﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UserInteraction.Infrastructure.DBContext;

#nullable disable

namespace UserInteraction.Infrastructure.Migrations
{
    [DbContext(typeof(UserInteractionServiceDBContext))]
    partial class UserInteractionServiceDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("SequenceComment")
                .IncrementsBy(10);

            modelBuilder.HasSequence("SequenceFavorite")
                .IncrementsBy(10);

            modelBuilder.HasSequence("SequenceRating")
                .IncrementsBy(10);

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequenceComment");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserAccountID")
                        .HasColumnType("text");

                    b.Property<long>("UserInteractionInfoID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserAccountID");

                    b.HasIndex("UserInteractionInfoID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.Favorite", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequenceFavorite");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserAccountID")
                        .HasColumnType("text");

                    b.Property<long>("UserInteractionInfoID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserAccountID");

                    b.HasIndex("UserInteractionInfoID");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.Rating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseHiLo(b.Property<long>("Id"), "SequenceRating");

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserAccountID")
                        .HasColumnType("text");

                    b.Property<long>("UserInteractionInfoID")
                        .HasColumnType("bigint");

                    b.Property<int>("Value")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("UserAccountID");

                    b.HasIndex("UserInteractionInfoID");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.UserAccount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CommentCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FavoriteCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<int>("RatingCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.UserInteractionInfo", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<double>("AverageRating")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("double precision")
                        .HasDefaultValue(0.0);

                    b.Property<DateTimeOffset?>("ChangeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CommentCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<DateTimeOffset>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FavoriteCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<int>("RatingCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.ToTable("UserInteractionInfos");
                });

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.Comment", b =>
                {
                    b.HasOne("UserInteraction.Domain.Aggregates.UserAccount", "UserAccount")
                        .WithMany("Comments")
                        .HasForeignKey("UserAccountID");

                    b.HasOne("UserInteraction.Domain.Aggregates.UserInteractionInfo", "UserInteractionInfo")
                        .WithMany("Comments")
                        .HasForeignKey("UserInteractionInfoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAccount");

                    b.Navigation("UserInteractionInfo");
                });

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.Favorite", b =>
                {
                    b.HasOne("UserInteraction.Domain.Aggregates.UserAccount", "UserAccount")
                        .WithMany("Favorites")
                        .HasForeignKey("UserAccountID");

                    b.HasOne("UserInteraction.Domain.Aggregates.UserInteractionInfo", "UserInteractionInfo")
                        .WithMany("Favorites")
                        .HasForeignKey("UserInteractionInfoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAccount");

                    b.Navigation("UserInteractionInfo");
                });

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.Rating", b =>
                {
                    b.HasOne("UserInteraction.Domain.Aggregates.UserAccount", "UserAccount")
                        .WithMany("Ratings")
                        .HasForeignKey("UserAccountID");

                    b.HasOne("UserInteraction.Domain.Aggregates.UserInteractionInfo", "UserInteractionInfo")
                        .WithMany("Ratings")
                        .HasForeignKey("UserInteractionInfoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserAccount");

                    b.Navigation("UserInteractionInfo");
                });

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.UserAccount", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Favorites");

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("UserInteraction.Domain.Aggregates.UserInteractionInfo", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Favorites");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
