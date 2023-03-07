using System.Data;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Zemoga_Test.Application.Common.Exceptions;
using Zemoga_Test.Application.Common.Interfaces;
using Zemoga_Test.Application.Common.Security;
using Zemoga_Test.Application.Posts.Queries;
using Zemoga_Test.Domain.Entities;
using Zemoga_Test.Domain.Enums;

namespace Zemoga_Test.Application.Posts.Commands.CreatePost;

[Authorize(Roles = "writer,editor")]
public record UpdatePostCommand : IRequest<PostDto>
{
    public int Id { get; set; }
    public string? Title { get; init; }
    public string? Content { get; set; }
    public string? EditorComment { get; set; }
    public PostStatus Status { get; set; } = 0;
}

public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _user;
    private readonly IMapper _mapper;

    public UpdatePostCommandHandler(IApplicationDbContext context, ICurrentUserService user, IMapper mapper)
    {
        _context = context;
        _user = user;
        _mapper = mapper;
    }

    public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Posts.FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Post), request.Id);
        }

        entity.Content = request.Content;
        entity.Title = request.Title;
        entity.Status = request.Status;

        if (request?.EditorComment != null)
        {
            entity.Comments.Add(new Comment { PostId= entity.Id, Content = request!.EditorComment });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PostDto>(entity);
    }
}