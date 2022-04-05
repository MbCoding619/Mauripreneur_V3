﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("API.Entities.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LName")
                        .HasColumnType("TEXT");

                    b.HasKey("AdminId");

                    b.HasIndex("AppUserId")
                        .IsUnique();

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Property<int>("AppUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AppUserRole")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("BLOB");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("accountStatus")
                        .HasColumnType("TEXT");

                    b.Property<string>("imagePath")
                        .HasColumnType("TEXT");

                    b.HasKey("AppUserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Entities.Application", b =>
                {
                    b.Property<int>("StudId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VacId")
                        .HasColumnType("INTEGER");

                    b.HasKey("StudId", "VacId");

                    b.HasIndex("VacId");

                    b.ToTable("Application");
                });

            modelBuilder.Entity("API.Entities.Bid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BidAmount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BidDate")
                        .HasColumnType("Date");

                    b.Property<string>("BidResponse")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("JobId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OtherDetails")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProfessionalId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SmeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("ProfessionalId");

                    b.HasIndex("SmeId");

                    b.ToTable("Bid");
                });

            modelBuilder.Entity("API.Entities.Field", b =>
                {
                    b.Property<int>("FieldId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("fieldStatus")
                        .HasColumnType("TEXT");

                    b.HasKey("FieldId");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("API.Entities.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Budget")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Desc")
                        .HasColumnType("TEXT");

                    b.Property<int>("FieldId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("JobTitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("Requirements")
                        .HasColumnType("TEXT");

                    b.Property<int>("SmeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Timeframe")
                        .HasColumnType("TEXT");

                    b.Property<string>("filePath")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("SmeId");

                    b.ToTable("Job");
                });

            modelBuilder.Entity("API.Entities.Meeting", b =>
                {
                    b.Property<int>("MeetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BidId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MeetTitle")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProfId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SmeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("StudId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("VacId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("endDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("startDate")
                        .HasColumnType("TEXT");

                    b.HasKey("MeetId");

                    b.HasIndex("BidId");

                    b.HasIndex("ProfId");

                    b.HasIndex("SmeId");

                    b.HasIndex("StudId");

                    b.HasIndex("VacId");

                    b.ToTable("Meeting");
                });

            modelBuilder.Entity("API.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrgName")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrgRepresent_FName")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrgRepresent_LName")
                        .HasColumnType("TEXT");

                    b.Property<int>("Phone")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId")
                        .IsUnique();

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("API.Entities.Professional", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BriefDesc")
                        .HasColumnType("TEXT");

                    b.Property<string>("EducationInstition")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmploymentHistory")
                        .HasColumnType("TEXT");

                    b.Property<string>("EmploymentStatus")
                        .HasColumnType("TEXT");

                    b.Property<string>("FName")
                        .HasColumnType("TEXT");

                    b.Property<int>("FieldId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IDNum")
                        .HasColumnType("TEXT");

                    b.Property<string>("LName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LinkedInLink")
                        .HasColumnType("TEXT");

                    b.Property<int>("Phone")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Qual1")
                        .HasColumnType("TEXT");

                    b.Property<string>("Qual2")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId")
                        .IsUnique();

                    b.HasIndex("FieldId");

                    b.ToTable("Professionals");
                });

            modelBuilder.Entity("API.Entities.Sme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("RepresentLName")
                        .HasColumnType("TEXT");

                    b.Property<string>("RepresentName")
                        .HasColumnType("TEXT");

                    b.Property<int>("RepresentPhone")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SocialLink")
                        .HasColumnType("TEXT");

                    b.Property<string>("compDescription")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId")
                        .IsUnique();

                    b.ToTable("Sme");
                });

            modelBuilder.Entity("API.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<int>("AppUserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Course")
                        .HasColumnType("TEXT");

                    b.Property<string>("Course_level")
                        .HasColumnType("TEXT");

                    b.Property<string>("FName")
                        .HasColumnType("TEXT");

                    b.Property<int>("FieldId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LinkedInLink")
                        .HasColumnType("TEXT");

                    b.Property<int>("Phone")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Uni")
                        .HasColumnType("TEXT");

                    b.Property<string>("briefDescription")
                        .HasColumnType("TEXT");

                    b.Property<string>("email")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId")
                        .IsUnique();

                    b.HasIndex("FieldId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("API.Entities.Vacancy", b =>
                {
                    b.Property<int>("VacId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Requirements")
                        .HasColumnType("TEXT");

                    b.Property<int>("SmeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VacTitle")
                        .HasColumnType("TEXT");

                    b.HasKey("VacId");

                    b.HasIndex("SmeId");

                    b.ToTable("Vacancy");
                });

            modelBuilder.Entity("API.Entities.Admin", b =>
                {
                    b.HasOne("API.Entities.AppUser", "User")
                        .WithOne("Admin")
                        .HasForeignKey("API.Entities.Admin", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Application", b =>
                {
                    b.HasOne("API.Entities.Student", "Student")
                        .WithMany("Vacancy")
                        .HasForeignKey("StudId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Vacancy", "Vacancy")
                        .WithMany("Students")
                        .HasForeignKey("VacId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("API.Entities.Bid", b =>
                {
                    b.HasOne("API.Entities.Job", "Job")
                        .WithMany("Bid")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Professional", "Professional")
                        .WithMany("Bid")
                        .HasForeignKey("ProfessionalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Sme", "Sme")
                        .WithMany("Bid")
                        .HasForeignKey("SmeId");

                    b.Navigation("Job");

                    b.Navigation("Professional");

                    b.Navigation("Sme");
                });

            modelBuilder.Entity("API.Entities.Job", b =>
                {
                    b.HasOne("API.Entities.Field", "Field")
                        .WithMany("Job")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Sme", "Sme")
                        .WithMany("Job")
                        .HasForeignKey("SmeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("Sme");
                });

            modelBuilder.Entity("API.Entities.Meeting", b =>
                {
                    b.HasOne("API.Entities.Bid", "Bid")
                        .WithMany("Meeting")
                        .HasForeignKey("BidId");

                    b.HasOne("API.Entities.Professional", "Professional")
                        .WithMany("Meeting")
                        .HasForeignKey("ProfId");

                    b.HasOne("API.Entities.Sme", "Sme")
                        .WithMany("Meeting")
                        .HasForeignKey("SmeId");

                    b.HasOne("API.Entities.Student", "Student")
                        .WithMany("Meeting")
                        .HasForeignKey("StudId");

                    b.HasOne("API.Entities.Vacancy", "Vacancy")
                        .WithMany("Meeting")
                        .HasForeignKey("VacId");

                    b.Navigation("Bid");

                    b.Navigation("Professional");

                    b.Navigation("Sme");

                    b.Navigation("Student");

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("API.Entities.Organization", b =>
                {
                    b.HasOne("API.Entities.AppUser", "User")
                        .WithOne("Organization")
                        .HasForeignKey("API.Entities.Organization", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Professional", b =>
                {
                    b.HasOne("API.Entities.AppUser", "User")
                        .WithOne("Professional")
                        .HasForeignKey("API.Entities.Professional", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Field", "Field")
                        .WithMany("Professional")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Sme", b =>
                {
                    b.HasOne("API.Entities.AppUser", "User")
                        .WithOne("Sme")
                        .HasForeignKey("API.Entities.Sme", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Student", b =>
                {
                    b.HasOne("API.Entities.AppUser", "User")
                        .WithOne("Student")
                        .HasForeignKey("API.Entities.Student", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API.Entities.Field", "Field")
                        .WithMany("Student")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.Vacancy", b =>
                {
                    b.HasOne("API.Entities.Sme", "Sme")
                        .WithMany("Vacancy")
                        .HasForeignKey("SmeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sme");
                });

            modelBuilder.Entity("API.Entities.AppUser", b =>
                {
                    b.Navigation("Admin");

                    b.Navigation("Organization");

                    b.Navigation("Professional");

                    b.Navigation("Sme");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("API.Entities.Bid", b =>
                {
                    b.Navigation("Meeting");
                });

            modelBuilder.Entity("API.Entities.Field", b =>
                {
                    b.Navigation("Job");

                    b.Navigation("Professional");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("API.Entities.Job", b =>
                {
                    b.Navigation("Bid");
                });

            modelBuilder.Entity("API.Entities.Professional", b =>
                {
                    b.Navigation("Bid");

                    b.Navigation("Meeting");
                });

            modelBuilder.Entity("API.Entities.Sme", b =>
                {
                    b.Navigation("Bid");

                    b.Navigation("Job");

                    b.Navigation("Meeting");

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("API.Entities.Student", b =>
                {
                    b.Navigation("Meeting");

                    b.Navigation("Vacancy");
                });

            modelBuilder.Entity("API.Entities.Vacancy", b =>
                {
                    b.Navigation("Meeting");

                    b.Navigation("Students");
                });
#pragma warning restore 612, 618
        }
    }
}
