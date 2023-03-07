using Zemoga_Test.Application.Common.Interfaces;

using MediatR;
using Zemoga_Test.Domain.Entities;
using Zemoga_Test.Domain.Enums;
using Zemoga_Test.Application.Comments.Queries;
using AutoMapper;
using Zemoga_Test.Application.Common.Security;

namespace Zemoga_Test.Application.Comments.Commands.CreatePost;

[Authorize(Roles ="public, writer, editor")]
public record CreateCommentCommand : IRequest<CommentDto>
{
    public int PostId { get; set; }
    public string? Content { get; set; }
}

public class CreateComentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _user;
    private readonly IMapper _mapper;
    private readonly IIdentityService _identity;

    public CreateComentCommandHandler(IApplicationDbContext context, ICurrentUserService user, IMapper mapper, IIdentityService identity)
    {
        _context = context;
        _user = user;        
        _mapper = mapper;
        _identity = identity;
    }

    public async Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {       

        var userName = await _identity.GetUserNameAsync(_user.UserId).WaitAsync(cancellationToken);

        var entity = new Comment
        {
            Author  = userName,
            Content = request.Content,
            PostId = request.PostId
        };

        _context.Comments.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CommentDto>(entity);
    }
}
