using System.ComponentModel.DataAnnotations;

namespace MembershipSystem.Api.Models
{
    public class NewMember
    {
        [Required]
        [StringLength(16)]
        public string CardId { get; set; }
        [Required]
        public string MemberId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Mobile { get; set; }
        [Required]
        [StringLength(4,MinimumLength =4)]
        public string Pin { get; set; }

    }
}
