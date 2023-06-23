using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Application.Queries.PostsQuery
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<Post>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRetrivalClient _postRetrivalClient;
        private readonly ILogger _logger;

        public GetPostsQueryHandler(
            IUnitOfWork unitOfWork,
            IPostRetrivalClient postRetrivalClient,
            ILogger<GetPostsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _postRetrivalClient = postRetrivalClient ?? throw new ArgumentNullException(nameof(postRetrivalClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Post>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.PostRepository.GetPostsAsync(request.Limit, cancellationToken);
            if (!posts.Any())
            {
                var postsFromApi = (await _postRetrivalClient.GetPostsAsync(request.Limit, cancellationToken)).ToList();
                _logger.LogInformation("Posts has been retrieved from reddit api");
                if (postsFromApi.Any())
                {
                    await _unitOfWork.PostRepository.CreatePosts(postsFromApi, cancellationToken);
                    await _unitOfWork.SaveChanges(cancellationToken);
                    _logger.LogInformation("Posts has been saved to db");
                }

                return postsFromApi;
            }

            _logger.LogInformation("Posts has been retrieved from db");
            return posts;
        }
    }
}
