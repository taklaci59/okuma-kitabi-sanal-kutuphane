using Microsoft.AspNetCore.Mvc;
using okumatakibisanalkutuphane.Data;
using okumatakibisanalkutuphane.Models;
using Microsoft.EntityFrameworkCore;

namespace okumatakibisanalkutuphane.Controllers
{
    public class ReadingController : Controller
    {
        private readonly AppDbContext _context;

        public ReadingController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> LogSession(int bookId, int startPage, int endPage, int duration)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return NotFound();

            var session = new ReadingSession
            {
                BookId = bookId,
                Date = DateTime.Now,
                StartPage = startPage,
                EndPage = endPage,
                DurationMinutes = duration
            };

            _context.ReadingSessions.Add(session);
            
            // Update book progress
            book.CurrentPage = endPage;
            if (book.CurrentPage >= book.TotalPages)
            {
                book.CurrentPage = book.TotalPages;
                book.Status = "Finished";
            }
            else
            {
                book.Status = "Reading";
            }
            _context.Update(book);

            // Update Goals progress
            var activeGoals = await _context.Goals
                .Where(g => g.StartDate <= DateTime.Today && g.EndDate >= DateTime.Today)
                .ToListAsync();

            foreach (var goal in activeGoals)
            {
                goal.CurrentProgress += session.PagesRead;
                _context.Update(goal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Books", new { id = bookId });
        }

        [HttpGet]
        public IActionResult CreateGoal()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal(Goal goal)
        {
            ModelState.Remove("Description");

            if (ModelState.IsValid)
            {
                // Boş gelirse null atayalım
                goal.Description = goal.Description ?? "";
                _context.Goals.Add(goal);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            
            TempData["Error"] = "Hedef oluşturulamadı. Lütfen bilgileri kontrol edin.";
            return RedirectToAction("Index", "Home");
        }
    }
}
