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
using CVGS_Main.Models.View_Models;
using Microsoft.AspNetCore.Authorization;

namespace CVGS_Main.Controllers
{
    [Authorize]
    public class FriendListController : Controller
    {
        private readonly UserManager<CvgsUser> _userManager;
        private readonly CvgsDbContext _context;

        public FriendListController(CvgsDbContext context, UserManager<CvgsUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FriendList
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            // get users friend list
            CvgsFriendList? tempFriendList = _context.CvgsFriendList.Where(f => f.UserId == user.Id).FirstOrDefault();

            if (tempFriendList == null)
            {
                // the users friend list doesnt exist, so create it
                tempFriendList = new CvgsFriendList();
                tempFriendList.UserId = user.Id;

                _context.CvgsFriendList.Add(tempFriendList);
                _context.SaveChanges();
            }

            var friends = _context.CvgsFriends.Where(f => f.FriendListId == tempFriendList.FriendListId).ToList();

            return View(friends);
        }

        // GET: FriendList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CvgsFriendList == null)
            {
                return NotFound();
            }

            var cvgsFriendList = await _context.CvgsFriendList
                .FirstOrDefaultAsync(m => m.FriendListId == id);
            if (cvgsFriendList == null)
            {
                return NotFound();
            }

            return View(cvgsFriendList);
        }

        // GET: FriendList/Create
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> AddFriend(CvgsFriendInputModel input)
        {
            var friend = await _userManager.FindByNameAsync(input.Username);
            var myself = await _userManager.GetUserAsync(User);

            if (friend == null)
            {
                TempData["InvalidUsername"] = "<div " +
                "class=\"alert alert-danger\">No friend exists with the username specified!</div>";

                return RedirectToAction("Create");
            }

            CvgsFriendList? myFriendList = _context.CvgsFriendList.Where(f => f.UserId == myself.Id).FirstOrDefault();

            if (myFriendList == null)
            {
                myFriendList = new CvgsFriendList();
                myFriendList.UserId = myself.Id;
                _context.CvgsFriendList.Add(myFriendList);
                _context.SaveChanges();
            }

            if (friend.Id == myself.Id)
            {
                TempData["InvalidUsername"] = "<div " +
               "class=\"alert alert-danger\">You cannot add yourself loser!</div>";
                return RedirectToAction("Create");
            }

            CvgsFriend newFriend = new CvgsFriend();
            newFriend.FriendToAdd = friend.Id;
            newFriend.FriendListId = myFriendList.FriendListId;
            _context.CvgsFriends.Add(newFriend);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: FriendList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FriendListId,UserId")] CvgsFriendList cvgsFriendList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cvgsFriendList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cvgsFriendList);
        }

        // GET: FriendList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CvgsFriendList == null)
            {
                return NotFound();
            }

            var cvgsFriendList = await _context.CvgsFriendList.FindAsync(id);
            if (cvgsFriendList == null)
            {
                return NotFound();
            }
            return View(cvgsFriendList);
        }

        // POST: FriendList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FriendListId,UserId")] CvgsFriendList cvgsFriendList)
        {
            if (id != cvgsFriendList.FriendListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cvgsFriendList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvgsFriendListExists(cvgsFriendList.FriendListId))
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
            return View(cvgsFriendList);
        }

        // GET: FriendList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CvgsFriendList == null)
            {
                return NotFound();
            }

            var cvgsFriendList = await _context.CvgsFriendList
                .FirstOrDefaultAsync(m => m.FriendListId == id);
            if (cvgsFriendList == null)
            {
                return NotFound();
            }

            return View(cvgsFriendList);
        }

        // POST: FriendList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CvgsFriendList == null)
            {
                return Problem("Entity set 'CvgsDbContext.CvgsFriendList'  is null.");
            }
            var cvgsFriendList = await _context.CvgsFriendList.FindAsync(id);
            if (cvgsFriendList != null)
            {
                _context.CvgsFriendList.Remove(cvgsFriendList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CvgsFriendListExists(int id)
        {
          return _context.CvgsFriendList.Any(e => e.FriendListId == id);
        }
    }
}
