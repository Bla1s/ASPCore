using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPCore.Models
{
	public class Film
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		public int AuthorId { get; set; }
		[Required]
		public int ReleaseDate { get; set; }
		[Required]
		public Category Category { get; set; }
		[Required]
		public string Description { get; set; }
		[Range(1, 5)]
		public double StarRating { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
		public List<Comment>? Comments { get; set; }
	}

	public enum Category
	{
		Action,
		Comedy,
		Drama,
		Horror,
		Thriller,
		SciFi,
		Fantasy,
		Adventure,
		Romance,
		Animation,
		Cartoon,
		Anime,
		Other
	}
}
