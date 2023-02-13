/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 24-Oct-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Applications controller to access edit, delete, create, list, and index pages. Handles saving and updating applications
    in the database and also handles file upload.
*/

using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TAApplication.Areas.Data;
using TAApplication.Data;
using Application = TAApplication.Models.Application;

namespace TAApplication.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly TAApplicationContext _context;
        private readonly UserManager<TAUser> _userManager;

        public ApplicationsController(IConfiguration configuration, TAApplicationContext context, UserManager<TAUser> userManager)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }

        // Applications list only viewable by admins and professors
        [Authorize(Roles = "Admin, Professor")]
        public async Task<IActionResult> List()
        {
              return View(await _context.Applications.Include(application => application.TAUser).ToListAsync());
        }

        // Application index page only viewable by admins
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Applications.Include(application => application.TAUser).ToListAsync());
        }

        // Application details page only viewable by admins, professors, and the specific
        // student attached to the application
        [Authorize(Roles = "Admin, Professor, Applicant")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(application => application.TAUser)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (application == null)
            {
                return NotFound();
            }

            // notify user is denied access if trying to access a different user's application
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Applicant") && user.Id != application.TAUser.Id)
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }

            return View(application);
        }

        // Application create page only viewable by applicants who have no application yet
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            var application = await _context.Applications
                .Include(application => application.TAUser)
                .FirstOrDefaultAsync(m => m.TAUser.Id == user.Id);

            if (application != null)
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }
            return View();
        }

        // Called when student clicks the create button to create the application
        [Authorize(Roles = "Applicant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,DegreePursuing,Department,GPA,AvailableHours,AvailableWeekBefore,SemestersCompleted,PersonalStatement,TransferSchool,LinkedInURL")] Application application)
        {
            // Remove TAUSer info to ensure ModelState is valid
            ModelState.Remove("TAUser.Unid");
            ModelState.Remove("TAUser.Name");
            application.TAUser = await _userManager.GetUserAsync(User);

            var user = await _userManager.GetUserAsync(User);
            var existingApplication = await _context.Applications
                .Include(app => app.TAUser)
                .FirstOrDefaultAsync(m => m.TAUser.Id == user.Id);

            if (existingApplication != null)
            {
                return BadRequest("User already has an application");
            }

            if (ModelState.IsValid)
            {
                _context.Add(application);
                await _context.SaveChangesAsync();
                var id = _context.Applications.Where(x => x == application).First().ID;
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(application);
        }

        // Edit page only viewable to admins and the specific applicant attached to the application
        [Authorize(Roles = "Admin, Applicant")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // notify user is denied access if trying to access a different user's application
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Applicant") && user.Id != application.TAUser.Id)
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }

            return View(application);
        }

        // Called when user clicks save on the edit page to update application
        [Authorize(Roles = "Admin, Applicant")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null) 
            {
                return BadRequest(); 
            }

            var applicationToUpdate = _context.Applications
                .Where(o => o.ID == id).Include(o => o.TAUser).FirstOrDefault();

            // check if user is authorized to edit this application
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Applicant") && user.Id != applicationToUpdate?.TAUser.Id)
            {
                return BadRequest("User is not authorized to edit this application");
            }

            if (applicationToUpdate != null)
            {
                if (await TryUpdateModelAsync<Application>(applicationToUpdate, "",
                    s => s.DegreePursuing,
                    s => s.Department,
                    s => s.GPA,
                    s => s.AvailableHours,
                    s => s.AvailableWeekBefore,
                    s => s.SemestersCompleted,
                    s => s.PersonalStatement,
                    s => s.TransferSchool,
                    s => s.LinkedInURL))
                {
                    try
                    {
                        _context.SaveChanges();
                        return RedirectToAction("Details", new { id = applicationToUpdate.ID });
                    }
                    catch (DataException)
                    {
                        ViewData["ErrorMessage"] = "Something went wrong while attempting to save data. Please try again.";
                        return View(applicationToUpdate);
                    }
                }
            }
            return View(applicationToUpdate);
        }

        // Delete page only viewable to admins and the specific applicant attached to the application
        [Authorize(Roles = "Admin, Applicant")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(application => application.TAUser)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (application == null)
            {
                return NotFound();
            }

            // notify user is denied access if trying to access a different user's application
            var user = await _userManager.GetUserAsync(User);
            if (await _userManager.IsInRoleAsync(user, "Applicant") && user.Id != application.TAUser.Id)
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }

            return View(application);
        }

        // Called when student clicks delete on the delete page
        [Authorize(Roles = "Admin, Applicant")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Applications == null)
            {
                return Problem("Entity set 'TAApplicationContext.Applications'  is null.");
            }

            var application = await _context.Applications.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);
            if (application != null)
            {
                if (await _userManager.IsInRoleAsync(user, "Applicant") && user.Id != application.TAUser.Id)
                {
                    return BadRequest("User is not authorized to delete this application");
                }

                _context.Applications.Remove(application);
            }
            
            await _context.SaveChangesAsync();

            // redirect applicant to create page
            if (await _userManager.IsInRoleAsync(user, "Applicant"))
            {
                return RedirectToAction(nameof(Create));
            }

            // redirect admin to list page
            else
            {
                return RedirectToAction(nameof(List));
            }
        }

        // File Upload handler for a user to upload image or resume. Does various checks with the category, file,
        // and applicationId to ensure data submitted is valid
        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> FileUpload(List<IFormFile> files, string category, int applicationID)
        {
            if (!(category == "resume" || category == "image"))
            {
                return BadRequest("The category specified was not valid.");
            }

            var application = await _context.Applications.FindAsync(applicationID);
            if (application == null)
            {
                return BadRequest("The application id is invalid");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user.Id != application.TAUser.Id)
            {
                return BadRequest("The user does not own the application");
            }

            if (files.Count() != 1)
            {
                TempData[category + "ErrorMessage"] = "Please submit 1 file";
                return RedirectToAction("Details", application);
            }

            if (files.First().Length > 10000000)
            {
                TempData[category + "ErrorMessage"] = "The file submitted was too large";
                return RedirectToAction("Details", application);
            }

            if (files.First().Length == 0)
            {
                TempData[category + "ErrorMessage"] = "The file submitted was empty";
                return RedirectToAction("Details", application);
            }

            if (!files.First().FileName.EndsWith("pdf") && category == "resume")
            {
                TempData[category + "ErrorMessage"] = "The resume uploaded must be a pdf";
                return RedirectToAction("Details", application);
            }

            if (!(files.First().FileName.EndsWith(".jpg") || files.First().FileName.EndsWith(".jpeg") || files.First().FileName.EndsWith(".png")) && category == "image")
            {
                TempData[category + "ErrorMessage"] = "The image uploaded must be a jpg, jpeg, or png";
                return RedirectToAction("Details", application);
            }

            // get random name removing random extension
            string new_name = Path.GetRandomFileName();
            new_name = new_name.Remove(new_name.IndexOf('.'));

            // put extension of file onto random name
            string fileExtension = files.First().FileName.Substring(files.First().FileName.IndexOf('.'));
            new_name = new_name + fileExtension;

            string path = Path.Combine(_configuration["FilePath"], new_name);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await files.First().CopyToAsync(stream);
            }

            if (category == "resume")
            {
                application.ResumeFilename = new_name;
            }

            if (category == "image")
            {
                application.ImageFilename = new_name;
            }

            _context.SaveChanges();

            return View(nameof(Details), application);
        }

        private bool ApplicationExists(int id)
        {
          return _context.Applications.Any(e => e.ID == id);
        }
    }
}
