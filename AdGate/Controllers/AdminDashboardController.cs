using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdGate.Data;
using AdGate.Models;

namespace AdGate.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly AdGateContext _context;

        public AdminDashboardController(AdGateContext context)
        {
            _context = context;
        }

        // GET: AdminDashboard/Index
        public async Task<IActionResult> Index()
        {
            if (HasSession())
            {
                ViewData["Me"] = SessionExtensions.GetInt32(HttpContext.Session, "UserId");
                return View(await _context.Users.ToListAsync());
            }
            else
            {
                return GetLoginRedirect();
            }
        }

        // GET: AdminDashboard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HasSession())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            else { return GetLoginRedirect(); }
        }

        // GET: AdminDashboard/CreateAdmin
        public IActionResult CreateAdmin()
        {
            if(HasSession()) { return View(); }
            else { return GetLoginRedirect(); }
        }

        // POST: AdminDashboard/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin([Bind("Email,Password,ConfirmPassword,UserType")] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (EmailExists(registerViewModel.Email))
                {
                    ViewData["ErrorNotification"] = "The email is already being used, please use a different email.";
                }
                else
                {
                    User user = new User();
                    user.Email = registerViewModel.Email;
                    user.Password = registerViewModel.Password;
                    user.UserType = registerViewModel.UserType;
                    user.IsActive = true;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(registerViewModel);
        }

        // GET: AdminDashboard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HasSession())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            else { return GetLoginRedirect(); }
        }

        // POST: AdminDashboard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,Password,UserType,IsActive")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            return View(user);
        }

        // GET: AdminDashboard/DeactivateAdmin/5
        public async Task<IActionResult> Deactivate(int? id)
        {
            if (HasSession())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            else { return GetLoginRedirect(); }
        }

        // POST: AdminDashboard/Delete/5
        [HttpPost, ActionName("Deactivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeactivateConfirmed(int UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            user.IsActive = false;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminDashboard/Activate/5
        public async Task<IActionResult> Activate(int? id)
        {
            if (HasSession())
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.Users
                    .FirstOrDefaultAsync(m => m.UserId == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            else { return GetLoginRedirect(); }
        }

        // POST: AdminDashboard/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateConfirmed(int UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            user.IsActive = true;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private bool EmailExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }

        private IActionResult GetLoginRedirect()
        {
            return RedirectToAction("Login", "Auth");
        }

        private bool HasSession()
        {
            int? uid = SessionExtensions.GetInt32(HttpContext.Session, "UserId");
            return uid.HasValue;
        }
    }
}
