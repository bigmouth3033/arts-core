﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using arts_core.Data;

#nullable disable

namespace arts_core.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240627094428_payment-createdAt")]
    partial class paymentcreatedAt
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TypeVariant", b =>
                {
                    b.Property<int>("TypesId")
                        .HasColumnType("int");

                    b.Property<int>("VariantsId")
                        .HasColumnType("int");

                    b.HasKey("TypesId", "VariantsId");

                    b.HasIndex("VariantsId");

                    b.ToTable("TypeVariant");
                });

            modelBuilder.Entity("arts_core.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AddressDetail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Province")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Ward")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("arts_core.Models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsChecked")
                        .HasColumnType("bit");

                    b.Property<int>("Quanity")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VariantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("VariantId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("arts_core.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("IconImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Arts"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Gift Articles"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Greeting Cards"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Dolls"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Files"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Hand Bags"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Wallets"
                        });
                });

            modelBuilder.Entity("arts_core.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Banner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("EventTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Id");

                    b.HasIndex("EventTypeId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("arts_core.Models.Exchange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExchangeDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("NewOrderId")
                        .HasColumnType("int");

                    b.Property<int>("OriginalOrderId")
                        .HasColumnType("int");

                    b.Property<string>("ReasonExchange")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseExchange")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NewOrderId");

                    b.HasIndex("OriginalOrderId");

                    b.ToTable("Exchanges");
                });

            modelBuilder.Entity("arts_core.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("From")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MessageContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("arts_core.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("OrderStatusId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<int>("Quanity")
                        .HasColumnType("int");

                    b.Property<float?>("TotalPrice")
                        .HasColumnType("real");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VariantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserId");

                    b.HasIndex("VariantId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("arts_core.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("DeliveryTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsCancel")
                        .HasColumnType("bit");

                    b.Property<int>("PaymentTypeId")
                        .HasColumnType("int");

                    b.Property<float>("ShipFee")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("DeliveryTypeId");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("arts_core.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WarrantyDuration")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("arts_core.Models.ProductEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<float>("SalePrice")
                        .HasColumnType("real");

                    b.Property<int>("VariantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("VariantId");

                    b.ToTable("ProductEvents");
                });

            modelBuilder.Entity("arts_core.Models.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("arts_core.Models.Refund", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<string>("ReasonRefund")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseRefund")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("PaymentId");

                    b.ToTable("Refunds");
                });

            modelBuilder.Entity("arts_core.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("arts_core.Models.ReviewImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReviewId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ReviewId");

                    b.ToTable("ReviewImages");
                });

            modelBuilder.Entity("arts_core.Models.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Types");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Fast",
                            NameType = "DeliveryType"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Normal",
                            NameType = "DeliveryType"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Material",
                            NameType = "VariantAttribute"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Admin",
                            NameType = "UserRole"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Employee",
                            NameType = "UserRole"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Customer",
                            NameType = "UserRole"
                        },
                        new
                        {
                            Id = 7,
                            Name = "VPP",
                            NameType = "PaymentType"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Cheque",
                            NameType = "PaymentType"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Credit",
                            NameType = "PaymentType"
                        },
                        new
                        {
                            Id = 10,
                            Name = "DD",
                            NameType = "PaymentType"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Size",
                            NameType = "VariantAttribute"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Color",
                            NameType = "VariantAttribute"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Pending",
                            NameType = "OrdersStatusType"
                        },
                        new
                        {
                            Id = 14,
                            Name = "Accepted",
                            NameType = "OrdersStatusType"
                        },
                        new
                        {
                            Id = 15,
                            Name = "Denied",
                            NameType = "OrdersStatusType"
                        },
                        new
                        {
                            Id = 16,
                            Name = "Success",
                            NameType = "OrdersStatusType"
                        },
                        new
                        {
                            Id = 17,
                            Name = "Delivery",
                            NameType = "OrdersStatusType"
                        });
                });

            modelBuilder.Entity("arts_core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RestrictedTypeId")
                        .HasColumnType("int");

                    b.Property<int?>("RoleTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Verifired")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("RestrictedTypeId");

                    b.HasIndex("RoleTypeId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = false,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "admin@admin.com",
                            Fullname = "Admin",
                            Password = "$2a$12$exQrFheHS3stHVydhi6.euQVkDzV0bplJ69dnLzAw6ls2Hmv.zP9O",
                            RoleTypeId = 4,
                            UpdatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Verifired = false
                        });
                });

            modelBuilder.Entity("arts_core.Models.Variant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("AvailableQuanity")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quanity")
                        .HasColumnType("int");

                    b.Property<float>("SalePrice")
                        .HasColumnType("real");

                    b.Property<string>("VariantImage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Variants");
                });

            modelBuilder.Entity("arts_core.Models.VariantAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AttributeTypeId")
                        .HasColumnType("int");

                    b.Property<string>("AttributeValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<int>("VariantId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttributeTypeId");

                    b.HasIndex("VariantId");

                    b.ToTable("VariantAttributes");
                });

            modelBuilder.Entity("TypeVariant", b =>
                {
                    b.HasOne("arts_core.Models.Type", null)
                        .WithMany()
                        .HasForeignKey("TypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Variant", null)
                        .WithMany()
                        .HasForeignKey("VariantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("arts_core.Models.Address", b =>
                {
                    b.HasOne("arts_core.Models.User", "User")
                        .WithMany("Addresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("arts_core.Models.Cart", b =>
                {
                    b.HasOne("arts_core.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Variant", "Variant")
                        .WithMany()
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("arts_core.Models.Event", b =>
                {
                    b.HasOne("arts_core.Models.Type", "EventType")
                        .WithMany("Events")
                        .HasForeignKey("EventTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventType");
                });

            modelBuilder.Entity("arts_core.Models.Exchange", b =>
                {
                    b.HasOne("arts_core.Models.Order", "NewOrder")
                        .WithMany()
                        .HasForeignKey("NewOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Order", "OriginalOrder")
                        .WithMany("Exchanges")
                        .HasForeignKey("OriginalOrderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("NewOrder");

                    b.Navigation("OriginalOrder");
                });

            modelBuilder.Entity("arts_core.Models.Message", b =>
                {
                    b.HasOne("arts_core.Models.User", "User")
                        .WithMany("Messages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("arts_core.Models.Order", b =>
                {
                    b.HasOne("arts_core.Models.Type", "OrderStatusType")
                        .WithMany("Orders")
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Payment", "Payment")
                        .WithMany("Orders")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("arts_core.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Variant", "Variant")
                        .WithMany()
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderStatusType");

                    b.Navigation("Payment");

                    b.Navigation("User");

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("arts_core.Models.Payment", b =>
                {
                    b.HasOne("arts_core.Models.Address", "Address")
                        .WithMany("Payments")
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Type", "DeliveryType")
                        .WithMany()
                        .HasForeignKey("DeliveryTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Type", "PaymentType")
                        .WithMany("Payments")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("DeliveryType");

                    b.Navigation("PaymentType");
                });

            modelBuilder.Entity("arts_core.Models.Product", b =>
                {
                    b.HasOne("arts_core.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("arts_core.Models.ProductEvent", b =>
                {
                    b.HasOne("arts_core.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Variant", "Variant")
                        .WithMany()
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("arts_core.Models.ProductImage", b =>
                {
                    b.HasOne("arts_core.Models.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("arts_core.Models.Refund", b =>
                {
                    b.HasOne("arts_core.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Payment", "Payment")
                        .WithMany("Refunds")
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("arts_core.Models.Review", b =>
                {
                    b.HasOne("arts_core.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("arts_core.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("arts_core.Models.ReviewImage", b =>
                {
                    b.HasOne("arts_core.Models.Review", "Review")
                        .WithMany("ReviewImages")
                        .HasForeignKey("ReviewId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Review");
                });

            modelBuilder.Entity("arts_core.Models.User", b =>
                {
                    b.HasOne("arts_core.Models.Type", "RestrictedType")
                        .WithMany()
                        .HasForeignKey("RestrictedTypeId");

                    b.HasOne("arts_core.Models.Type", "RoleType")
                        .WithMany("Users")
                        .HasForeignKey("RoleTypeId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("RestrictedType");

                    b.Navigation("RoleType");
                });

            modelBuilder.Entity("arts_core.Models.Variant", b =>
                {
                    b.HasOne("arts_core.Models.Product", "Product")
                        .WithMany("Variants")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("arts_core.Models.VariantAttribute", b =>
                {
                    b.HasOne("arts_core.Models.Type", "AttributeType")
                        .WithMany()
                        .HasForeignKey("AttributeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("arts_core.Models.Variant", "Variant")
                        .WithMany("VariantAttributes")
                        .HasForeignKey("VariantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AttributeType");

                    b.Navigation("Variant");
                });

            modelBuilder.Entity("arts_core.Models.Address", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("arts_core.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("arts_core.Models.Order", b =>
                {
                    b.Navigation("Exchanges");
                });

            modelBuilder.Entity("arts_core.Models.Payment", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Refunds");
                });

            modelBuilder.Entity("arts_core.Models.Product", b =>
                {
                    b.Navigation("ProductImages");

                    b.Navigation("Variants");
                });

            modelBuilder.Entity("arts_core.Models.Review", b =>
                {
                    b.Navigation("ReviewImages");
                });

            modelBuilder.Entity("arts_core.Models.Type", b =>
                {
                    b.Navigation("Events");

                    b.Navigation("Orders");

                    b.Navigation("Payments");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("arts_core.Models.User", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Messages");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("arts_core.Models.Variant", b =>
                {
                    b.Navigation("VariantAttributes");
                });
#pragma warning restore 612, 618
        }
    }
}
