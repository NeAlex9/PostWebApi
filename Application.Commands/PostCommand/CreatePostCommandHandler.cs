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

        public Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
