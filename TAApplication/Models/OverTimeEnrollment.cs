/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 2-Dec-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    OverTimeEnrollment model that associates a course with an enrollment count, taken on a specific day.
*/

using System.ComponentModel.DataAnnotations;
using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public class OverTimeEnrollment
    {
        public int ID { get; set; }
        public DateTime DateTaken { get; set; }
        public int EnrollmentCount { get; set; }
        public Course Course { get; set; } = new Course();
    }
}