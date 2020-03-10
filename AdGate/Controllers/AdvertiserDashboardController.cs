using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdGate.Models;
using AdGate.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.IO;

namespace AdGate.Controllers
{
    public class AdvertiserDashboardController : Controller
    {
        private readonly AdGateContext _context;

        public AdvertiserDashboardController(AdGateContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            AdvertiserProfile advertiserProfile = _context.AdvertiserProfiles
                .Include(ap => ap.AdvertiserProfileTags)
                .Include(ap => ap.User)
                .Where(ap => ap.UserId == SessionExtensions.GetInt32(HttpContext.Session, "UserId"))
                .FirstOrDefault();
            foreach (AdvertiserProfileTag apt in advertiserProfile.AdvertiserProfileTags)
            {
                apt.Tag = _context.Tags.Where(t => t.TagId == apt.TagId).FirstOrDefault();
            }
            return View(advertiserProfile);
        }

        // Get AdvertiserDashboard/SetProfile
        public IActionResult SetProfile()
        {
            if (HasSession())
            {
                AdvertiserProfileViewModel advertiserProfileViewModel = new AdvertiserProfileViewModel();
                advertiserProfileViewModel.Tags = _context.Tags
                    .Select(t => new SelectListItem { Text = t.TypeName, Value = t.TagId.ToString() })
                    .ToList();
                advertiserProfileViewModel.UserId = SessionExtensions.GetInt32(HttpContext.Session, "UserId").Value;
                return View(advertiserProfileViewModel);
            } else
            {
                return GetLoginRedirect();
            }
        }

        [HttpPost, ActionName("SetProfile")]
        public async Task<IActionResult> SetProfile([Bind("UserId,ProfilePicture,GivenName,FamilyName,CompanyName,SelectedTags")] AdvertiserProfileViewModel advertiserProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = _context.Users.Where(u => u.UserId == advertiserProfileViewModel.UserId).FirstOrDefault();
                var profilePictureUri = new ProfilePictureController().Upload(advertiserProfileViewModel.ProfilePicture, DateTime.Now.ToFileTime() + "-" + advertiserProfileViewModel.GivenName + "-" + advertiserProfileViewModel.FamilyName + "-ProfilePicture");
                Profile userProfile = new AdvertiserProfile
                {
                    User = user,
                    GivenName = advertiserProfileViewModel.GivenName,
                    FamilyName = advertiserProfileViewModel.FamilyName,
                    CompanyName = advertiserProfileViewModel.CompanyName,
                    ProfilePicture = profilePictureUri.Result
                };
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                var selectedTags = _context.Tags.Where(t => advertiserProfileViewModel.SelectedTags.Contains(t.TagId.ToString())).ToList();
                var advertiserTags = selectedTags.Select(st => new AdvertiserProfileTag { AdvertiserProfile = userProfile as AdvertiserProfile, Tag = st }).ToList();
                _context.AddRange(advertiserTags.ToArray());
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advertiserProfileViewModel);
        }

        public IActionResult EditProfile(int profileId)
        {
            AdvertiserProfile advertiserProfile = _context.AdvertiserProfiles
                .Include(ap => ap.AdvertiserProfileTags)
                .Include(ap => ap.User)
                .Where(ap => ap.UserId == SessionExtensions.GetInt32(HttpContext.Session, "UserId"))
                .FirstOrDefault();
            AdvertiserEditProfileViewModel advertiserEditProfileViewModel = new AdvertiserEditProfileViewModel
            {
                ProfileId = profileId,
                GivenName = advertiserProfile.GivenName,
                FamilyName = advertiserProfile.FamilyName,
                CompanyName = advertiserProfile.CompanyName,
                ProfilePicture = advertiserProfile.ProfilePicture,
                Password = _context.Users
                    .Where(u => u.UserId == SessionExtensions.GetInt32(HttpContext.Session, "UserId"))
                    .FirstOrDefault().Password,
                Tags = _context.Tags
                    .Select(t => new SelectListItem
                    {
                        Text = t.TypeName,
                        Value = t.TagId.ToString(),
                        Selected = advertiserProfile.AdvertiserProfileTags
                            .Where(apt => apt.TagId == t.TagId)
                            .FirstOrDefault() != null
                    })
                    .ToList(),
                SelectedTags = advertiserProfile.AdvertiserProfileTags.Select(apt => apt.TagId.ToString()).ToArray()
            };
            return View(advertiserEditProfileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile([Bind("ProfileId,NewProfilePicture,GivenName,FamilyName,CompanyName,Password,ConfirmPassword,SelectedTags")] AdvertiserEditProfileViewModel advertiserEditProfileViewModel)
        {
            advertiserEditProfileViewModel.ProfilePicture = _context.AdvertiserProfiles
                .Where(ap => ap.ProfileId == advertiserEditProfileViewModel.ProfileId)
                .FirstOrDefault()
                .ProfilePicture;
            if (ModelState.IsValid) {
                AdvertiserProfile advertiserProfile = _context.AdvertiserProfiles.Include(ap => ap.User).Where(ap => ap.ProfileId == advertiserEditProfileViewModel.ProfileId).FirstOrDefault();
                advertiserProfile.GivenName = advertiserEditProfileViewModel.GivenName;
                advertiserProfile.FamilyName = advertiserEditProfileViewModel.FamilyName;
                advertiserProfile.CompanyName = advertiserEditProfileViewModel.CompanyName;
                advertiserProfile.User.Password = advertiserEditProfileViewModel.Password;
                if (advertiserEditProfileViewModel.NewProfilePicture != null)
                {
                    var profilePictureUri = new ProfilePictureController()
                        .Upload(
                            advertiserEditProfileViewModel.NewProfilePicture,
                            DateTime.Now.ToFileTime() + "-" + advertiserEditProfileViewModel.GivenName + "-" + advertiserEditProfileViewModel.FamilyName + "-ProfilePicture");
                    advertiserProfile.ProfilePicture = profilePictureUri.Result;
                }
                _context.Update(advertiserProfile);
                _context.SaveChanges();
                string fileName = advertiserEditProfileViewModel.ProfilePicture.Split('/').Last();
                new ProfilePictureController().DeleteProfilePicture(fileName);
                return RedirectToAction(nameof(Index));
            }
            advertiserEditProfileViewModel.Tags = _context.Tags
                    .Select(t => new SelectListItem
                    {
                        Text = t.TypeName,
                        Value = t.TagId.ToString(),
                        Selected = advertiserEditProfileViewModel.SelectedTags
                            .Where(st => int.Parse(st) == t.TagId)
                            .FirstOrDefault() != null
                    })
                    .ToList();
            return View(advertiserEditProfileViewModel);
        }

        private bool HasSession()
        {
            int? uid = SessionExtensions.GetInt32(HttpContext.Session, "UserId");
            return uid.HasValue;
        }

        private IActionResult GetLoginRedirect()
        {
            return RedirectToAction("Login", "Auth");
        }
    }
}