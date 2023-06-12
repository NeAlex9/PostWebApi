using Domain.Interfaces;
using MediatR;

namespace Application.Commands.PostCommand
{
    internal class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Unit>
    {
        private readonly IPostRepository _postRepository;

        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        }

        public Task<Unit> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
