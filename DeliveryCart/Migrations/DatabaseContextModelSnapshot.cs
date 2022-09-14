﻿// <auto-generated />
using Assignment_2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeliveryCart.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

            modelBuilder.Entity("Assignment_2.Models.Item", b =>
                {
                    b.Property<int>("ItemID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ItemID");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("Assignment_2.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CustomerAddress")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("TEXT");

                    b.Property<double>("OrderTotal")
                        .HasColumnType("REAL");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("OrderID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Assignment_2.Models.OrderedItem", b =>
                {
                    b.Property<int>("OrderID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemID")
                        .HasColumnType("INTEGER");

                    b.HasKey("OrderID", "ItemID");

                    b.HasIndex("ItemID");

                    b.ToTable("OrderedItems");
                });

            modelBuilder.Entity("Assignment_2.Models.OrderedItem", b =>
                {
                    b.HasOne("Assignment_2.Models.Item", "Item")
                        .WithMany("OrderedItems")
                        .HasForeignKey("ItemID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Assignment_2.Models.Order", "Order")
                        .WithMany("OrderedItems")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Assignment_2.Models.Item", b =>
                {
                    b.Navigation("OrderedItems");
                });

            modelBuilder.Entity("Assignment_2.Models.Order", b =>
                {
                    b.Navigation("OrderedItems");
                });
#pragma warning restore 612, 618
        }
    }
}