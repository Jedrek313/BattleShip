﻿// <auto-generated />
using System;
using BattleShip.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BattleShip.Migrations
{
    [DbContext(typeof(BattleShipContext))]
    partial class BattleShipContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BattleShip.Models.Board.Game", b =>
                {
                    b.Property<int>("gameId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("finished");

                    b.Property<int?>("playerId");

                    b.HasKey("gameId");

                    b.HasIndex("playerId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("BattleShip.Models.Board.Move", b =>
                {
                    b.Property<int>("moveId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("gameId");

                    b.Property<bool>("playerMove");

                    b.Property<int>("x");

                    b.Property<int>("y");

                    b.HasKey("moveId");

                    b.HasIndex("gameId");

                    b.ToTable("Move");
                });

            modelBuilder.Entity("BattleShip.Models.Board.Player", b =>
                {
                    b.Property<int>("playerId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("playerName");

                    b.HasKey("playerId");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("BattleShip.Models.Board.Ship", b =>
                {
                    b.Property<int>("shipId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("gameId");

                    b.Property<bool>("playerShip");

                    b.Property<int>("shipNumber");

                    b.Property<int>("x");

                    b.Property<int>("y");

                    b.HasKey("shipId");

                    b.HasIndex("gameId");

                    b.ToTable("Ship");
                });

            modelBuilder.Entity("BattleShip.Models.Board.Game", b =>
                {
                    b.HasOne("BattleShip.Models.Board.Player")
                        .WithMany("games")
                        .HasForeignKey("playerId");
                });

            modelBuilder.Entity("BattleShip.Models.Board.Move", b =>
                {
                    b.HasOne("BattleShip.Models.Board.Game")
                        .WithMany("moves")
                        .HasForeignKey("gameId");
                });

            modelBuilder.Entity("BattleShip.Models.Board.Ship", b =>
                {
                    b.HasOne("BattleShip.Models.Board.Game")
                        .WithMany("ships")
                        .HasForeignKey("gameId");
                });
#pragma warning restore 612, 618
        }
    }
}