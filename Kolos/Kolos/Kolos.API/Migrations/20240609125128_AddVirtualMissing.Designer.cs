﻿// <auto-generated />
using System;
using Kolos.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kolos.API.Migrations
{
    [DbContext(typeof(KolosDbContext))]
    [Migration("20240609125128_AddVirtualMissing")]
    partial class AddVirtualMissing
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Kolos.API.Data.Models.Client", b =>
                {
                    b.Property<int>("IdClient")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdClient"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdClient");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Discount", b =>
                {
                    b.Property<int>("IdDiscount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDiscount"));

                    b.Property<DateTime>("DateFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("IdDiscount");

                    b.HasIndex("IdClient");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Payment", b =>
                {
                    b.Property<int>("IdPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPayment"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdSubscription")
                        .HasColumnType("int");

                    b.HasKey("IdPayment");

                    b.HasIndex("IdClient");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Sale", b =>
                {
                    b.Property<int>("IdSale")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSale"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdClient")
                        .HasColumnType("int");

                    b.Property<int>("IdSubscription")
                        .HasColumnType("int");

                    b.HasKey("IdSale");

                    b.HasIndex("IdClient");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Subscription", b =>
                {
                    b.Property<int>("IdSubscription")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSubscription"));

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("PaymentIdPayment")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RenewallPeriod")
                        .HasColumnType("int");

                    b.Property<int?>("SaleIdSale")
                        .HasColumnType("int");

                    b.HasKey("IdSubscription");

                    b.HasIndex("PaymentIdPayment");

                    b.HasIndex("SaleIdSale");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Discount", b =>
                {
                    b.HasOne("Kolos.API.Data.Models.Client", "Client")
                        .WithMany("Discounts")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Payment", b =>
                {
                    b.HasOne("Kolos.API.Data.Models.Client", "Client")
                        .WithMany("Payments")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Sale", b =>
                {
                    b.HasOne("Kolos.API.Data.Models.Client", "Client")
                        .WithMany("Sales")
                        .HasForeignKey("IdClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Subscription", b =>
                {
                    b.HasOne("Kolos.API.Data.Models.Payment", null)
                        .WithMany("Subscriptions")
                        .HasForeignKey("PaymentIdPayment");

                    b.HasOne("Kolos.API.Data.Models.Sale", null)
                        .WithMany("Subscriptions")
                        .HasForeignKey("SaleIdSale");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Client", b =>
                {
                    b.Navigation("Discounts");

                    b.Navigation("Payments");

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Payment", b =>
                {
                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("Kolos.API.Data.Models.Sale", b =>
                {
                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
