using Application.Queries.Models;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Application.Queries.PostQuery
{
    internal class GetPostQueryHandler : IRequestHandler<GetPostQuery, Post?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICachingService _cachingService;
        private readonly ILogger<GetPostQueryHandler> _logger;
        private readonly PostCachingOptions _cachingOptions;

        public GetPostQueryHandler(
            IUnitOfWork unitOfWork,
            ICachingService cachingService,
            IOptions<PostCachingOptions> options,
            ILogger<GetPostQueryHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _cachingService = cachingService ?? throw new ArgumentNullException(nameof(cachingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _cachingOptions = options.Value ?? throw new ArgumentNullException();
        }

        public async Task<Post?> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            if (_cachingService.TryGetValue(request.Id, out Post cachedPost))
            {
                _logger.LogInformation($"Cached post {request.Id} has been retrieved");
                return cachedPost;
            }

            var post = await _unitOfWork.PostRepository.GetPostAsync(request.Id, cancellationToken);
            if (post is not null)
            {
                _logger.LogInformation($"Post {request.Id} has been cached");
                var validUntil = DateTime.UtcNow.AddSeconds(_cachingOptions.ValidInSeconds);
                _cachingService.Set(request.Id, validUntil, post);
            }

            _logger.LogInformation(post is not null 
                ? $"Post {request.Id} has been retrieved from db"
                : $"Failed to get post {request.Id}");
            return post;
        }
    }
}
