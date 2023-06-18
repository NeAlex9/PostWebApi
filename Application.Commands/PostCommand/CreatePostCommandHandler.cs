using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Commands.PostCommand
{
    internal class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreatePostCommandHandler> _logger;

        public CreatePostCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var post = new Post(id, request.Title, request.AuthorName, request.Score, request.CreatedAt);
            await _unitOfWork.PostRepository.CreatePost(post, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
            _logger.LogInformation($"Post {post.Id} has been saved to db");
            return Unit.Value;
        }
    }
}
