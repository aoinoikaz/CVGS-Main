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
using CVGS_Main.Areas.Identity.Pages.Account.Manage;
using Microsoft.AspNetCore.Authorization;

namespace CVGS_Main.Controllers
{
    [Authorize]
    public class WishlistItemsController : Controller
    {
        private readonly CvgsDbContext _context;
        private readonly UserManager<CvgsUser> _userManager;

        public WishlistItemsController(CvgsDbContext context, UserManager<CvgsUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: WishlistItems
        public async Task<IActionResult> Index(string? id)
        {
            var myself = await _userManager.GetUserAsync(User);

            if (id == null)
            {
                TempData["WishlistUser"] = "Your wishlist";
                ViewData["Ownership"] = true;
                var wishList = _context.CvgsWishlistItems.Where(w => w.UserId == myself.Id);
                return View(await wishList.ToListAsync());
            }
            else
            {
                var friend = await _userManager.FindByIdAsync(id);
                TempData["WishlistUser"] = friend.UserName + "'s wishlist";
                ViewData["Ownership"] = false;
                var wishList = _context.CvgsWishlistItems.Where(w => w.UserId == id);
                return View(await wishList.ToListAsync());
            }
          
        }

        // GET: WishlistItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CvgsWishlistItems == null)
            {
                return NotFound();
            }

            var cvgsWishlistItems = await _context.CvgsWishlistItems
                .FirstOrDefaultAsync(m => m.WishlistItemId == id);

            if (cvgsWishlistItems == null)
            {
                return NotFound();
            }

            return View(cvgsWishlistItems);
        }
       

        public async Task<IActionResult> Create(int? id)
        {

            if (id == null || _context.CvgsGame == null)
            {
                return NotFound();
            }

            CvgsWishlistItems items = new CvgsWishlistItems();
            var user = await _userManager.GetUserAsync(User);
            items.UserId = user.Id;
            items.GameId = id.Value;

            // lookup table with game id and user id

            // games = context.games.find(gameid)

            _context.Add(items);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: WishlistItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CvgsWishlistItems == null)
            {
                return NotFound();
            }

            var cvgsWishlistItems = await _context.CvgsWishlistItems.FindAsync(id);
            if (cvgsWishlistItems == null)
            {
                return NotFound();
            }
            return View(cvgsWishlistItems);
        }

        // POST: WishlistItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WishlistItemsId,GameId,GameTitle,UserId")] CvgsWishlistItems cvgsWishlistItems)
        {
            if (id != cvgsWishlistItems.WishlistItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cvgsWishlistItems);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvgsWishlistItemsExists(cvgsWishlistItems.WishlistItemId))
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
            return View(cvgsWishlistItems);
        }

        // GET: WishlistItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CvgsWishlistItems == null)
            {
                return NotFound();
            }

            var cvgsWishlistItems = await _context.CvgsWishlistItems
                .FirstOrDefaultAsync(m => m.WishlistItemId == id);
            if (cvgsWishlistItems == null)
            {
                return NotFound();
            }

            return View(cvgsWishlistItems);
        }

        // POST: WishlistItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CvgsWishlistItems == null)
            {
                return Problem("Entity set 'CvgsDbContext.CvgsWishlistItems'  is null.");
            }
            var cvgsWishlistItems = await _context.CvgsWishlistItems.FindAsync(id);
            if (cvgsWishlistItems != null)
            {
                _context.CvgsWishlistItems.Remove(cvgsWishlistItems);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CvgsWishlistItemsExists(int id)
        {
          return _context.CvgsWishlistItems.Any(e => e.WishlistItemId == id);
        }
    }
}
