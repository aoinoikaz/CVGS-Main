using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS_Main.Areas.Identity.Data;
using CVGS_Main.Models;
using Microsoft.AspNetCore.Identity;

namespace CVGS_Main.Controllers
{
    public class ReviewController : Controller
    {
        private readonly CvgsDbContext _context;
        private readonly UserManager<CvgsUser> _userManager;


        public ReviewController(CvgsDbContext context, UserManager<CvgsUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Review
        public async Task<IActionResult> Index()
        {
            // pass in game id through the index 

            // 
              return View(await _context.CvgsReviews.ToListAsync());
            // 
        }

        // GET: Review/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CvgsReviews == null)
            {
                return NotFound();
            }

            var cvgsReview = await _context.CvgsReviews
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (cvgsReview == null)
            {
                return NotFound();
            }

            return View(cvgsReview);
        }

        // GET: Review/Create
        public IActionResult Create(int? id)
        {
            if (id != null)
            {
                ViewData["gameId"] = id;
            }

            return View();
        }

        // POST: Review/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReview([Bind("GameId, Description")] CvgsReview input)
        {
            // assign userid and game id to new review
            // add review to table
            // save 

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {

                CvgsReview newReview = new CvgsReview();
                newReview.GameId = input.GameId; // retrieve game id some how
                newReview.UserId = user.Id;
                newReview.Description = input.Description;

                _context.CvgsReviews.Add(newReview);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Store", new { id=input.GameId});
            }

            return RedirectToAction("Details", "Store", new { id = input.GameId });

        }

        public async Task<IActionResult> ApproveReview(int? id)
        {

            var review = _context.CvgsReviews.Find(id);

            if (review != null)
            {

                review.IsApproved = true;

                _context.CvgsReviews.Update(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Admin");
        }


        // GET: Review/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CvgsReviews == null)
            {
                return NotFound();
            }

            var cvgsReview = await _context.CvgsReviews.FindAsync(id);
            if (cvgsReview == null)
            {
                return NotFound();
            }
            return View(cvgsReview);
        }

        // POST: Review/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReviewId,GameId,UserId,Description,IsApproved")] CvgsReview cvgsReview)
        {
            if (id != cvgsReview.ReviewId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cvgsReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvgsReviewExists(cvgsReview.ReviewId))
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
            return View(cvgsReview);
        }

        // GET: Review/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CvgsReviews == null)
            {
                return NotFound();
            }

            var cvgsReview = await _context.CvgsReviews
                .FirstOrDefaultAsync(m => m.ReviewId == id);
            if (cvgsReview == null)
            {
                return NotFound();
            }

            return View(cvgsReview);
        }

        // POST: Review/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CvgsReviews == null)
            {
                return Problem("Entity set 'CvgsDbContext.CvgsReview'  is null.");
            }
            var cvgsReview = await _context.CvgsReviews.FindAsync(id);
            if (cvgsReview != null)
            {
                _context.CvgsReviews.Remove(cvgsReview);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CvgsReviewExists(int id)
        {
          return _context.CvgsReviews.Any(e => e.ReviewId == id);
        }
    }
}
