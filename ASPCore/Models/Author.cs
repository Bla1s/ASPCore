using System.ComponentModel.DataAnnotations;

namespace ASPCore.Models
{
	public class Author
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Surname { get; set; }
		[Required]
		public string DateOfBirth { get; set; }
		[Required]
		public string Description { get; set; }
		public List<Film>? Films { get; set; }

	}
}
