using Zemoga_Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zemoga_Test.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(t => t.Content)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasMaxLength(200)
            .IsRequired();
    }
}
