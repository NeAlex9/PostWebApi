using Application.Queries.Models;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.Queries.PostQuery
{
    internal class GetPostQueryHandler : IRequestHandler<GetPostQuery, Post?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICachingService<Post> _cachingService;
        private readonly CachingOptions _cachingOptions;

        public GetPostQueryHandler(
            IUnitOfWork unitOfWork,
            ICachingService<Post> cachingService,
            IOptions<CachingOptions> options)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _cachingService = cachingService ?? throw new ArgumentNullException(nameof(cachingService));
            _cachingOptions = options.Value ?? throw new ArgumentNullException();
        }

        public async Task<Post?> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            if (_cachingService.TryGetValue(request.Id, out Post cachedPost))
            {
                return cachedPost;
            }

            var post = await _unitOfWork.PostRepository.GetPostAsync(request.Id, cancellationToken);
            if (post != null)
            {
                _cachingService.Set(request.Id, DateTime.UtcNow.AddSeconds(_cachingOptions.ValidInSeconds), post);
            }

            return post;
        }
    }
}
