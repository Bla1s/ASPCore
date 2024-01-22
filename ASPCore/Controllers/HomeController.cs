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
		public async Task<IActionResult> Index()
		{
			var films = await _context.Films.ToListAsync();
			var authors = await _context.Authors.ToDictionaryAsync(a => a.Id, a => a.Name);

			ViewBag.Authors = authors;

			return View(films);
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
		public async Task<IActionResult> DeleteComment(int id)
		{
			var comment = await _context.Comments.FindAsync(id);
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
