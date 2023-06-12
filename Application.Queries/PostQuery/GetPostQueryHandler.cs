using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.PostQuery
{
    internal class GetPostQueryHandler : IRequestHandler<GetPostQuery, Post>
    {
        private readonly IPostRepository _postRepository;
        private readonly ICachingService _cachingService;

        public GetPostQueryHandler(
            IPostRepository postRepository,
            ICachingService cachingService)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _cachingService = cachingService ?? throw new ArgumentNullException(nameof(cachingService));
        }

        public Task<Post> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
