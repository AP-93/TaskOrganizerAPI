﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskOrganizerAPI.Data;

namespace TaskOrganizerAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190730145428_addDataMigration")]
    partial class addDataMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TaskOrganizerAPI.Model.Board", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("TaskOrganizerAPI.Model.BoardList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardId");

                    b.Property<string>("ListName");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.ToTable("BoardList");
                });

            modelBuilder.Entity("TaskOrganizerAPI.Model.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardListId");

                    b.Property<string>("CardText");

                    b.HasKey("Id");

                    b.HasIndex("BoardListId");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("TaskOrganizerAPI.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("PaswordHash");

                    b.Property<byte[]>("PaswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskOrganizerAPI.Model.UserBoard", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("BoardId");

                    b.HasKey("UserId", "BoardId");

                    b.HasIndex("BoardId");

                    b.ToTable("UserBoards");
                });

            modelBuilder.Entity("TaskOrganizerAPI.Model.BoardList", b =>
                {
                    b.HasOne("TaskOrganizerAPI.Model.Board", "Board")
                        .WithMany("BoardLists")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TaskOrganizerAPI.Model.Card", b =>
                {
                    b.HasOne("TaskOrganizerAPI.Model.BoardList", "BoardList")
                        .WithMany("Cards")
                        .HasForeignKey("BoardListId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TaskOrganizerAPI.Model.UserBoard", b =>
                {
                    b.HasOne("TaskOrganizerAPI.Model.Board", "Board")
                        .WithMany("Users")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TaskOrganizerAPI.Model.User", "User")
                        .WithMany("Boards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}