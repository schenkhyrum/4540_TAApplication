/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 22-Oct-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Course model used to update, create, and delete courses in our database. Fields are labeled with necessary
    data annotations to ensure they get validated propertly.
*/

using System.ComponentModel.DataAnnotations;
using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public enum SemesterOffered
    {
        Fall, Spring, Summer
    }

    public class Course : ModificationTracking
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Semester Offered", Prompt = "Fall")]
        public SemesterOffered SemesterOffered { get; set; }

        [Required]
        [Display(Name = "Year Offered", Prompt = "2022")]
        public int YearOffered { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Course Title", Prompt = "Web Software")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [Display(Name = "Department", Prompt = "CS")]
        public string Department { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Course Number", Prompt = "4540")]
        public int CourseNumber { get; set; }

        [Required]
        [StringLength(3)]
        [RegularExpression("^00[0-9]")]
        [Display(Name = "Course Section", Prompt = "001")]
        public string Section { get; set; } = string.Empty;

        [Required]
        [StringLength(50000)]
        [Display(Name = "Course Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(8)]
        [Display(Name = "Professor's Unid", Prompt = "u0000000")]
        [RegularExpression("^u[0-9]{7}")]
        public string ProfessorUnid { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Professor's Name")]
        public string ProfessorName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Start Time", Prompt = "3:30")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time", Prompt = "5:00")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Days Offered", Prompt = "M/W/F")]
        public string DaysOffered { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Display(Name = "Location", Prompt = "WEB L 104")]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Credit Hours", Prompt = "3")]
        public int CreditHours { get; set; }

        [Required]
        [Display(Name = "Enrollment Count", Prompt = "150")]
        public int EnrollmentCount { get; set; }

        [Display(Name = "Note")]
        [StringLength(50000)]
        public string? Note { get; set; }
    }
}