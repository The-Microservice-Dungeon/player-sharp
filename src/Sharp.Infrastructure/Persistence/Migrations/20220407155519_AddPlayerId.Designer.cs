// <auto-generated />


#nullable disable

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sharp.Infrastructure.Persistence.Contexts;

namespace Sharp.Data.Migrations
{
    [DbContext(typeof(SharpDbContext))]
    [Migration("20220407155519_AddPlayerId")]
    partial class AddPlayerId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("Sharp.Data.Models.CommandTransaction", b =>
                {
                    b.Property<string>("TransactionId")
                        .HasColumnType("TEXT");

                    b.Property<int>("CommandType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlanetId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RobotId")
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetId")
                        .HasColumnType("TEXT");

                    b.HasKey("TransactionId");

                    b.ToTable("CommandTransactions");
                });

            modelBuilder.Entity("Sharp.Data.Models.GameRegistration", b =>
                {
                    b.Property<string>("GameId")
                        .HasColumnType("TEXT");

                    b.Property<string>("TransactionId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("GameId");

                    b.ToTable("GameRegistrations");
                });

            modelBuilder.Entity("Sharp.Data.Models.PlayerDetails", b =>
                {
                    b.Property<string>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PlayerId");

                    b.HasIndex("Email", "Name")
                        .IsUnique();

                    b.ToTable("PlayerDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
