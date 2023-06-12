using Domain.Entities;
using MediatR;

namespace Application.Queries.PostQuery
{
    public class GetPostQuery : IRequest<Post>
    {
        public Guid Id { get; set; }
    }
}
