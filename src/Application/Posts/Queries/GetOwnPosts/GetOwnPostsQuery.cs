using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zemoga_Test.Application.Common.Interfaces;
using Zemoga_Test.Application.Common.Security;
using Zemoga_Test.Application.Posts.Queries.GetPosts;

namespace Zemoga_Test.Application.Posts.Queries.GetOwnPostss;

[Authorize(Roles = "writer")]
public record GetOwnPostsQuery : IRequest<PostsVm> { };

public class GetOwnPostsQueryHandler : IRequestHandler<GetOwnPostsQuery, PostsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _user;

    public GetOwnPostsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService user)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
    }

    public async Task<PostsVm> Handle(GetOwnPostsQuery request, CancellationToken cancellationToken)
    {
        return new PostsVm
        {
            Posts = await _context.Posts.Where(x => x.CreatedBy == _user.UserId)
               .AsNoTracking()
               .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
               .OrderBy(t => t.Title)
               .ToListAsync(cancellationToken)
        };
    }
}