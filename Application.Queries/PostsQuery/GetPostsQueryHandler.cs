using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.PostsQuery
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<Post>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostRetrivalClient _postRetrivalClient;

        public GetPostsQueryHandler(
            IPostRepository postRepository, 
            IPostRetrivalClient postRetrivalClient)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _postRetrivalClient = postRetrivalClient ?? throw new ArgumentNullException(nameof(postRetrivalClient));
        }

        public Task<IEnumerable<Post>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
