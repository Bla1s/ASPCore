using ASPCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPCore.Controllers
{

	public class FilmController : Controller
	{
		private readonly FilmDbContext _context;

		public FilmController(FilmDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var films = await _context.Films.ToListAsync();
			var authors = await _context.Authors.ToDictionaryAsync(a => a.Id, a => a.Name);

			ViewBag.Authors = authors;

			return View(films);
		}
		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Authors = new SelectList(_context.Authors, "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Film film)
		{
			if (ModelState.IsValid)
			{
				var author = await _context.Authors.FindAsync(film.AuthorId);
				if (author == null)
				{
					ModelState.AddModelError("", "Author not found");
					return View(film);
				}
				if (film.Image != null)
				{
					var fileName = Path.GetFileName(film.Image.FileName);
					var filePath = Path.Combine("wwwroot/images", fileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						await film.Image.CopyToAsync(fileStream);
					}
				}

				Film newFilm = new Film
				{
					Name = film.Name,
					AuthorId = film.AuthorId,
					ReleaseDate = film.ReleaseDate,
					Category = film.Category,
					Description = film.Description,
					StarRating = film.StarRating,
					Image = film.Image
				};
				if (author.Films == null)
                {
                    author.Films = new List<Film>();
                }
                author.Films.Add(newFilm);

                _context.Films.Add(newFilm);
                _context.Authors.Update(author);
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

			return View(film);
		}
		[HttpGet]
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var film = await _context.Films.FindAsync(id);
			if (film == null)
			{
				return NotFound();
			}

			return View(film);
		}

		// Update action (POST)
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, Film film)
		{
			if (id != film.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(film);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!FilmExists(film.Id))
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
			return View(film);
		}

		// Delete action (POST)
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var film = await _context.Films.FindAsync(id);
			_context.Films.Remove(film);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		private bool FilmExists(int id)
		{
			return _context.Films.Any(e => e.Id == id);
		}


	}
}
