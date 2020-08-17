﻿// <auto-generated />
using System;
using BeltExam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BeltExam.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BeltExam.Models.Association", b =>
                {
                    b.Property<int>("AssociationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FuntimeId");

                    b.Property<int>("UserId");

                    b.HasKey("AssociationId");

                    b.HasIndex("FuntimeId");

                    b.HasIndex("UserId");

                    b.ToTable("Associations");
                });

            modelBuilder.Entity("BeltExam.Models.Funtime", b =>
                {
                    b.Property<int>("FuntimeId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<int>("Duration");

                    b.Property<string>("Length");

                    b.Property<TimeSpan>("Time");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserId");

                    b.HasKey("FuntimeId");

                    b.HasIndex("UserId");

                    b.ToTable("Funtimes");
                });

            modelBuilder.Entity("BeltExam.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BeltExam.Models.Association", b =>
                {
                    b.HasOne("BeltExam.Models.Funtime", "Funtime")
                        .WithMany("Associations")
                        .HasForeignKey("FuntimeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BeltExam.Models.User", "Participant")
                        .WithMany("Associations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BeltExam.Models.Funtime", b =>
                {
                    b.HasOne("BeltExam.Models.User", "Planner")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
