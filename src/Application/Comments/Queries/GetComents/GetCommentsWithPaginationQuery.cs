using System.Security.Principal;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zemoga_Test.Application.Common.Interfaces;
using Zemoga_Test.Application.Common.Mappings;
using Zemoga_Test.Application.Common.Models;
using Zemoga_Test.Application.Common.Security;
using Zemoga_Test.Domain.Entities;

namespace Zemoga_Test.Application.Comments.Queries.GetOwnCommentss;

[Authorize(Roles = "public, writer, editor")]
public record GetCommentsWithPaginationQuery : IRequest<PaginatedList<CommentDto>>
{
    public int PostId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCommentsWithPaginationQueryHandler : IRequestHandler<GetCommentsWithPaginationQuery, PaginatedList<CommentDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCommentsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CommentDto>> Handle(GetCommentsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Comments
            .Where(x => x.PostId == request.PostId)            
            .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}