using ASPCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPCore.Controllers
{
	public class AuthorController : Controller
	{
		private readonly FilmDbContext _context;

		public AuthorController(FilmDbContext context)
		{
			_context = context;
		}

        // Index action to view all authors
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var authorsWithFilms = await _context.Authors.Include(a => a.Films).ToListAsync();
            return View(authorsWithFilms);
        }

        // Create action (GET)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
		{
			return View();
		}

		// Create action (POST)
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Author author)
		{
			if (ModelState.IsValid)
			{
				Author newAuthor = new Author
				{
					Name = author.Name,
					Surname = author.Surname,
					DateOfBirth = author.DateOfBirth,
				};

				_context.Authors.Add(newAuthor);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			// Log ModelState errors
			foreach (var modelState in ModelState.Values)
			{
				foreach (var error in modelState.Errors)
				{
					Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
				}
			}

			return View(author);
		}
		[HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var author = await _context.Authors.FindAsync(id);
			if (author == null)
			{
				return NotFound();
			}

			return View(author);
		}

		// Update action (POST)
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Author author)
		{
			if (id != author.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(author);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AuthorExists(author.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(author);
		}

		// Delete action (POST)
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
		{
			var author = await _context.Authors.FindAsync(id);
			_context.Authors.Remove(author);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool AuthorExists(int id)
		{
			return _context.Authors.Any(e => e.Id == id);
		}

	}
}
