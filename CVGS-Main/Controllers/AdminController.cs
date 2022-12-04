using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS_Main.Areas.Identity.Data;
using CVGS_Main.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using CVGS_Main.Models.View_Models;
using System.Security.Cryptography.Xml;

namespace CVGS_Main.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly CvgsDbContext _context;

        public AdminController(CvgsDbContext context)
        {
            _context = context;
        }

        // GET: CvgsGame
        public async Task<IActionResult> Index()
        {
            var games = _context.CvgsGame.Include(c => c.Developer).Include(c => c.Genre).Include(c => c.Publisher);
            var events = _context.CvgsEvent;
            var reviews = _context.CvgsReviews.Where(r => !r.IsApproved);

            CvgsAdminContainerViewModel acvm = new CvgsAdminContainerViewModel();
            acvm.Games = await games.ToListAsync();
            acvm.Events = await events.ToListAsync();
            acvm.Reviews = await reviews.ToListAsync();

            return View(acvm);
        }

        // GET: CvgsGame/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CvgsGame == null)
            {
                return NotFound();
            }

            var cvgsGame = await _context.CvgsGame
                .Include(c => c.Developer)
                .Include(c => c.Genre)
                .Include(c => c.Publisher)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (cvgsGame == null)
            {
                return NotFound();
            }

            return View(cvgsGame);
        }

        // GET: CvgsGame/Create
        public IActionResult Create()
        {
            ViewData["DeveloperId"] = new SelectList(_context.Set<CvgsDeveloper>(), "DeveloperId", "Name");
            ViewData["GenreId"] = new SelectList(_context.Set<CvgsGenre>(), "GenreId", "Type");
            ViewData["PublisherId"] = new SelectList(_context.Set<CvgsPublisher>(), "PublisherId", "Name");
            return View();
        }

        // POST: CvgsGame/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ReleaseDate,PublisherId,DeveloperId,GenreId,OverallScore")] CvgsAdminViewModel cvgsGameViewModel)
        {
            if (ModelState.IsValid)
            {

                CvgsGame cvgsGame = new CvgsGame()
                {
                    Name = cvgsGameViewModel.Name,
                    ReleaseDate = cvgsGameViewModel.ReleaseDate,
                    PublisherId = cvgsGameViewModel.PublisherId,
                    DeveloperId = cvgsGameViewModel.DeveloperId,
                    GenreId = cvgsGameViewModel.GenreId,
                    OverallScore = cvgsGameViewModel.OverallScore
                };

                _context.Add(cvgsGame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeveloperId"] = new SelectList(_context.Set<CvgsDeveloper>(), "DeveloperId", "DeveloperId", cvgsGameViewModel.DeveloperId);
            ViewData["GenreId"] = new SelectList(_context.Set<CvgsGenre>(), "GenreId", "GenreId", cvgsGameViewModel.GenreId);
            ViewData["PublisherId"] = new SelectList(_context.Set<CvgsPublisher>(), "PublisherId", "PublisherId", cvgsGameViewModel.PublisherId);
            return View(cvgsGameViewModel);
        }

        // GET: CvgsGame/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CvgsGame == null)
            {
                return NotFound();
            }

            var cvgsGame = await _context.CvgsGame.FindAsync(id);
            if (cvgsGame == null)
            {
                return NotFound();
            }

            ViewData["DeveloperId"] = new SelectList(_context.Set<CvgsDeveloper>(), "DeveloperId", "Name", cvgsGame.DeveloperId);
            ViewData["GenreId"] = new SelectList(_context.Set<CvgsGenre>(), "GenreId", "Type", cvgsGame.GenreId);
            ViewData["PublisherId"] = new SelectList(_context.Set<CvgsPublisher>(), "PublisherId", "Name", cvgsGame.PublisherId);

            CvgsAdminViewModel gvm = new CvgsAdminViewModel();
            gvm.Name = cvgsGame.Name;
            gvm.ReleaseDate = cvgsGame.ReleaseDate;
            gvm.DeveloperId = cvgsGame.DeveloperId;
            gvm.PublisherId = cvgsGame.PublisherId;
            gvm.GenreId = cvgsGame.GenreId;
            gvm.OverallScore = cvgsGame.OverallScore;
            gvm.GameId = cvgsGame.GameId;

            return View(gvm);
        }

        // POST: CvgsGame/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,Name,ReleaseDate,PublisherId,DeveloperId,GenreId,OverallScore")] CvgsAdminViewModel cvgsGameViewModel)
        {
            if (id != cvgsGameViewModel.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                CvgsGame cvgsGame = new CvgsGame()
                {
                    Name = cvgsGameViewModel.Name,
                    ReleaseDate = cvgsGameViewModel.ReleaseDate,
                    PublisherId = cvgsGameViewModel.PublisherId,
                    DeveloperId = cvgsGameViewModel.DeveloperId,
                    GenreId = cvgsGameViewModel.GenreId,
                    GameId = cvgsGameViewModel.GameId,
                   OverallScore = cvgsGameViewModel.OverallScore
                };

                try
                {
                    _context.Update(cvgsGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvgsGameExists(cvgsGame.GameId))
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
            ViewData["DeveloperId"] = new SelectList(_context.Set<CvgsDeveloper>(), "DeveloperId", "DeveloperId", cvgsGameViewModel.DeveloperId);
            ViewData["GenreId"] = new SelectList(_context.Set<CvgsGenre>(), "GenreId", "GenreId", cvgsGameViewModel.GenreId);
            ViewData["PublisherId"] = new SelectList(_context.Set<CvgsPublisher>(), "PublisherId", "PublisherId", cvgsGameViewModel.PublisherId);
            return View(cvgsGameViewModel);
        }

        // GET: CvgsGame/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CvgsGame == null)
            {
                return NotFound();
            }

            var cvgsGame = await _context.CvgsGame
                .Include(c => c.Developer)
                .Include(c => c.Genre)
                .Include(c => c.Publisher)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (cvgsGame == null)
            {
                return NotFound();
            }

            return View(cvgsGame);
        }

        // POST: CvgsGame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CvgsGame == null)
            {
                return Problem("Entity set 'CvgsDbContext.CvgsGame'  is null.");
            }
            var cvgsGame = await _context.CvgsGame.FindAsync(id);
            if (cvgsGame != null)
            {
                _context.CvgsGame.Remove(cvgsGame);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CvgsGameExists(int id)
        {
          return _context.CvgsGame.Any(e => e.GameId == id);
        }
    }
}
