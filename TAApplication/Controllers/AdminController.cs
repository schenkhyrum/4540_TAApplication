/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 24-Sep-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Admin Controller to direct user to admin pages, as well as update roles if an
    administrator changes a user's roles on the Role Manager page. Also contains
    a enrollment trends page to get enrollment over time for specified courses.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using TAApplication.Areas.Data;
using TAApplication.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly TAApplicationContext _context;
        private readonly UserManager<TAUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ILogger<AdminController> logger, TAApplicationContext context, UserManager<TAUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EnrollmentTrends()
        {
            // get enrollments
            var enrollments = _context.OverTimeEnrollments.Include(x => x.Course).ToList();

            // selects distinct ordered courses for the courses we have over time enrollments for
            var list = enrollments.Select(e =>
                  new SelectListItem
                  {
                      Value = e.ID.ToString(),
                      Text = e.Course.Department + " " + e.Course.CourseNumber.ToString()
                  })
                .DistinctBy(x => x.Text).OrderBy(x => x.Text).ToList();

            return View(list);
        }

        // Called when an admin clicks to change a user's role
        [HttpPost]
        public async Task<IActionResult> EditRole(string role, string userId, string addOrRemove)
        {
            if (userId == null)
            {
                return NotFound();
            }

            // get student and try to update specific values to prevent overposting
            var userToUpdate = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == userId);

            if (userToUpdate != null)
            {
                if (addOrRemove == "add")
                {
                    await _userManager.AddToRoleAsync(userToUpdate, role);
                    return Ok(new { success = true, message = "added!" });
                }
                else if (addOrRemove == "remove")
                {
                    await _userManager.RemoveFromRoleAsync(userToUpdate, role);
                    return Ok(new { success = true, message = "removed!" });
                }
            }

            //user to update was not found
            return NotFound();
        }

        // Gets the enrollment data
        [HttpGet]
        public async Task<IActionResult> GetEnrollmentData(DateTime startDate, DateTime endDate, string department, int courseNumber)
        {
            var enrollments = await _context.OverTimeEnrollments.Where(x => x.Course.Department == department && 
                                                                       x.Course.CourseNumber == courseNumber &&
                                                                       x.DateTaken.Date >= startDate.Date &&
                                                                       x.DateTaken.Date <= endDate.Date).ToListAsync();
            return Ok(enrollments);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}