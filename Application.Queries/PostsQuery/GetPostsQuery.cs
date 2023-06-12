using Domain.Entities;
using MediatR;

namespace Application.Queries.PostsQuery
{
    public class GetPostsQuery : IRequest<IEnumerable<Post>>
    {
    }
}
