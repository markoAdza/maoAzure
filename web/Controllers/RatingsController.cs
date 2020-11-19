using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

using web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace web.Controllers
{
    public class RatingsController : Controller
    {
        private readonly MaoContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public RatingsController(MaoContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;

        }

        // GET: Ratings
        public async Task<IActionResult> Index()
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            var maoContext = _context.Rating.Where(o => o.Client == currentUser).Include(r => r.Menu);
            return View(await maoContext.ToListAsync());
        }

        // GET: Ratings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .Include(r => r.Menu)
                .FirstOrDefaultAsync(m => m.RatingID == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }


        // GET: Ratings/Create
        public async Task<IActionResult> Create()
        {
            PopulateMenuData(await _usermanager.GetUserAsync(User));

            return await Task.Run(() => View());
        }

        private void PopulateMenuData(ApplicationUser user)
        {
            var viewModel = new List<OrderedMenuData>();

            var ratedMenus = new List<int>();
            foreach (var rating in _context.Ratings)
            {
                if (rating.Client == user)
                {
                    ratedMenus.Add(rating.MenuID);
                }
            }

            foreach (var menu in _context.Menus)
            {
                if (!ratedMenus.Contains(menu.MenuID))
                {
                    viewModel.Add(new OrderedMenuData
                    {
                        MenuID = menu.MenuID,
                        FoodName = menu.FoodName
                    });
                }

            }

            ViewData["Menus"] = viewModel;
        }



        // POST: Ratings/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RatingID,MenuID,value")] Rating rating)
        {
            var currentUser = await _usermanager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                rating.Client = currentUser;
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateMenuData(currentUser);

            return View(rating); // naj nastavi selected na neki??
        }


        //    PopulateMenuData(await _usermanager.GetUserAsync(User));
        //     return await Task.Run(() => View());


        // GET: Ratings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating.FindAsync(id);
            if (rating == null)
            {
                return NotFound();
            }

            ViewData["MenuID"] = new SelectList(_context.Menus.Where(o => o.MenuID == rating.MenuID), "MenuID", "FoodName", rating.MenuID);
            return View(rating);
        }

        // POST: Ratings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RatingID,MenuID,value")] Rating rating)
        {
            if (id != rating.RatingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingExists(rating.RatingID))
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
            ViewData["MenuID"] = new SelectList(_context.Menus.Where(o => o.MenuID == rating.MenuID), "MenuID", "FoodName", rating.MenuID);
            return View(rating);
        }

        // GET: Ratings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rating = await _context.Rating
                .Include(r => r.Menu)
                .FirstOrDefaultAsync(m => m.RatingID == id);
            if (rating == null)
            {
                return NotFound();
            }

            return View(rating);
        }

        // POST: Ratings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rating = await _context.Rating.FindAsync(id);
            _context.Rating.Remove(rating);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RatingExists(int id)
        {
            return _context.Rating.Any(e => e.RatingID == id);
        }
    }
}
