using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace Infrastructure.Data.Config
{
    public class ShoppingOrderProductConfiguration : IEntityTypeConfiguration<ShoppingOrderProduct>
    {
        public void Configure(EntityTypeBuilder<ShoppingOrderProduct> builder)
        {
            builder.Property(p => p.Id).UseIdentityColumn();
        }
    }
}
