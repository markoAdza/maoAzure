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
            var maoContext = _context.Rating.Include(r => r.Menu);
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

        public async Task<IActionResult> Create()
        {
            var viewModel = new List<OrderedMenuData>();

            var currentUser = await _usermanager.GetUserAsync(User);

            var ratedMenus = new List<int>();
            foreach (var rating in _context.Ratings)
            {
                if (rating.Client == currentUser)
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
                        FoodName = menu.FoodName,
                        // Ordered = false
                    });
                }

            }

            ViewData["Menus"] = viewModel;
            return await Task.Run(() => View());
        }

        // GET: Ratings/Create
        // public async IActionResult Create()
        // {


        //     //ViewData["MenuID"] = new SelectList(_context.Menus, "MenuID", "FoodName");

        //     // gets all menus except the ones the user has already rated
        //     // PopulateMenuData();

        //     // var allMenus = _context.Menus;
        //     // var allRatings = _context.Ratings;

        //     // var viewModel = new List<OrderedMenuData>();

        //     // var currentUser = await _usermanager.GetUserAsync(User);

        //     // // Console.WriteLine("USER: " + user);

        //     // var ratedMenus = new List<int>();
        //     // foreach (var rating in allRatings)
        //     // {
        //     //     // if (rating.Client == currentUser)
        //     //     // {
        //     //     // ratedMenus.Add(rating.MenuID);
        //     //     // }
        //     // }

        //     // foreach (var menu in allMenus)
        //     // {
        //     //     // if (!ratedMenus.Contains(menu.MenuID))
        //     //     //{
        //     //     viewModel.Add(new OrderedMenuData
        //     //     {
        //     //         MenuID = menu.MenuID,
        //     //         FoodName = menu.FoodName,
        //     //         // Ordered = false
        //     //     });
        //     //     //}

        //     // }

        //     ViewData["Menus"] = viewModel;

        //     return View();
        // }

        // Provide information for checkboxes @ Create
        // private async void PopulateMenuData()
        // {
        //     var allMenus = _context.Menus;
        //     var allRatings = _context.Ratings;

        //     var viewModel = new List<OrderedMenuData>();

        //     var currentUser = await _usermanager.GetUserAsync(User);

        //     // Console.WriteLine("USER: " + user);

        //     var ratedMenus = new List<int>();
        //     foreach (var rating in allRatings)
        //     {
        //         // if (rating.Client == currentUser)
        //         // {
        //         // ratedMenus.Add(rating.MenuID);
        //         // }
        //     }

        //     foreach (var menu in allMenus)
        //     {
        //         // if (!ratedMenus.Contains(menu.MenuID))
        //         //{
        //         viewModel.Add(new OrderedMenuData
        //         {
        //             MenuID = menu.MenuID,
        //             FoodName = menu.FoodName,
        //             // Ordered = false
        //         });
        //         //}

        //     }

        //     ViewData["Menus"] = viewModel;
        // }




        // POST: Ratings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RatingID,MenuID,value")] Rating rating)
        {

            // add current user to the rating
            var currentUser = await _usermanager.GetUserAsync(User);
            rating.Client = currentUser;

            if (ModelState.IsValid)
            {
                _context.Add(rating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuID"] = new SelectList(_context.Menus, "MenuID", "MenuID", rating.MenuID);
            return View(rating);


            // var currentUser = await _usermanager.GetUserAsync(User);

            // order.MenuOrders = new List<MenuOrder>();
            // var selectedMenusHS = new HashSet<string>(selectedMenus);

            // foreach (var menu in _context.Menus)
            // {
            //     if (selectedMenusHS.Contains(menu.MenuID.ToString()))
            //     {
            //         order.MenuOrders.Add(new MenuOrder
            //         {
            //             OrderID = order.OrderID,
            //             MenuID = menu.MenuID
            //         });
            //     }
            // }


            // if (ModelState.IsValid)
            // {
            //     // dateCreated?
            //     Console.WriteLine("Current user: " + currentUser);
            //     order.Client = currentUser;
            //     _context.Add(order);
            //     await _context.SaveChangesAsync();
            //     return RedirectToAction(nameof(Index));
            // }
            // return View(order);
        }



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
            ViewData["MenuID"] = new SelectList(_context.Menus, "MenuID", "MenuID", rating.MenuID);
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
            ViewData["MenuID"] = new SelectList(_context.Menus, "MenuID", "MenuID", rating.MenuID);
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
