using Zemoga_Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Zemoga_Test.Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(t => t.Content)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(t => t.Author)
            .HasMaxLength(200)
            .IsRequired();
    }
}
