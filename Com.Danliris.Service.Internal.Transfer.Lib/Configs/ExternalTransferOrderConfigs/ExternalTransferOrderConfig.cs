﻿using Com.Danliris.Service.Internal.Transfer.Lib.Models.ExternalTransferOrderModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Danliris.Service.Internal.Transfer.Lib.Configs.ExternalTransferOrderConfigs
{
    public class ExternalTransferOrderConfig : IEntityTypeConfiguration<ExternalTransferOrder>
    {
        public void Configure(EntityTypeBuilder<ExternalTransferOrder> builder)
        {
            builder.Property(p => p.DivisionId).HasMaxLength(255);
            builder.Property(p => p.DivisionCode).HasMaxLength(255);
            builder.Property(p => p.DivisionName).HasMaxLength(255);
            builder.Property(p => p.CurrencyId).HasMaxLength(255);
            builder.Property(p => p.CurrencyCode).HasMaxLength(255);
            builder.Property(p => p.CurrencySymbol).HasMaxLength(255);
            builder.Property(p => p.CurrencyRate).HasMaxLength(255);
            builder.Property(p => p.Remark).HasMaxLength(1000);

            builder
                .HasMany(h => h.ExternalTransferOrderItems)
                .WithOne(w => w.ExternalTransferOrder)
                .HasForeignKey(f => f.ETOId)
                .IsRequired();
        }
    }
}
