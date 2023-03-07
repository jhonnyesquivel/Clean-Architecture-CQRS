using Zemoga_Test.Application.Common.Exceptions;
using Zemoga_Test.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Zemoga_Test.Application.Posts.Commands.CreatePost;

namespace Zemoga_Test.Application.IntegrationTests.TodoLists.Commands;

using static Testing;

public class CreatePostTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreatePostCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireUniqueTitle()
    {
        await SendAsync(new CreatePostCommand
        {
            Title = "Shopping",
            Content = "Shopping Shopping Shopping"
        });

        var command = new CreatePostCommand
        {
            Title = "Shopping",
            Content = "Shopping Shopping Shopping"
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateTodoList()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreatePostCommand
        {
            Title = "Tasks",
             Content = "Shopping Shopping Shopping"
        };

        var id = await SendAsync(command);

        var list = await FindAsync<Post>(id);

        list.Should().NotBeNull();
        list!.Title.Should().Be(command.Title);
        list.CreatedBy.Should().Be(userId);
        list.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
