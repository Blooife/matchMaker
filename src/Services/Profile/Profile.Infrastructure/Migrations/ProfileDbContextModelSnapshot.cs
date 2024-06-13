﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Profile.Infrastructure.Contexts;

#nullable disable

namespace Profile.Infrastructure.Migrations
{
    [DbContext(typeof(ProfileDbContext))]
    partial class ProfileDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InterestUserProfile", b =>
                {
                    b.Property<int>("InterestsId")
                        .HasColumnType("integer");

                    b.Property<string>("ProfilesId")
                        .HasColumnType("text");

                    b.HasKey("InterestsId", "ProfilesId");

                    b.HasIndex("ProfilesId");

                    b.ToTable("InterestUserProfile");
                });

            modelBuilder.Entity("LanguageUserProfile", b =>
                {
                    b.Property<int>("LanguagesId")
                        .HasColumnType("integer");

                    b.Property<string>("ProfilesId")
                        .HasColumnType("text");

                    b.HasKey("LanguagesId", "ProfilesId");

                    b.HasIndex("ProfilesId");

                    b.ToTable("LanguageUserProfile");
                });

            modelBuilder.Entity("Profile.Domain.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CountryId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Profile.Domain.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Profile.Domain.Models.Education", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Educations");
                });

            modelBuilder.Entity("Profile.Domain.Models.Goal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Goals");
                });

            modelBuilder.Entity("Profile.Domain.Models.Interest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Interests");
                });

            modelBuilder.Entity("Profile.Domain.Models.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Profile.Domain.Models.Preference", b =>
                {
                    b.Property<string>("ProfileId")
                        .HasColumnType("text");

                    b.Property<int>("AgeFrom")
                        .HasColumnType("integer");

                    b.Property<int>("AgeTo")
                        .HasColumnType("integer");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int>("MaxDistance")
                        .HasColumnType("integer");

                    b.HasKey("ProfileId");

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("Profile.Domain.Models.UserEducation", b =>
                {
                    b.Property<string>("ProfileId")
                        .HasColumnType("text");

                    b.Property<int>("EducationId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ProfileId", "EducationId");

                    b.HasIndex("EducationId");

                    b.ToTable("UserEducation");
                });

            modelBuilder.Entity("Profile.Domain.Models.UserProfile", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text");

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("CityId")
                        .HasColumnType("integer");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<int?>("GoalId")
                        .HasColumnType("integer");

                    b.Property<int>("Height")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastOnline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("ShowAge")
                        .HasColumnType("boolean");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("GoalId");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("InterestUserProfile", b =>
                {
                    b.HasOne("Profile.Domain.Models.Interest", null)
                        .WithMany()
                        .HasForeignKey("InterestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Profile.Domain.Models.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("ProfilesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LanguageUserProfile", b =>
                {
                    b.HasOne("Profile.Domain.Models.Language", null)
                        .WithMany()
                        .HasForeignKey("LanguagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Profile.Domain.Models.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("ProfilesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Profile.Domain.Models.City", b =>
                {
                    b.HasOne("Profile.Domain.Models.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Profile.Domain.Models.Preference", b =>
                {
                    b.HasOne("Profile.Domain.Models.UserProfile", "Profile")
                        .WithOne("Preference")
                        .HasForeignKey("Profile.Domain.Models.Preference", "ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Profile.Domain.Models.UserEducation", b =>
                {
                    b.HasOne("Profile.Domain.Models.Education", "Education")
                        .WithMany("UserEducations")
                        .HasForeignKey("EducationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Profile.Domain.Models.UserProfile", "Profile")
                        .WithMany("UserEducations")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Education");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Profile.Domain.Models.UserProfile", b =>
                {
                    b.HasOne("Profile.Domain.Models.City", "City")
                        .WithMany("Profiles")
                        .HasForeignKey("CityId");

                    b.HasOne("Profile.Domain.Models.Goal", "Goal")
                        .WithMany("Profiles")
                        .HasForeignKey("GoalId");

                    b.Navigation("City");

                    b.Navigation("Goal");
                });

            modelBuilder.Entity("Profile.Domain.Models.City", b =>
                {
                    b.Navigation("Profiles");
                });

            modelBuilder.Entity("Profile.Domain.Models.Country", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("Profile.Domain.Models.Education", b =>
                {
                    b.Navigation("UserEducations");
                });

            modelBuilder.Entity("Profile.Domain.Models.Goal", b =>
                {
                    b.Navigation("Profiles");
                });

            modelBuilder.Entity("Profile.Domain.Models.UserProfile", b =>
                {
                    b.Navigation("Preference")
                        .IsRequired();

                    b.Navigation("UserEducations");
                });
#pragma warning restore 612, 618
        }
    }
}
