﻿// <auto-generated />
using Com.Danliris.Service.Internal.Transfer.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Com.Danliris.Service.Internal.Transfer.Lib.Migrations
{
    [DbContext(typeof(InternalTransferDbContext))]
    partial class InternalTransferDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel.ExternalTransferOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("CurrencyCode")
                        .HasMaxLength(255);

                    b.Property<string>("CurrencyDescription");

                    b.Property<string>("CurrencyId")
                        .HasMaxLength(255);

                    b.Property<string>("CurrencyRate")
                        .HasMaxLength(255);

                    b.Property<string>("CurrencySymbol")
                        .HasMaxLength(255);

                    b.Property<DateTime>("DeliveryDate");

                    b.Property<string>("DeliveryDivisionCode")
                        .HasMaxLength(255);

                    b.Property<string>("DeliveryDivisionId")
                        .HasMaxLength(255);

                    b.Property<string>("DeliveryDivisionName")
                        .HasMaxLength(255);

                    b.Property<string>("ETONo");

                    b.Property<bool>("IsCanceled");

                    b.Property<bool>("IsClosed");

                    b.Property<bool>("IsPosted");

                    b.Property<DateTime>("OrderDate");

                    b.Property<string>("OrderDivisionCode");

                    b.Property<string>("OrderDivisionId");

                    b.Property<string>("OrderDivisionName");

                    b.Property<string>("Remark")
                        .HasMaxLength(1000);

                    b.Property<string>("_CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_CreatedUtc");

                    b.Property<string>("_DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_DeletedUtc");

                    b.Property<bool>("_IsDeleted");

                    b.Property<string>("_LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_LastModifiedUtc");

                    b.HasKey("Id");

                    b.ToTable("ExternalTransferOrders");
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel.ExternalTransferOrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<double>("Convertion");

                    b.Property<double>("DOQuantity");

                    b.Property<double>("DealQuantity");

                    b.Property<string>("DealUomId")
                        .HasMaxLength(255);

                    b.Property<string>("DealUomUnit")
                        .HasMaxLength(255);

                    b.Property<double>("DefaultQuantity");

                    b.Property<string>("DefaultUomId")
                        .HasMaxLength(255);

                    b.Property<string>("DefaultUomUnit")
                        .HasMaxLength(255);

                    b.Property<int>("ETOItemId");

                    b.Property<string>("Grade")
                        .HasMaxLength(255);

                    b.Property<int>("ITODetailId");

                    b.Property<double>("Price");

                    b.Property<string>("ProductCode")
                        .HasMaxLength(255);

                    b.Property<string>("ProductId")
                        .HasMaxLength(255);

                    b.Property<string>("ProductName")
                        .HasMaxLength(255);

                    b.Property<string>("ProductRemark")
                        .HasMaxLength(1000);

                    b.Property<double>("RemainingQuantity");

                    b.Property<int>("TRDetailId");

                    b.Property<string>("_CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_CreatedUtc");

                    b.Property<string>("_DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_DeletedUtc");

                    b.Property<bool>("_IsDeleted");

                    b.Property<string>("_LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_LastModifiedUtc");

                    b.HasKey("Id");

                    b.HasIndex("ETOItemId");

                    b.ToTable("ExternalTransferOrderDetails");
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel.ExternalTransferOrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("ETOId");

                    b.Property<int>("ITOId");

                    b.Property<string>("ITONo")
                        .HasMaxLength(255);

                    b.Property<int>("TRId");

                    b.Property<string>("TRNo")
                        .HasMaxLength(255);

                    b.Property<string>("UnitCode");

                    b.Property<string>("UnitId");

                    b.Property<string>("UnitName");

                    b.Property<string>("_CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_CreatedUtc");

                    b.Property<string>("_DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_DeletedUtc");

                    b.Property<bool>("_IsDeleted");

                    b.Property<string>("_LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_LastModifiedUtc");

                    b.HasKey("Id");

                    b.HasIndex("ETOId");

                    b.ToTable("ExternalTransferOrderItems");
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.InternalTransferOrderModel.InternalTransferOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("AutoIncrementNumber");

                    b.Property<string>("CategoryCode");

                    b.Property<string>("CategoryId");

                    b.Property<string>("CategoryName");

                    b.Property<string>("DivisionCode");

                    b.Property<string>("DivisionId");

                    b.Property<string>("DivisionName");

                    b.Property<string>("ITONo")
                        .HasMaxLength(50);

                    b.Property<bool>("IsCanceled");

                    b.Property<bool>("IsPost");

                    b.Property<string>("Remarks");

                    b.Property<DateTime>("RequestedArrivalDate");

                    b.Property<DateTime>("TRDate");

                    b.Property<int>("TRId");

                    b.Property<string>("TRNo")
                        .HasMaxLength(50);

                    b.Property<string>("UnitCode");

                    b.Property<string>("UnitId");

                    b.Property<string>("UnitName");

                    b.Property<string>("_CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_CreatedUtc");

                    b.Property<string>("_DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_DeletedUtc");

                    b.Property<bool>("_IsDeleted");

                    b.Property<string>("_LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_LastModifiedUtc");

                    b.HasKey("Id");

                    b.ToTable("InternalTransferOrders");
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.InternalTransferOrderModel.InternalTransferOrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Grade");

                    b.Property<int>("ITOId");

                    b.Property<string>("ProductCode");

                    b.Property<string>("ProductId")
                        .HasMaxLength(100);

                    b.Property<string>("ProductName");

                    b.Property<string>("ProductRemark")
                        .HasMaxLength(1000);

                    b.Property<double>("Quantity");

                    b.Property<string>("Status");

                    b.Property<int>("TRDetailId");

                    b.Property<string>("UomId");

                    b.Property<string>("UomUnit");

                    b.Property<string>("_CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_CreatedUtc");

                    b.Property<string>("_DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_DeletedUtc");

                    b.Property<bool>("_IsDeleted");

                    b.Property<string>("_LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_LastModifiedUtc");

                    b.HasKey("Id");

                    b.HasIndex("ITOId");

                    b.ToTable("InternalTransferOrderDetails");
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.TransferRequestModel.TransferRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("AutoIncrementNumber");

                    b.Property<string>("CategoryCode")
                        .HasMaxLength(255);

                    b.Property<string>("CategoryId")
                        .HasMaxLength(255);

                    b.Property<string>("CategoryName")
                        .HasMaxLength(255);

                    b.Property<string>("DivisionCode");

                    b.Property<string>("DivisionId");

                    b.Property<string>("DivisionName");

                    b.Property<bool>("IsCanceled");

                    b.Property<bool>("IsPosted");

                    b.Property<string>("Remark")
                        .HasMaxLength(255);

                    b.Property<DateTime>("RequestedArrivalDate")
                        .HasMaxLength(255);

                    b.Property<DateTime>("TRDate")
                        .HasMaxLength(255);

                    b.Property<string>("TRNo")
                        .HasMaxLength(255);

                    b.Property<string>("UnitCode")
                        .HasMaxLength(255);

                    b.Property<string>("UnitId")
                        .HasMaxLength(255);

                    b.Property<string>("UnitName")
                        .HasMaxLength(255);

                    b.Property<string>("_CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_CreatedUtc");

                    b.Property<string>("_DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_DeletedUtc");

                    b.Property<bool>("_IsDeleted");

                    b.Property<string>("_LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_LastModifiedUtc");

                    b.HasKey("Id");

                    b.ToTable("TransferRequests");
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.TransferRequestModel.TransferRequestDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Grade");

                    b.Property<string>("ProductCode")
                        .HasMaxLength(255);

                    b.Property<string>("ProductId")
                        .HasMaxLength(255);

                    b.Property<string>("ProductName")
                        .HasMaxLength(255);

                    b.Property<string>("ProductRemark")
                        .HasMaxLength(255);

                    b.Property<double>("Quantity");

                    b.Property<string>("Status");

                    b.Property<int>("TransferRequestId");

                    b.Property<string>("UomId")
                        .HasMaxLength(255);

                    b.Property<string>("UomUnit")
                        .HasMaxLength(255);

                    b.Property<string>("_CreatedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_CreatedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_CreatedUtc");

                    b.Property<string>("_DeletedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_DeletedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_DeletedUtc");

                    b.Property<bool>("_IsDeleted");

                    b.Property<string>("_LastModifiedAgent")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("_LastModifiedBy")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("_LastModifiedUtc");

                    b.HasKey("Id");

                    b.HasIndex("TransferRequestId");

                    b.ToTable("TransferRequestDetails");
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel.ExternalTransferOrderDetail", b =>
                {
                    b.HasOne("Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel.ExternalTransferOrderItem", "ExternalTransferOrderItem")
                        .WithMany("ExternalTransferOrderDetails")
                        .HasForeignKey("ETOItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel.ExternalTransferOrderItem", b =>
                {
                    b.HasOne("Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel.ExternalTransferOrder", "ExternalTransferOrder")
                        .WithMany("ExternalTransferOrderItems")
                        .HasForeignKey("ETOId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.InternalTransferOrderModel.InternalTransferOrderDetail", b =>
                {
                    b.HasOne("Com.Danliris.Service.Internal.Transfer.Lib.Models.InternalTransferOrderModel.InternalTransferOrder", "InternalTransferOrder")
                        .WithMany("InternalTransferOrderDetails")
                        .HasForeignKey("ITOId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Com.Danliris.Service.Internal.Transfer.Lib.Models.TransferRequestModel.TransferRequestDetail", b =>
                {
                    b.HasOne("Com.Danliris.Service.Internal.Transfer.Lib.Models.TransferRequestModel.TransferRequest", "TransferRequest")
                        .WithMany("TransferRequestDetails")
                        .HasForeignKey("TransferRequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
