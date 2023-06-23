using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.PostCommand
{
    public class CreatePostCommand : IRequest<Unit>
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public int Score { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
