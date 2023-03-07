using Zemoga_Test.Application.Common.Mappings;
using Zemoga_Test.Domain.Entities;

namespace Zemoga_Test.Application.Comments.Queries;

public class CommentDto : IMapFrom<Comment>
{
    public int PostId { get; set; }

    public int Id { get; set; }

    public string? Content { get; set; }

    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }
}