using Domain.Entities;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Application.Commands.PostCommand
{
    internal class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreatePostCommandHandler> _logger;
        private readonly UserCredentials _userCredentials;

        public CreatePostCommandHandler(
            IUnitOfWork unitOfWork, 
            ILogger<CreatePostCommandHandler> logger, 
            IOptions<UserCredentials> options)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userCredentials = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var authorName = _userCredentials.UserName;
            var post = new Post(id, request.Title, authorName, request.Score, request.CreatedAt);
            await _unitOfWork.PostRepository.CreatePost(post, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
            _logger.LogInformation($"Post {post.Id} has been saved to db");
            return Unit.Value;
        }
    }
}
