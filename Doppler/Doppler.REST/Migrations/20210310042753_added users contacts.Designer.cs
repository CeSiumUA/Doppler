﻿// <auto-generated />
using System;
using Doppler.REST.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Doppler.REST.Migrations
{
    [DbContext(typeof(ApplicationDatabaseContext))]
    [Migration("20210310042753_added users contacts")]
    partial class addeduserscontacts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Doppler.API.Authentication.JwtToken", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Token");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Doppler.API.Social.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("Doppler.API.Social.UserContact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("ContactId")
                        .HasColumnType("int");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersContacts");
                });

            modelBuilder.Entity("Doppler.REST.Models.Cryptography.Password", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Iterations")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Passwords");
                });

            modelBuilder.Entity("Doppler.REST.Models.Social.DopplerUser", b =>
                {
                    b.HasBaseType("Doppler.API.Social.User");

                    b.Property<int?>("PasswordId")
                        .HasColumnType("int");

                    b.Property<string>("RefreshTokenToken")
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("PasswordId");

                    b.HasIndex("RefreshTokenToken");

                    b.HasDiscriminator().HasValue("DopplerUser");
                });

            modelBuilder.Entity("Doppler.API.Social.UserContact", b =>
                {
                    b.HasOne("Doppler.API.Social.User", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId");

                    b.HasOne("Doppler.API.Social.User", "User")
                        .WithMany("UserContacts")
                        .HasForeignKey("UserId");

                    b.Navigation("Contact");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Doppler.REST.Models.Social.DopplerUser", b =>
                {
                    b.HasOne("Doppler.REST.Models.Cryptography.Password", "Password")
                        .WithMany()
                        .HasForeignKey("PasswordId");

                    b.HasOne("Doppler.API.Authentication.JwtToken", "RefreshToken")
                        .WithMany()
                        .HasForeignKey("RefreshTokenToken");

                    b.Navigation("Password");

                    b.Navigation("RefreshToken");
                });

            modelBuilder.Entity("Doppler.API.Social.User", b =>
                {
                    b.Navigation("UserContacts");
                });
#pragma warning restore 612, 618
        }
    }
}
