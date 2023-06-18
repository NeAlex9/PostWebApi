using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.PostsQuery
{
    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<Post>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRetrivalClient _postRetrivalClient;

        public GetPostsQueryHandler(
            IUnitOfWork unitOfWork, 
            IPostRetrivalClient postRetrivalClient)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _postRetrivalClient = postRetrivalClient ?? throw new ArgumentNullException(nameof(postRetrivalClient));
        }

        public async Task<IEnumerable<Post>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.PostRepository.GetPostsAsync(cancellationToken);
            if (!posts.Any())
            {
                var postsFromApi = (await _postRetrivalClient.GetPostsAsync(cancellationToken)).ToList();
                if (postsFromApi.Any())
                {
                    await _unitOfWork.PostRepository.CreatePosts(postsFromApi, cancellationToken);
                    await _unitOfWork.SaveChanges(cancellationToken);
                }

                return postsFromApi;
            }

            return posts;
        }
    }
}
