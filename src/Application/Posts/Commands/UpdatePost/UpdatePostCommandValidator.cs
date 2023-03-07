using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zemoga_Test.Application.Common.Interfaces;
using Zemoga_Test.Application.Posts.Commands.CreatePost;
using Zemoga_Test.Domain.Enums;

namespace Zemoga_Test.Application.Posts.Commands.CreatePostCommandValidator;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identity;
    private readonly ICurrentUserService _user;

    public UpdatePostCommandValidator(IApplicationDbContext context, IIdentityService identity, ICurrentUserService user)
    {
        _context = context;
        _identity = identity;
        _user = user;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(v => v).
            MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");

        RuleFor(v => v.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(2000).WithMessage($"Title must not exceed 2000 characters.");

        RuleFor(v => v.Status)
            .NotNull().WithMessage("Status is required.")
            .IsInEnum().WithMessage("Must be a valid status")
            .MustAsync(async (x, y, c, d) => await CanBeUpdated(x, d))
            .WithMessage("Status selected is not valid for your role");

        RuleFor(v => v.EditorComment)           
           .MaximumLength(200).WithMessage($"Comment must not exceed 2000 characters.");
    }

    public async Task<bool> CanBeUpdated(UpdatePostCommand furure, CancellationToken cancellationToken)
    {
        var userRole = await _identity.GetRolesAsync(_user.UserId);
        var writerValidEditPostStatus = new PostStatus[] { PostStatus.Draft, PostStatus.Rejected };
        var editorValidEditPostStatus = new PostStatus[] { PostStatus.Pending };

        var editorValidPostStatus = new PostStatus[] { PostStatus.Pending, PostStatus.Approved, PostStatus.Rejected };
        var writerValidPostStatus = new PostStatus[] { PostStatus.Draft, PostStatus.Pending };

        var existing = await _context.Posts.FindAsync(new object[] { furure.Id }, cancellationToken);

        if (userRole.Contains(Role.Writer) &&
            writerValidEditPostStatus.Contains(existing.Status) &&
            writerValidPostStatus.Contains(furure.Status))
        {
            return true;
        }
        else if (userRole.Contains(Role.Editor) &&
            editorValidEditPostStatus.Contains(existing.Status) &&
            editorValidPostStatus.Contains(furure.Status))
        {
            return true;
        }

        return false;
    }

    public async Task<bool> BeUniqueTitle(UpdatePostCommand furure, CancellationToken cancellationToken)
    {
        //return await _context.Posts
        //    .AllAsync(l => l.Title != furure.Title && l.Id != furure.Id, cancellationToken);
        return true;
    }
}