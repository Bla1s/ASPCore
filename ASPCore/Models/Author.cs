using System.ComponentModel.DataAnnotations;
using ASPCore.Validators;

namespace ASPCore.Models
{
	public class Author
	{
        public int Id { get; set; }

        [Required]
        [NoNumbers]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [NoNumbers]
		[StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Surname { get; set; }

        [Required]
        [Range(1900, 2024, ErrorMessage = "Date of birth must be between 1900 and 2024")]
        public int DateOfBirth { get; set; }

        public List<Film>? Films { get; set; }

    }
}
