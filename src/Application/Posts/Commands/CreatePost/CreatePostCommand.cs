using Zemoga_Test.Application.Common.Interfaces;

using MediatR;
using Zemoga_Test.Domain.Entities;
using Zemoga_Test.Domain.Enums;
using Zemoga_Test.Application.Posts.Queries;
using AutoMapper;
using Zemoga_Test.Application.Common.Security;

namespace Zemoga_Test.Application.Posts.Commands.CreatePost;

[Authorize(Roles ="writer")]
public record CreatePostCommand : IRequest<PostDto>
{
    public string? Title { get; init; }
    public string? Content { get; set; }
    public PostStatus Status { get; set; } = 0;
}

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _user;
    private readonly IMapper _mapper;
    public CreatePostCommandHandler(IApplicationDbContext context, ICurrentUserService user, IMapper mapper)
    {
        _context = context;
        _user = user;        
        _mapper = mapper;
    }

    public async Task<PostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = new Post
        {
            Title = request.Title,
            Content = request.Content,
            Status = request.Status    
        };

        _context.Posts.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PostDto>(entity);
    }
}
