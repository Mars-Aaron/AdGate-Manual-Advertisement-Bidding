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
    public class PublisherDashboardController : Controller
    {
        private readonly AdGateContext _context;

        public PublisherDashboardController(AdGateContext context)
        {
            _context = context;
        }

        // GET: PublisherDashboard
        public IActionResult Index()
        {
            PublisherProfile publisherProfile = _context.PublisherProfiles
                .Include(pp => pp.User)
                .Where(pp => pp.UserId == SessionExtensions.GetInt32(HttpContext.Session, "UserId"))
                .FirstOrDefault();
            return View(publisherProfile);
        }

        // Get PublisherDashboard/SetProfile
        public IActionResult SetProfile()
        {
            if (HasSession())
            {
                PublisherProfileViewModel publisherProfileViewModel = new PublisherProfileViewModel
                {
                    UserId = SessionExtensions.GetInt32(HttpContext.Session, "UserId").Value
                };
                return View(publisherProfileViewModel);
            }
            else
            {
                return GetLoginRedirect();
            }
        }

        [HttpPost, ActionName("SetProfile")]
        public async Task<IActionResult> SetProfile([Bind("UserId,GivenName,FamilyName,ProfilePicture")] PublisherProfileViewModel publisherProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = _context.Users.Where(u => u.UserId == publisherProfileViewModel.UserId).FirstOrDefault();
                var profilePictureUri = new ProfilePictureController().Upload(publisherProfileViewModel.ProfilePicture, DateTime.Now.ToFileTime() + "-" + publisherProfileViewModel.GivenName + "-" + publisherProfileViewModel.FamilyName + "-ProfilePicture");
                Profile userProfile = new PublisherProfile
                {
                    User = user,
                    GivenName = publisherProfileViewModel.GivenName,
                    FamilyName = publisherProfileViewModel.FamilyName,
                    ProfilePicture = profilePictureUri.Result
                };
                _context.Add(userProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publisherProfileViewModel);
        }

        public IActionResult EditProfile(int profileId)
        {
            PublisherEditProfileViewModel publisherEditProfileViewModel = new PublisherEditProfileViewModel();
            PublisherProfile publisherProfile = _context.PublisherProfiles
                .Include(ap => ap.User)
                .Where(ap => ap.UserId == SessionExtensions.GetInt32(HttpContext.Session, "UserId"))
                .FirstOrDefault();
            publisherEditProfileViewModel.ProfileId = profileId;
            publisherEditProfileViewModel.GivenName = publisherProfile.GivenName;
            publisherEditProfileViewModel.FamilyName = publisherProfile.FamilyName;
            publisherEditProfileViewModel.Password = _context.Users
                .Where(u => u.UserId == SessionExtensions.GetInt32(HttpContext.Session, "UserId"))
                .FirstOrDefault().Password;
            return View(publisherEditProfileViewModel);
        }

        public IActionResult ListContentProfiles()
        {
            var publisherProfileId = _context.Profiles
                .Where(p => p.UserId == SessionExtensions
                    .GetInt32(HttpContext.Session, "UserId"))
                .FirstOrDefault().ProfileId;
            List<ContentProfile> contentProfiles = _context.ContentProfiles
                .Include(cp => cp.ContentProfileTags)
                .Where(cp => cp.PublisherProfileId == publisherProfileId)
                .ToList();
            foreach (ContentProfile cp in contentProfiles)
            {
                foreach (ContentProfileTag cpt in cp.ContentProfileTags)
                {
                    cpt.Tag = _context.Tags.Where(t => t.TagId == cpt.TagId).FirstOrDefault();
                }
            }
            return View(contentProfiles);
        }

        public IActionResult CreateContentProfile()
        {
            CreateContentProfileViewModel createContentProfileViewModel = new CreateContentProfileViewModel
            {
                PublisherProfileId = _context.Users.Include(u => u.Profile).Where(u => u.UserId == SessionExtensions.GetInt32(HttpContext.Session, "UserId")).FirstOrDefault().Profile.ProfileId,
                Tags = _context.Tags
                 .Select(t => new SelectListItem { Text = t.TypeName, Value = t.TagId.ToString() })
                 .ToList()
            };
            return View(createContentProfileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateContentProfile([Bind("PublisherProfileId,ContentName,ContentMedium,VisitsPerMonth,SelectedTags")] CreateContentProfileViewModel createContentProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                ContentProfile contentProfile = new ContentProfile
                {
                    PublisherProfileId = createContentProfileViewModel.PublisherProfileId,
                    ContentName = createContentProfileViewModel.ContentName,
                    ContentMedium = createContentProfileViewModel.ContentMedium,
                    VisitsPerMonth = createContentProfileViewModel.VisitsPerMonth,
                    IsVerified = false
                };
                _context.Add(contentProfile);
                await _context.SaveChangesAsync();
                var selectedTags = _context.Tags.Where(t => createContentProfileViewModel.SelectedTags.Contains(t.TagId.ToString())).ToList();
                var contentProfileTags = selectedTags.Select(st => new ContentProfileTag { ContentProfile = contentProfile as ContentProfile, Tag = st }).ToList();
                _context.AddRange(contentProfileTags.ToArray());
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "ContentProfiles");
            }
            return View(createContentProfileViewModel);
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
