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
    public class OrdersController : Controller
    {
        private readonly MaoContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;
        public OrdersController(MaoContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // // GET: Orders
        // public async Task<IActionResult> Index()
        // {
        //     return View(await _context.Orders.ToListAsync());
        // }

        public async Task<IActionResult> Index(int? id, int? courseID)
        {
            var viewModel = new OrderIndexData();
            viewModel.Orders = await _context.Orders
                  .Include(i => i.Client)
                  .Include(i => i.MenuOrders)
                  .ThenInclude(i => i.Menu)
                  .AsNoTracking()
                  .ToListAsync();

            // SELECTING SPECIFIC ORDER
            if (id != null)
            {
                ViewData["OrderID"] = id.Value;
                Order order = viewModel.Orders.Where(
                    i => i.OrderID == id.Value).Single();
                viewModel.Menus = order.MenuOrders.Select(s => s.Menu);
            }

            return View(viewModel);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            PopulateMenuData();

            return View();
        }



        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("OrderID,Comment")] Order order, string[] selectedMenus)
        {

            var currentUser = await _usermanager.GetUserAsync(User);

            order.MenuOrders = new List<MenuOrder>();
            var selectedMenusHS = new HashSet<string>(selectedMenus);

            foreach (var menu in _context.Menus)
            {
                if (selectedMenusHS.Contains(menu.MenuID.ToString()))
                {
                    order.MenuOrders.Add(new MenuOrder { OrderID = order.OrderID, MenuID = menu.MenuID });
                }
            }


            if (ModelState.IsValid)
            {
                // dateCreated?
                Console.WriteLine("Current user: " + currentUser);
                order.Client = currentUser;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(i => i.MenuOrders).ThenInclude(i => i.Menu)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.OrderID == id);

            if (order == null)
            {
                return NotFound();
            }
            PopulateOrderedMenuData(order);

            return View(order);
        }

        // Provide information for checkboxes @ Edit
        private void PopulateOrderedMenuData(Order order)
        {
            var allMenus = _context.Menus;
            var orderMenus = new HashSet<int>(order.MenuOrders.Select(c => c.MenuID));
            var viewModel = new List<OrderedMenuData>();
            foreach (var menu in allMenus)
            {
                viewModel.Add(new OrderedMenuData
                {
                    MenuID = menu.MenuID,
                    FoodName = menu.FoodName,
                    Ordered = orderMenus.Contains(menu.MenuID)
                });
            }
            ViewData["Menus"] = viewModel;
        }

        // Provide information for checkboxes @ Create
        private void PopulateMenuData()
        {
            var allMenus = _context.Menus;
            var viewModel = new List<OrderedMenuData>();
            foreach (var menu in allMenus)
            {
                viewModel.Add(new OrderedMenuData
                {
                    MenuID = menu.MenuID,
                    FoodName = menu.FoodName,
                    Ordered = false
                });
            }
            ViewData["Menus"] = viewModel;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int? id, string[] selectedMenus)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderToUpdate = await _context.Orders
                .Include(i => i.MenuOrders)
                .ThenInclude(i => i.Menu)
                .FirstOrDefaultAsync(s => s.OrderID == id);

            if (await TryUpdateModelAsync<Order>(
                orderToUpdate,
                "",
                i => i.Comment))
            {
                UpdateOrderMenus(selectedMenus, orderToUpdate);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateOrderMenus(selectedMenus, orderToUpdate);
            PopulateOrderedMenuData(orderToUpdate);
            return View(orderToUpdate);
        }

        private void UpdateOrderMenus(string[] selectedMenus, Order orderToUpdate)
        {

            if (selectedMenus == null)
            {
                orderToUpdate.MenuOrders = new List<MenuOrder>();
                return;
            }

            var selectedMenusHS = new HashSet<string>(selectedMenus);
            var orderMenus = new HashSet<int>
                (orderToUpdate.MenuOrders.Select(c => c.Menu.MenuID));
            foreach (var menu in _context.Menus)
            {
                if (selectedMenusHS.Contains(menu.MenuID.ToString()))
                {
                    if (!orderMenus.Contains(menu.MenuID))
                    {
                        orderToUpdate.MenuOrders.Add(new MenuOrder { OrderID = orderToUpdate.OrderID, MenuID = menu.MenuID });
                    }
                }
                else
                {

                    if (orderMenus.Contains(menu.MenuID))
                    {
                        MenuOrder menuToRemove = orderToUpdate.MenuOrders.FirstOrDefault(i => i.MenuID == menu.MenuID);
                        _context.Remove(menuToRemove);
                    }
                }
            }
        }

        // [HttpPost, ActionName("Edit")]
        // [ValidateAntiForgeryToken]

        // public async Task<IActionResult> EditPost(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var orderToUpdate = await _context.Orders
        //         .FirstOrDefaultAsync(s => s.OrderID == id);

        //     if (await TryUpdateModelAsync<Order>(
        //         orderToUpdate,
        //         "",
        //         i => i.Comment))
        //     {

        //         try
        //         {
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateException /* ex */)
        //         {
        //             //Log the error (uncomment ex variable name and write a log.)
        //             ModelState.AddModelError("", "Unable to save changes. " +
        //                 "Try again, and if the problem persists, " +
        //                 "see your system administrator.");
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(orderToUpdate);
        // }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
