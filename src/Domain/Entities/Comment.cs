namespace Zemoga_Test.Domain.Entities;
public class Comment : BaseAuditableEntity
{
    public int PostId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;

}
