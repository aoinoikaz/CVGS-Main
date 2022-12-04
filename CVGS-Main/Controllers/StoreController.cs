using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS_Main.Areas.Identity.Data;
using CVGS_Main.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Identity;
using CVGS_Main.Areas.Identity.Pages.Account.Manage;
using CVGS_Main.Models.View_Models;

namespace CVGS_Main.Controllers
{
    public class StoreController : Controller
    {
        private readonly CvgsDbContext _context;

        public StoreController(CvgsDbContext context)
        {
            _context = context;
        }


        // GET: Store
        public async Task<IActionResult> Index(string searchQuery)
        {

            ViewData["searchQuery"] = searchQuery;

            var games = from g in _context.CvgsGame.
                        Include(d => d.Developer).
                        Include(g => g.Genre).
                        Include(p => p.Publisher)
                        select g;


            if (!string.IsNullOrEmpty(searchQuery))
            {
                games = games.Where(g => g.Name.Contains(searchQuery) ||
                    g.Developer.Name.Contains(searchQuery) ||
                    g.Publisher.Name.Contains(searchQuery) ||
                    g.Genre.Type.Contains(searchQuery));
            }

            return View(await games.ToListAsync());
        }


        // GET: Store/Details/5
        public async Task<IActionResult> Rate(int? id)
        {
            if (id == null || _context.CvgsGame == null)
            {
                return NotFound();
            }

            var game = _context.CvgsGame.Where(g => g.GameId == id.Value).FirstOrDefault();

            if (game != null)
            {
                game.OverallScore++;

                _context.CvgsGame.Update(game);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Store");
        }

        // add review to game 
        // text box for review, 


        // GET: Store/Details/5
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

            CvgsGameReviewViewModel model = new CvgsGameReviewViewModel();

            model.Game = cvgsGame;

            var reviews =  _context.CvgsReviews.Where(r => r.GameId == cvgsGame.GameId).ToList();

            if (reviews != null)
            {
                model.Reviews = reviews;
            }

            return View(model);
        }
    }
}
