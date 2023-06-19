using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.PostCommand
{
    public class CreatePostCommand : IRequest<Unit>
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string AuthorName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Score { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
