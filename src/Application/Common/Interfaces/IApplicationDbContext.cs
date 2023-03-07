using Zemoga_Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Zemoga_Test.Application.Common.Interfaces;

public interface IApplicationDbContext
{   
    DbSet<Post> Posts { get; }

    DbSet<Comment> Comments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
