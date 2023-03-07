using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zemoga_Test.Application.Common.Interfaces;
using Zemoga_Test.Application.Common.Security;
using Zemoga_Test.Domain.Enums;

namespace Zemoga_Test.Application.Posts.Queries.GetPosts;

[Authorize(Roles = "editor")]
public record GetPendingPostsQuery : IRequest<PostsVm>
{
}

public class GetPendingPostsQueryHandler : IRequestHandler<GetPendingPostsQuery, PostsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPendingPostsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PostsVm> Handle(GetPendingPostsQuery request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        return new PostsVm
        {
            Posts = await _context.Posts.Where(x => (x.Status == PostStatus.Pending))
                     .AsNoTracking()
                     .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                     .OrderBy(t => t.Title)
                     .ToListAsync(cancellationToken)
        };
    }
}