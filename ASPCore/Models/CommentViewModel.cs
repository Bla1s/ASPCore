using System.ComponentModel.DataAnnotations;

namespace ASPCore.Models
{
	public class CommentViewModel
	{
		public int FilmId { get; set; }
		[Required]
		public string Description { get; set; }
		[Range(1, 5)]
		public int StarRating { get; set; }
	}
}
