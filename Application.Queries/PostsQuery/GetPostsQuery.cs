using Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Queries.PostsQuery
{
    public class GetPostsQuery : IRequest<IEnumerable<Post>>
    {
        [Range(1, int.MaxValue)]
        public int Limit { get; set; } = 15;
    }
}
