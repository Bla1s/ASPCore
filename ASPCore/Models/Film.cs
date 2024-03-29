﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ASPCore.Validators;

namespace ASPCore.Models
{
	public class Film
	{
		public int Id { get; set; }
        [Required]
		[NoNumbers]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        public int AuthorId { get; set; }

        [Required]
        [Range(1900, 2099)]
        public int ReleaseDate { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public string ImagePath { get; set; }
        public List<Comment>? Comments { get; set; }
        [NotMapped] // This attribute means the property will not be stored in the database
		public double AverageStarRating
		{
			get
			{
				if (Comments == null || Comments.Count == 0)
				{
					return 0;
				}

				return Comments.Average(c => c.StarRating);
			}
		}
	}


	public enum Category
	{
		Action,
		Comedy,
		Horror,
		SciFi,
		Cartoon,
	}
}
