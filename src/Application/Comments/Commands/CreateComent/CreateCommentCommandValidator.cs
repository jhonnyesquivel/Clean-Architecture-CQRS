using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zemoga_Test.Application.Comments.Commands.CreatePost;
using Zemoga_Test.Application.Common.Interfaces;

namespace Zemoga_Test.Application.Comments.Commands.CreateCommentCommandValidator;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCommentCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(2000).WithMessage($"Title must not exceed 2000 characters.");

        RuleFor(v => v.PostId)
            .GreaterThan(0).WithMessage("Must have a Post Id")
            .MustAsync(CanComment).WithMessage("Is not possible comment this post");
    }


    public async Task<bool> CanComment(int postId, CancellationToken cancellationToken)
    {
        var existing = await _context.Posts.FindAsync(new object[] { postId }, cancellationToken);

        if(existing.Status != Domain.Enums.PostStatus.Approved)
        {
            return false;
        }
        return true;
    }

}