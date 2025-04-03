using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.DbContext;
using Rent_A_Car.Models;

namespace Rent_A_Car.Controllers
{
	public class RequestController : Controller
	{
		private readonly AppDbContext _context;
		private readonly UserManager<User> _userManager;

		public RequestController(AppDbContext context, UserManager<User> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		// GET: Request

		[Authorize]
		public async Task<IActionResult> Index()
		{
			var userId = _userManager.GetUserId(User);
			var userRequests = new List<Request>();

			if (!User.IsInRole("Admin"))
			{
				userRequests = await _context.Request
							.Where(r => r.UserId == userId)
							.ToListAsync();
			}
			else if (User.IsInRole("Admin"))
			{
				userRequests = await _context.Request
							.ToListAsync();
			}
			return View(userRequests);
		}
		// GET: Request/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var request = await _context.Request
				.Include(r => r.Car)
				.Include(r => r.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (request == null)
			{
				return NotFound();
			}

			return View(request);
		}

		// GET: Request/Create
		public IActionResult Create()
		{
			var loggedUser = _userManager.GetUserId(User);
			var currentLoggedUsers = new List<User>();
			currentLoggedUsers.Add(_context.Users.FirstOrDefault(u => u.Id == loggedUser));

			if (User.IsInRole("Admin"))
			{
				ViewData["CarId"] = new SelectList(_context.Car, "Id", "Model", "Brand", "Brand");
				ViewData["UserId"] = new SelectList(_context.Users, "Id", "EGN", "FirstName", "FirstName");
				return View();
			}
			else
			{
				ViewData["CarId"] = new SelectList(_context.Car, "Id", "Model", "Brand", "Brand");
				ViewData["UserId"] = new SelectList(currentLoggedUsers, "Id", "EGN", "FirstName", "FirstName");
				return View();
			}
		}

		// POST: Request/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,CarId,StartDate,EndDate,Id")] Request request)
		{
			var existingRequest = await _context.Request.FirstOrDefaultAsync(r => r.CarId == request.CarId);
			if (existingRequest == null)
			{
				_context.Request.Add(request);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			else
			{
				ModelState.AddModelError(string.Empty, "A request for this car already exists.");
			}

			return RedirectToAction(nameof(Index));
		}
		//ViewData["CarId"] = new SelectList(_context.Car, "Id", "Model", "Brand", "Brand");
		//ViewData["UserId"] = new SelectList(_context.Users, "Id", "EGN", "FirstName", "FirstName");
		//return View();

		// GET: Request/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var request = await _context.Request.FindAsync(id);
			if (request == null)
			{
				return NotFound();
			}
			ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", request.CarId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", request.UserId);
			return View(request);
		}

		// POST: Request/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,CarId,StartDate,EndDate,UserId,IsTaken")] Request request)
		{
			if (id != request.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(request);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!RequestExists(request.Id))
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
			ViewData["CarId"] = new SelectList(_context.Car, "Id", "Id", request.CarId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", request.UserId);
			return View(request);
		}

		// GET: Request/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var request = await _context.Request
				.Include(r => r.Car)
				.Include(r => r.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (request == null)
			{
				return NotFound();
			}

			return View(request);
		}

		// POST: Request/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var request = await _context.Request.FindAsync(id);
			if (request != null)
			{
				_context.Request.Remove(request);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool RequestExists(int id)
		{
			return _context.Request.Any(e => e.Id == id);
		}
	}
}
