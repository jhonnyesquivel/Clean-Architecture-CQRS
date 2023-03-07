using FluentValidation;

namespace Zemoga_Test.Application.Comments.Queries.GetOwnCommentss;

public class GetCommentsWithPaginationQueryValidator : AbstractValidator<GetCommentsWithPaginationQuery>
{
    public GetCommentsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty().WithMessage("PostId is required.");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}