using Zemoga_Test.Application.Common.Mappings;
using Zemoga_Test.Domain.Entities;
using Zemoga_Test.Domain.Enums;

namespace Zemoga_Test.Application.Posts.Queries;
public class PostDto : IMapFrom<Post>
{
    public PostDto()
    {
        Comments = new List<Comment>();
    }

    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Status { get; set; }

    public IList<Comment> Comments { get; set; }
}
