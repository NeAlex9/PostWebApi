using MediatR;

namespace Application.Commands.PostCommand
{
    public class CreatePostCommand : IRequest<Unit>
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int Score { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
