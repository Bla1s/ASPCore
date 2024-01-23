using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ASPCore.Models
{
    public class FilmViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        public int AuthorId { get; set; }

        [Required]
        [Range(1900, 2024)]
        public int ReleaseDate { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }

}
