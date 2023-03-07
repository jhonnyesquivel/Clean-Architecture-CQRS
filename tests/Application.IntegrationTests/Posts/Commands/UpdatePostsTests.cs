using Zemoga_Test.Application.Common.Exceptions;
using Zemoga_Test.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Zemoga_Test.Application.Posts.Commands.CreatePost;

namespace Zemoga_Test.Application.IntegrationTests.Todopostitems.Commands;

using static Testing;

public class UpdatePostsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodopostitemId()
    {
        var command = new UpdatePostCommand { Id = 99, Title = "New Title", Content = "Shopping Shopping Shopping" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        var postitemId = await SendAsync(new CreatePostCommand
        {
            Title = "New postitem"
        });

        await SendAsync(new CreatePostCommand
        {
            Title = "Other postitem"
        });

        var command = new UpdatePostCommand
        {
            Id = postitemId.Id,
            Title = "Other postitem"
        };

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Title")))
                .And.Errors["Title"].Should().Contain("The specified title already exists.");
    }

    [Test]
    public async Task ShouldUpdateTodopostitem()
    {
        var userId = await RunAsDefaultUserAsync();

        var postitemId = await SendAsync(new CreatePostCommand
        {
            Title = "New postitem"
        });

        var command = new UpdatePostCommand
        {
            Id = postitemId.Id,
            Title = "Updated postitem Title"
        };

        await SendAsync(command);

        var postitem = await FindAsync<Post>(postitemId);

        postitem.Should().NotBeNull();
        postitem!.Title.Should().Be(command.Title);
        postitem.LastModifiedBy.Should().NotBeNull();
        postitem.LastModifiedBy.Should().Be(userId);
        postitem.LastModified.Should().NotBeNull();
        postitem.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
