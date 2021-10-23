﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using dashserver.Infrastructure;

namespace dashserver.Migrations
{
    [DbContext(typeof(DashDBContext))]
    [Migration("20211023185251_Update4")]
    partial class Update4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("dashserver.Models.DB.KPI", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("KPIId")
                        .HasColumnType("integer");

                    b.Property<int>("KPIType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Threshold")
                        .HasColumnType("numeric");

                    b.Property<bool>("ThresholdDirection")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("KPIId");

                    b.ToTable("KPIs");
                });

            modelBuilder.Entity("dashserver.Models.DB.KPIRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("KPIId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("KPIId");

                    b.ToTable("KPIRecords");
                });

            modelBuilder.Entity("dashserver.Models.DB.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("dashserver.Models.DB.PlanDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Day")
                        .HasColumnType("integer");

                    b.Property<int>("DurationInDays")
                        .HasColumnType("integer");

                    b.Property<decimal>("OccupiedPercent")
                        .HasColumnType("numeric");

                    b.Property<int>("PlanId")
                        .HasColumnType("integer");

                    b.Property<int>("ResourceGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("ResourceId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("UnavailablePercent")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("PlanDays");
                });

            modelBuilder.Entity("dashserver.Models.DB.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ResourceGroupId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResourceGroupId");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("dashserver.Models.DB.ResourceGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ShopId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("ResourceGroups");
                });

            modelBuilder.Entity("dashserver.Models.DB.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("dashserver.Models.DB.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("dashserver.Models.DB.StockBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("AllowedOverload")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("MaxBalance")
                        .HasColumnType("numeric");

                    b.Property<int>("PlanId")
                        .HasColumnType("integer");

                    b.Property<decimal>("PlannedBalance")
                        .HasColumnType("numeric");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlanId");

                    b.HasIndex("StockId");

                    b.ToTable("StockBalances");
                });

            modelBuilder.Entity("dashserver.Models.DB.StockLink", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ResourceGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResourceGroupId");

                    b.HasIndex("StockId");

                    b.ToTable("StockLinks");
                });

            modelBuilder.Entity("dashserver.Models.DB.KPI", b =>
                {
                    b.HasOne("dashserver.Models.DB.KPI", null)
                        .WithMany("KPIRecords")
                        .HasForeignKey("KPIId");
                });

            modelBuilder.Entity("dashserver.Models.DB.KPIRecord", b =>
                {
                    b.HasOne("dashserver.Models.DB.KPI", "KPI")
                        .WithMany()
                        .HasForeignKey("KPIId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KPI");
                });

            modelBuilder.Entity("dashserver.Models.DB.Resource", b =>
                {
                    b.HasOne("dashserver.Models.DB.ResourceGroup", null)
                        .WithMany("Resources")
                        .HasForeignKey("ResourceGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dashserver.Models.DB.ResourceGroup", b =>
                {
                    b.HasOne("dashserver.Models.DB.Shop", null)
                        .WithMany("ResourceGroups")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dashserver.Models.DB.StockBalance", b =>
                {
                    b.HasOne("dashserver.Models.DB.Plan", "Plan")
                        .WithMany()
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dashserver.Models.DB.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plan");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("dashserver.Models.DB.StockLink", b =>
                {
                    b.HasOne("dashserver.Models.DB.ResourceGroup", "ResourceGroup")
                        .WithMany()
                        .HasForeignKey("ResourceGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dashserver.Models.DB.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ResourceGroup");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("dashserver.Models.DB.KPI", b =>
                {
                    b.Navigation("KPIRecords");
                });

            modelBuilder.Entity("dashserver.Models.DB.ResourceGroup", b =>
                {
                    b.Navigation("Resources");
                });

            modelBuilder.Entity("dashserver.Models.DB.Shop", b =>
                {
                    b.Navigation("ResourceGroups");
                });
#pragma warning restore 612, 618
        }
    }
}