using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.PostCommand
{
    internal class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePostCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var post = new Post(id, request.Title, request.AuthorName, request.Score, request.CreatedAt);
            await _unitOfWork.PostRepository.CreatePost(post, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);
            return Unit.Value;
        }
    }
}
