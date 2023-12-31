﻿using Application.Commands.PostCommand;
using Application.Queries.PostQuery;
using Application.Queries.PostsQuery;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace PostsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPostAsync([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            var command = new GetPostQuery()
            {
                Id = id
            };
            var post = await _mediator.Send(command, cancellationToken);
            return post is not null
                ? Ok(post)
                : NotFound();
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Post>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPostsAsync([FromQuery]GetPostsQuery command, CancellationToken cancellationToken)
        {
            var posts = await _mediator.Send(command, cancellationToken);
            return posts.Any()
                ? Ok(posts)
                : NotFound();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePostAsync(CreatePostCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);
            return Ok();
        }
    }
}
