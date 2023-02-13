using System;
using System.Collections.Generic;
using System.Data;
/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 21-Nov-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Availability controller to access the availability page and has methods to set and get a user's available schedule.
*/

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAApplication.Areas.Data;
using TAApplication.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    public class AvailabilityController : Controller
    {
        private readonly TAApplicationContext _context; 
        private readonly UserManager<TAUser> _userManager;

        public AvailabilityController(TAApplicationContext context, UserManager<TAUser> userManager)
        {
            _context = context; 
            _userManager = userManager;
        }

        // Index page for availability, must be admin, profssor, or applicant to view
        [Authorize(Roles = "Admin, Professor, Applicant")]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            // find the application for the given id
            var application = await _context.Applications
                .Include(application => application.TAUser)
                .FirstOrDefaultAsync(m => m.ID == id);

            // find the user's application
            var user = await _userManager.GetUserAsync(User);
            var userApplication = await _context.Applications
                .Include(application => application.TAUser)
                .FirstOrDefaultAsync(m => m.TAUser == user);

            // If an applicant has not created an application, this redirects them to the application create page
            if (userApplication == null && await _userManager.IsInRoleAsync(user, "Applicant"))
            {
                return LocalRedirect("/Applications/Create");
            }

            if (application == null)
            {
                return NotFound();
            }

            // notify user is denied access if trying to access a different user's application
            if (await _userManager.IsInRoleAsync(user, "Applicant") && user.Id != application.TAUser.Id)
            {
                return LocalRedirect("/Identity/Account/AccessDenied");
            }

            return View(application.TAUser);
        }

        // Saves the user's schedule based on the input list
        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> SetSchedule([FromBody] List<Availability> availabilityList)
        {
            var user = await _userManager.GetUserAsync(User);
            var isAvailabilityEmpty = _context.Availability.Where(x => x.TAUser == user).Count() == 0;

            // update the user's application with the correct available hours
            int availableHours = (int)(availabilityList.Where(x => x.Available == true).Count() * .25);
            _context.Applications.Where(x => x.TAUser == user).First().AvailableHours = availableHours;

            foreach (var availability in availabilityList)
            {
                try
                {
                    // add to database if we haven't updated availability before
                    if (isAvailabilityEmpty)
                    {
                        _context.Add(availability);
                    }

                    // if we have updated availability before, update the old availability with the newly updated information
                    else
                    {
                        var oldAvailability = _context.Availability.Where(x => x.TAUser == user && x.DayOfWeek == availability.DayOfWeek && x.StartTime.TimeOfDay == availability.StartTime.TimeOfDay).First();
                        availability.ID = oldAvailability.ID;
                        availability.TAUser = user;
                        availability.CreatedBy = oldAvailability.CreatedBy;
                        availability.CreationDate = oldAvailability.CreationDate;
                        _context.Entry(oldAvailability).CurrentValues.SetValues(availability);
                    }

                    await _context.SaveChangesAsync();
                }
                catch(Exception)
                {
                    throw new Exception("Something failed");
                }

            }
            return Ok(new { success = true, message = "changed!" });
        }

        // Gets the User's availability schedule
        [HttpGet]
        [Authorize(Roles = "Admin, Professor, Applicant")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            // return the user's availability list if they are an applicant
            if (await _userManager.IsInRoleAsync(user, "Applicant"))
            {
                var availabilityList = this._context.Availability.Where(x => x.TAUser == user).ToList();
                return Ok(availabilityList);
            }

            // find the correct user's availability based on the id parameter
            else
            {
                var course = _context.Applications.Where(x => x.ID == id).Include(x => x.TAUser).First();
                var scheduleUser = course.TAUser;
                var availabilityList = this._context.Availability.Where(x => x.TAUser == scheduleUser).ToList();
                return Ok(availabilityList);
            }

        }
    }
}
