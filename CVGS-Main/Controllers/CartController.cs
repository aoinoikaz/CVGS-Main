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
using Microsoft.AspNetCore.Identity;
using CVGS_Main.Models.View_Models;

namespace CVGS_Main.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly CvgsDbContext _context;
        private readonly UserManager<CvgsUser> _userManager;

        public CartController(CvgsDbContext context, UserManager<CvgsUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            CvgsCartViewModel v = new CvgsCartViewModel();

            var user = await _userManager.GetUserAsync(User);

            var userCart = _context.CvgsCart.Where(c => c.UserId == user.Id).FirstOrDefault();

            List<CvgsLineItem> cartItems = null;

            if (userCart != null)
            {
                v.CvgsCart = userCart;
                cartItems = _context.CvgsLineItem.Where(i => i.CartId == userCart.CartId).ToList();
                v.LineItems = cartItems;
            }
            else
            {
                CvgsCart cart = new CvgsCart();
                cart.UserId = user.Id;
                _context.CvgsCart.Add(cart);
                _context.SaveChanges();
            }

            return View(v);
          
        }

        // GET: Cart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CvgsCart == null)
            {
                return NotFound();
            }

            var cvgsCart = await _context.CvgsCart
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cvgsCart == null)
            {
                return NotFound();
            }

            return View(cvgsCart);
        }
    

        public async Task<IActionResult> AddToCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            CvgsCart? existingCart = _context.CvgsCart.Where(c => c.UserId == user.Id).FirstOrDefault();

            // first, check if a cart exists for this current user
            // if it doesnt exist, create it
            // if it exists, add a line item to it with the specific id
            // update
            // save

            CvgsCart? tempCart = new CvgsCart();

            if (existingCart == null)
            {
                // create cart for the first time  
                tempCart.UserId = user.Id;
                _context.CvgsCart.Add(tempCart);
                _context.SaveChanges();
            }
            else
            {
                // load the cart for the user
                tempCart = existingCart;
            }

            CvgsLineItem newItem = new CvgsLineItem();
            newItem.CartId = tempCart.CartId;
            newItem.GameId = id.Value;

            _context.CvgsLineItem.Add(newItem);
            _context.SaveChanges();

            TempData["AddItemToCartSuccess"] = "<div " +
                "class=\"alert alert-success alert-dismissible\">Successfully added item to cart!</div>";
   
            return RedirectToAction("Index", "Store");
        }

        // GET: Cart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CvgsCart == null)
            {
                return NotFound();
            }

            var cvgsCart = await _context.CvgsCart.FindAsync(id);
            if (cvgsCart == null)
            {
                return NotFound();
            }
            return View(cvgsCart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,UserId")] CvgsCart cvgsCart)
        {
            if (id != cvgsCart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cvgsCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvgsCartExists(cvgsCart.CartId))
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
            return View(cvgsCart);
        }

        // GET: Cart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CvgsCart == null)
            {
                return NotFound();
            }

            var cvgsCart = await _context.CvgsCart
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cvgsCart == null)
            {
                return NotFound();
            }

            return View(cvgsCart);
        }


        // POST: Cart/Delete/5
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            if (_context.CvgsLineItem == null)
            {
                return Problem("Entity set 'CvgsDbContext.CvgsLineItem'  is null.");
            }

            CvgsLineItem? lineItem = _context.CvgsLineItem.Where(item => item.LineItemId == id).FirstOrDefault();
            if (lineItem != null)
            {
                _context.CvgsLineItem.Remove(lineItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       

        private bool CvgsCartExists(int id)
        {
          return _context.CvgsCart.Any(e => e.CartId == id);
        }
    }
}
