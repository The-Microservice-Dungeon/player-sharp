﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sharp.Data.Context;

#nullable disable

namespace Sharp.Data.Migrations
{
    [DbContext(typeof(SharpDbContext))]
    partial class SharpDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("Sharp.Data.Model.GameRegistration", b =>
                {
                    b.Property<string>("GameId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerDetailsEmail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerDetailsName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("GameId");

                    b.HasIndex("PlayerDetailsName", "PlayerDetailsEmail");

                    b.ToTable("GameRegistrations");
                });

            modelBuilder.Entity("Sharp.Data.Model.PlayerDetails", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Name", "Email");

                    b.ToTable("PlayerDetails");
                });

            modelBuilder.Entity("Sharp.Data.Model.GameRegistration", b =>
                {
                    b.HasOne("Sharp.Data.Model.PlayerDetails", "PlayerDetails")
                        .WithMany()
                        .HasForeignKey("PlayerDetailsName", "PlayerDetailsEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlayerDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
