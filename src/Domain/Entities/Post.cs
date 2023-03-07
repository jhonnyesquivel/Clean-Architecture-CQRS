namespace Zemoga_Test.Domain.Entities;
public class Post: BaseAuditableEntity
{
    public string? Title { get; set; }
    public string? Content { get; set; } 
    public IList<Comment> Comments { get; private set; } = new List<Comment>();
    public PostStatus Status { get; set; } = PostStatus.Pending;
}
