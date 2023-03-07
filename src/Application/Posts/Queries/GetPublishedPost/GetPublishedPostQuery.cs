using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zemoga_Test.Application.Common.Interfaces;
using Zemoga_Test.Application.Common.Security;
using Zemoga_Test.Domain.Enums;

namespace Zemoga_Test.Application.Posts.Queries.GetPosts;

[Authorize(Roles = "public,writer,editor")]
public record GetPublishedPostQuery : IRequest<PostsVm>
{
}

public class GetPublishedPostQueryHandler : IRequestHandler<GetPublishedPostQuery, PostsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;    

    public GetPublishedPostQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PostsVm> Handle(GetPublishedPostQuery request, CancellationToken cancellationToken)
    {     

        return new PostsVm
        {
            Posts = await _context.Posts.Where(x => (x.Status == PostStatus.Approved))
                     .AsNoTracking()
                     .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                     .OrderBy(t => t.Title)
                     .ToListAsync(cancellationToken)
        };

    }
}