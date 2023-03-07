using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zemoga_Test.Application.Comments.Commands.CreatePost;
using Zemoga_Test.Application.Comments.Queries;
using Zemoga_Test.Application.Comments.Queries.GetOwnCommentss;
using Zemoga_Test.Application.Common.Models;

namespace Zemoga_Test.WebAPI.Controllers;

[Authorize]
public class CommentsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<CommentDto>>> GetTodoItemsWithPagination([FromQuery] GetCommentsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<CommentDto> Create(CreateCommentCommand command)
    {
        var post = await Mediator.Send(command);
        return post;
    }
}