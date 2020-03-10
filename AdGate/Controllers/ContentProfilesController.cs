using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdGate.Data;
using AdGate.Models;
using Microsoft.AspNetCore.Http;

namespace AdGate.Controllers
{
    public class ContentProfilesController : Controller
    {
        private readonly AdGateContext _context;

        public ContentProfilesController(AdGateContext context)
        {
            _context = context;
        }

        // GET: ContentProfiles
        public async Task<IActionResult> Index()
        {
            var publisherProfileId = _context.Profiles
                .Where(p => p.UserId == SessionExtensions
                    .GetInt32(HttpContext.Session, "UserId"))
                .FirstOrDefault().ProfileId;
            ContentProfile contentProfiles = _context.ContentProfiles
                .FirstOrDefault();
            foreach (ContentProfileTag apt in contentProfiles.ContentProfileTags)
            {
                apt.Tag = _context.Tags.Where(t => t.TagId == apt.TagId).FirstOrDefault();
            }
            return View(contentProfiles);
        }

        // GET: ContentProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentProfile = await _context.ContentProfiles
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(m => m.ContentProfileId == id);
            if (contentProfile == null)
            {
                return NotFound();
            }

            return View(contentProfile);
        }

        // GET: ContentProfiles/Create
        public IActionResult Create()
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

        // POST: ContentProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PublisherProfileId,ContentName,ContentMedium,VisitsPerMonth,SelectedTags")] CreateContentProfileViewModel createContentProfileViewModel)
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

        // GET: ContentProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentProfile = await _context.ContentProfiles.FindAsync(id);
            if (contentProfile == null)
            {
                return NotFound();
            }
            ViewData["PublisherProfileId"] = new SelectList(_context.PublisherProfiles, "ProfileId", "Discriminator", contentProfile.PublisherProfileId);
            return View(contentProfile);
        }

        // POST: ContentProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContentProfileId,ContentName,ContentMedium,VisitsPerMonth,IsVerified,PublisherProfileId")] ContentProfile contentProfile)
        {
            if (id != contentProfile.ContentProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contentProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentProfileExists(contentProfile.ContentProfileId))
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
            ViewData["PublisherProfileId"] = new SelectList(_context.PublisherProfiles, "ProfileId", "Discriminator", contentProfile.PublisherProfileId);
            return View(contentProfile);
        }

        // GET: ContentProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contentProfile = await _context.ContentProfiles
                .Include(c => c.Owner)
                .FirstOrDefaultAsync(m => m.ContentProfileId == id);
            if (contentProfile == null)
            {
                return NotFound();
            }

            return View(contentProfile);
        }

        // POST: ContentProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contentProfile = await _context.ContentProfiles.FindAsync(id);
            _context.ContentProfiles.Remove(contentProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentProfileExists(int id)
        {
            return _context.ContentProfiles.Any(e => e.ContentProfileId == id);
        }
    }
}
