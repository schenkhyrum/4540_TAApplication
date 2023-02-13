/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 3-Oct-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Old controller for handwritten ApplicationDetails, ApplicationCreate, and ApplicationList pages.
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TAApplication.Controllers
{
    [Authorize]
    public class OLDController : Controller
    {
        private readonly ILogger<OLDController> _logger;

        public OLDController(ILogger<OLDController> logger)
        {
            _logger = logger;
        }

        public IActionResult ApplicationCreate()
        {
            return View();
        }

        public IActionResult ApplicationDetails()
        {
            return View();
        }

        public IActionResult ApplicationList()
        {
            return View();
        }
    }
}