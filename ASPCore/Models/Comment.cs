using System.ComponentModel.DataAnnotations;

namespace ASPCore.Models
{
	public class Comment
	{
		public int Id { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		public string UserEmail { get; set; }
		[Required]
		public string Description { get; set; }
		[Range(1, 5)]
		public int StarRating { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
