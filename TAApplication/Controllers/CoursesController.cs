/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 22-Oct-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Courses controller to access edit, delete, create, and list pages. Handles saving and updating courses
    in the database and also handles updating a course's note via AJAX.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TAApplication.Data;
using TAApplication.Models;

namespace TAApplication.Controllers
{
    public class CoursesController : Controller
    {
        private readonly TAApplicationContext _context;

        public CoursesController(TAApplicationContext context)
        {
            _context = context;
        }

        // GET: Courses
        [Authorize(Roles = "Admin, Professor")]
        public async Task<IActionResult> List()
        {
              return View(await _context.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        [Authorize(Roles = "Admin, Professor, Applicant")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ID,SemesterOffered,YearOffered,Title,Department,CourseNumber,Section,Description,ProfessorUnid,ProfessorName,StartTime,EndTime,DaysOffered,Location,CreditHours,EnrollmentCount,Note")] Course course,
            bool monday, bool tuesday, bool wednesday, bool thursday, bool friday)
        {
            // Form the daysOffered string based on the values checked from the checkboxes
            string daysOffered = string.Empty;
            if (monday) { daysOffered = "M/"; }
            if (tuesday) { daysOffered += "Tu/"; }
            if (wednesday) { daysOffered += "W/"; }
            if (thursday) { daysOffered += "Th/"; }
            if (friday) { daysOffered += "F/"; }

            // if any days were checked, remove the last slash from the string and
            // remove the ModelState DaysOffered so the ModelState will be valid
            if (daysOffered != string.Empty)
            {
                ModelState.Remove("DaysOffered");
                course.DaysOffered = daysOffered.Remove(daysOffered.Length - 1);
            }

            // convert start and end times to UTC
            course.StartTime = course.StartTime.ToUniversalTime();
            course.EndTime = course.EndTime.ToUniversalTime();

            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // Called when user clicks save on the edit page to update course
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
            
        public async Task<IActionResult> EditPost(int? id, bool monday, bool tuesday, bool wednesday, bool thursday, bool friday)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var courseToUpdate = _context.Courses
                .Where(o => o.ID == id).FirstOrDefault();

            // form the daysOffered string based on checkboxes
            string daysOffered = string.Empty;
            if (monday) { daysOffered = "M/"; }
            if (tuesday) { daysOffered += "Tu/"; }
            if (wednesday) { daysOffered += "W/"; }
            if (thursday) { daysOffered += "Th/"; }
            if (friday) { daysOffered += "F/"; }

            if (courseToUpdate != null)
            {
                // if any days were checked, remove the last slash from the string and
                // remove the ModelState DaysOffered so the ModelState will be valid
                if (daysOffered != string.Empty)
                {
                    ModelState.Remove("DaysOffered");
                    courseToUpdate.DaysOffered = daysOffered.Remove(daysOffered.Length - 1);
                }

                if (await TryUpdateModelAsync<Course>(courseToUpdate, "",
                    s => s.SemesterOffered,
                    s => s.YearOffered,
                    s => s.Title,
                    s => s.Department,
                    s => s.CourseNumber,
                    s => s.Section,
                    s => s.Description,
                    s => s.ProfessorUnid,
                    s => s.ProfessorName,
                    s => s.StartTime,
                    s => s.EndTime,
                    s => s.DaysOffered,
                    s => s.Location,
                    s => s.CreditHours,
                    s => s.EnrollmentCount,
                    s => s.Note))
                {
                    try
                    {
                        // convert start and end times to UTC
                        courseToUpdate.StartTime = courseToUpdate.StartTime.ToUniversalTime();
                        courseToUpdate.EndTime = courseToUpdate.EndTime.ToUniversalTime();
                        _context.SaveChanges();
                        return RedirectToAction("Details", new { id = courseToUpdate.ID });
                    }
                    catch (DataException)
                    {
                        ViewData["ErrorMessage"] = "Something went wrong while attempting to save data. Please try again.";
                        return View(courseToUpdate);
                    }
                }
            }
            return View(courseToUpdate);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'TAApplicationContext.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }

        // Called when an admin clicks to save a note on the course list page
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditNote(int courseID, string newNote)
        {
            var course = await _context.Courses.FindAsync(courseID);
            if (course == null)
            {
                return BadRequest("The course was found with the given id.");
            }

            course.Note = newNote;
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "changed!" });
        }

        private bool CourseExists(int id)
        {
          return _context.Courses.Any(e => e.ID == id);
        }
    }
}
