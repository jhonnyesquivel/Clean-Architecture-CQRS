using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zemoga_Test.Application.Common.Interfaces;
using Zemoga_Test.Application.Posts.Commands.CreatePost;
using Zemoga_Test.Application.Posts.Queries;
using Zemoga_Test.Application.Posts.Queries.GetOwnPostss;
using Zemoga_Test.Application.Posts.Queries.GetPosts;
using Zemoga_Test.Domain.Entities;

namespace Zemoga_Test.WebAPI.Controllers;

[Authorize]
public class PostsController : ApiControllerBase
{
    

    [HttpGet]
    public async Task<ActionResult<PostsVm>> Get()
    {
        return await Mediator.Send(new GetPublishedPostQuery());
    }

    [HttpPost]
    public async Task<PostDto> Create(CreatePostCommand command)
    {
        var post = await Mediator.Send(command);
        return post;
    }

    [HttpGet]
    [Route("mine")]
    public async Task<ActionResult<PostsVm>> GetOwnedPosts()
    {
        return await Mediator.Send(new GetOwnPostsQuery());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdatePostCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        var result = await Mediator.Send(command);

        return Ok(result);
    }

}
