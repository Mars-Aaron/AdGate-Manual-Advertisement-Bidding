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
using System.IO;
using System.Drawing;

namespace AdGate.Controllers
{
    public class AuthController : Controller
    {
        private readonly AdGateContext _context;

        public AuthController(AdGateContext context)
        {
            _context = context;
        }

        // GET: Auth/Login
        public IActionResult Login(string message)
        {
            int? userId = SessionExtensions.GetInt32(HttpContext.Session, "UserId");
            if (userId.HasValue) {
                if (UserExists(userId.Value))
                {
                    User user = _context.Users.Include(u => u.Profile).FirstOrDefault(u => u.UserId == userId.Value);
                    if (user != null)
                    {
                        return GetRedirectAfterLogin(user);
                    }
                } else
                {
                    HttpContext.Session.Clear();
                    ViewData["ErrorNotification"] = "Session is invalid, please Sign In again.";
                }
            }
            if (message != null && message == "RegisterRedirectSuccess")
            {
                ViewData["SuccessNotification"] = "Successfully created a new account, Please sign in to continue...";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] LoginViewModel loginViewModel)
        {

            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .Include(u => u.Profile)
                    .FirstOrDefaultAsync(m => m.Email.ToLower() == loginViewModel.Email.ToLower());
                if(user != null && user.Password == loginViewModel.Password)
                {
                    if (!user.IsActive)
                    {
                        ViewData["ErrorNotification"] = "Your account is currently deactivated, please contact an admin to reactivate.";
                    } else
                    {
                        SessionExtensions.SetInt32(HttpContext.Session, "UserId", user.UserId);
                        return GetRedirectAfterLogin(user);
                    }
                } else
                {
                    ViewData["ErrorNotification"] = "User not found, email or password is incorrect.";
                }
            }
            return View(loginViewModel);
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Email,Password,ConfirmPassword,UserType")] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (EmailExists(registerViewModel.Email))
                {
                    ViewData["ErrorNotification"] = "The email is already being used, please use a different email.";
                } else
                {
                    User user = new User
                    {
                        Email = registerViewModel.Email,
                        Password = registerViewModel.Password,
                        UserType = registerViewModel.UserType,
                        IsActive = true
                    };
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    ViewData["SuccessNotification"] = "A new account has been created, please sign into the new account to proceed.";
                    return RedirectToAction("Login", new { message = "RegisterRedirectSuccess" });
                }
            }
            return View(registerViewModel);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }

        private bool EmailExists(string email)
        {
            return _context.Users.Any(e => e.Email == email);
        }

        private IActionResult GetRedirectAfterLogin(User user)
        {
            if (user.UserType == UserType.Administrator)
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
            else if (user.UserType == UserType.Advertiser)
            {
                if (user.Profile == null)
                {
                    return RedirectToAction("SetProfile", "AdvertiserDashboard");
                }
                else
                {
                    return RedirectToAction("Index", "AdvertiserDashboard");
                }
            }
            else
            {
                if (user.Profile == null)
                {
                    return RedirectToAction("SetProfile", "PublisherDashboard");
                }
                else
                {
                    return RedirectToAction("Index", "PublisherDashboard");
                }
            }
        }
    }
}
