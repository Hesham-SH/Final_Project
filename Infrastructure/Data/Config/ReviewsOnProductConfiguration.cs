using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Models;

namespace Infrastructure.Data.Config
{
    public class ReviewsOnProductConfiguration : IEntityTypeConfiguration<ReviewsOnProduct>
    {
        public void Configure(EntityTypeBuilder<ReviewsOnProduct> builder)
        {
            builder.Property(p => p.Id).UseIdentityColumn();
        }
    }
}
