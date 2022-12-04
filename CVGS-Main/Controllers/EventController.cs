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

namespace CVGS_Main.Controllers
{
    public class EventController : Controller
    {
        private readonly CvgsDbContext _context;

        public EventController(CvgsDbContext context)
        {
            _context = context;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            return View(await _context.CvgsEvent.ToListAsync());
            //return View
        }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CvgsEvent == null)
            {
                return NotFound();
            }

            var cvgsEvent = await _context.CvgsEvent
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (cvgsEvent == null)
            {
                return NotFound();
            }

            return View(cvgsEvent);
        }



        // GET: Event/Create
        [Authorize(Roles ="Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Name,Description,ScheduledTime")] CvgsEvent cvgsEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cvgsEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cvgsEvent);
        }

        // GET: Event/Edit/5

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CvgsEvent == null)
            {
                return NotFound();
            }

            var cvgsEvent = await _context.CvgsEvent.FindAsync(id);
            if (cvgsEvent == null)
            {
                return NotFound();
            }
            return View(cvgsEvent);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Edit(int id, [Bind("EventId,Name,Description,ScheduledTime")] CvgsEvent cvgsEvent)
        {
            if (id != cvgsEvent.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cvgsEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CvgsEventExists(cvgsEvent.EventId))
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
            return View(cvgsEvent);
        }

        // GET: Event/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CvgsEvent == null)
            {
                return NotFound();
            }

            var cvgsEvent = await _context.CvgsEvent
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (cvgsEvent == null)
            {
                return NotFound();
            }

            return View(cvgsEvent);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CvgsEvent == null)
            {
                return Problem("Entity set 'CvgsDbContext.CvgsEvent'  is null.");
            }
            var cvgsEvent = await _context.CvgsEvent.FindAsync(id);
            if (cvgsEvent != null)
            {
                _context.CvgsEvent.Remove(cvgsEvent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CvgsEventExists(int id)
        {
          return _context.CvgsEvent.Any(e => e.EventId == id);
        }
    }
}
