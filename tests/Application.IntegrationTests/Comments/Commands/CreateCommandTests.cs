using Zemoga_Test.Application.Common.Exceptions;
using Zemoga_Test.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Zemoga_Test.Application.Comments.Commands.CreatePost;
using Zemoga_Test.Application.Posts.Commands.CreatePost;

namespace Zemoga_Test.Application.IntegrationTests.Comments.Commands;

using static Testing;

public class CreateCommandTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateCommentCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateComment()
    {
        var userId = await RunAsDefaultUserAsync();

        var post = await SendAsync(new CreatePostCommand
        {
            Title = "Title",
            Content = "Content"
        });

        var command = new CreateCommentCommand
        {
            PostId = post.Id,
            Content = "coment"
        };

        var itemId = await SendAsync(command);

        var result = await FindAsync<Comment>(itemId);

        result.Should().NotBeNull();
        result!.PostId.Should().Be(command.PostId);
        result.Content.Should().Be(command.Content);
        result.CreatedBy.Should().Be(userId);
        result.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        result.LastModifiedBy.Should().Be(userId);
        result.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
