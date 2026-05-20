using Microsoft.AspNetCore.Mvc;
using okumatakibisanalkutuphane.Data;
using okumatakibisanalkutuphane.Models;
using okumatakibisanalkutuphane.Services;
using Microsoft.EntityFrameworkCore;

namespace okumatakibisanalkutuphane.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ExportService _exportService;

        public BooksController(AppDbContext context, ExportService exportService)
        {
            _context = context;
            _exportService = exportService;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            var book = await _context.Books
                .Include(b => b.Sessions)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            // Nullable referans tipleri yüzünden opsiyonel alanlarda hata fırlatmasını engelle
            ModelState.Remove("Genre");
            ModelState.Remove("ImageUrl");
            ModelState.Remove("Sessions");
            ModelState.Remove("Status");
            
            if (ModelState.IsValid)
            {
                // Veritabanında NOT NULL oldukları için null gelen değerleri boş string yap
                book.Genre = book.Genre ?? "";
                book.ImageUrl = book.ImageUrl ?? "";

                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id) return NotFound();

            ModelState.Remove("Genre");
            ModelState.Remove("ImageUrl");
            ModelState.Remove("Sessions");
            ModelState.Remove("Status");

            if (ModelState.IsValid)
            {
                try
                {
                    book.Genre = book.Genre ?? "";
                    book.ImageUrl = book.ImageUrl ?? "";

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Books.Any(e => e.Id == book.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportExcel()
        {
            var books = await _context.Books.ToListAsync();
            var content = _exportService.GenerateExcel(books);
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Kitap_Listesi.xlsx");
        }

        public async Task<IActionResult> ExportPdf()
        {
            var books = await _context.Books.ToListAsync();
            var content = _exportService.GeneratePdf(books);
            return File(content, "application/pdf", "Kitap_Listesi.pdf");
        }
    }
}
