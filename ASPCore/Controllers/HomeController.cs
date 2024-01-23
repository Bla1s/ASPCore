using System.Diagnostics;
using ASPCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPCore.Controllers
{
	public class HomeController : Controller
	{
		private UserManager<AppUser> userManager;
		private FilmDbContext _context;

		public HomeController(UserManager<AppUser> userMgr, FilmDbContext context)
		{
			userManager = userMgr;
			_context = context;
		}

		/*[Authorize]*/
		public async Task<IActionResult> Index(int? authorId, Category? category, bool? ascending)
		{
			var films = _context.Films.Include(f => f.Comments).AsQueryable(); // Include the comments

			if (authorId.HasValue)
			{
				films = films.Where(f => f.AuthorId == authorId.Value);
			}

			if (category.HasValue)
			{
				films = films.Where(f => f.Category == category.Value);
			}

			var filmList = await films.ToListAsync();

			if (ascending.HasValue)
			{
				filmList = ascending.Value
					? filmList.OrderBy(f => f.AverageStarRating).ToList()
					: filmList.OrderByDescending(f => f.AverageStarRating).ToList();
			}

			var authors = await _context.Authors.ToDictionaryAsync(a => a.Id, a => a.Name + " " + a.Surname);
			ViewBag.Authors = authors;

			return View(filmList);
		}
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var film = await _context.Films.Include(f => f.Comments).FirstOrDefaultAsync(m => m.Id == id);
			if (film == null)
			{
				return NotFound();
			}

			var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == film.AuthorId);
			if (author != null)
			{
				ViewBag.Author = author;
			}

			return View(film);
		}
		public IActionResult ByAuthor(int authorId)
		{
			var films = _context.Films.Include(f => f.Comments)
				.Where(f => f.AuthorId == authorId)
				.ToList();

			return View("Index", films);
		}

		public IActionResult ByCategory(Category category)
		{
			var films = _context.Films.Include(f => f.Comments)
				.Where(f => f.Category == category)
				.ToList();

			return View("Index", films);
		}

		public IActionResult ByRating(bool ascending)
		{
			var films = ascending
				? _context.Films.OrderBy(f => f.AverageStarRating).ToList()
				: _context.Films.OrderByDescending(f => f.AverageStarRating).ToList();

			return View("Index", films);
		}
		[Authorize]
		public IActionResult AddComment(int id)
		{
			var model = new CommentViewModel { FilmId = id };
			return View(model);
		}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddComment(CommentViewModel model)
		{
			if (ModelState.IsValid)
			{
				var film = await _context.Films.Include(f => f.Comments).FirstOrDefaultAsync(f => f.Id == model.FilmId);
				if (film == null)
				{
					return NotFound();
				}

				var comment = new Comment
				{
					Username = User.Identity.Name,
					UserEmail = (await userManager.GetUserAsync(User)).Email,
					Description = model.Description,
					StarRating = model.StarRating,
					CreatedAt = DateTime.Now
				};

				if (film.Comments == null)
				{
					film.Comments = new List<Comment>();
				}

				film.Comments.Add(comment);

				await _context.SaveChangesAsync();

				return RedirectToAction("Details", new { id = model.FilmId });
			}

			return View(model);
		}
		[HttpPost]
		[Authorize] // Ensure that only logged-in users can access this action
		public async Task<IActionResult> DeleteComment(int commentId)
		{
			var comment = await _context.Comments.FindAsync(commentId);
			if (comment == null)
			{
				return NotFound();
			}

			// Check if the logged-in user is the author of the comment
			if (User.Identity.Name != comment.Username)
			{
				return Unauthorized();
			}

			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index)); // Redirect to the index action after deleting the comment
		}

	}
}
