using Zemoga_Test.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Zemoga_Test.Application.Posts.Commands.CreatePost;
using Zemoga_Test.Domain.Enums;

namespace Zemoga_Test.Application.Posts.Commands.CreatePostCommandValidator;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    private readonly IApplicationDbContext _context;

    public CreatePostCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");

        RuleFor(v => v.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(2000).WithMessage($"Title must not exceed 2000 characters.");

        RuleFor(v => v.Status)
            .NotNull().WithMessage("Status is required.")
            .IsInEnum().WithMessage("Must be a valid status [Draft, Pending].")
            .Must(x => new[] { PostStatus.Draft, PostStatus.Pending }.Contains(x))
            .WithMessage("Must be a valid status [Draft, Pending].");



    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}
