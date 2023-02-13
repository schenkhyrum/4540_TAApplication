/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 24-Sep-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Application model used to update, create, and delete applications in our database. Fields are labeled with necessary
    data annotations to ensure they get validated propertly. Contains a navigation property to the TAUser associated with
    the particular application.
*/

using System.ComponentModel.DataAnnotations;
using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public enum DegreePursuing
    {
        AS, BS, BSMS, MS, PhD
    }
    public class Application : ModificationTracking
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Degree Being Pursued")]
        public DegreePursuing DegreePursuing { get; set; }

        [Required]
        [Display(Name = "Department", Prompt = "CS")]
        public string Department { get; set; } = string.Empty;

        [Required]
        [Range(0.0, 4.0)]
        [Display(Name = "UofU GPA", Prompt = "3.5")]
        public double GPA { get; set; }

        [Required]
        [Range(5, 20)]
        [Display(Name = "Available Hours", Prompt = "10")]
        public int AvailableHours { get; set; }

        [Required]
        [Display(Name = "Available Week Before")]
        public bool AvailableWeekBefore { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Semesters Completed")]
        public int SemestersCompleted { get; set; }

        [StringLength(50000)]
        [Display(Name = "Personal Statement", Prompt = "Let us know more about you!")]
        public string? PersonalStatement { get; set; }

        [Display(Name = "Previous University", Prompt = "Weber State")]
        public string? TransferSchool { get; set; }

        [Url]
        [Display(Name = "LinkedIn URL (Optional)")]
        public string? LinkedInURL { get; set; }

        [Display(Name = "Upload Resume")]
        public string? ResumeFilename { get; set; }

        [Display(Name = "Upload Image")]
        public string? ImageFilename { get; set; }

        public TAUser TAUser { get; set; } = new TAUser();

    }
}