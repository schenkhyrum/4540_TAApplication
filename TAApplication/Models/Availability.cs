/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 10-Nov-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Availability model to hold a slot the user is available for. This model stores the start time,
    day of week, the user associated with the slot, and whether or not the user is available for the slot
*/

using System.ComponentModel.DataAnnotations;
using TAApplication.Areas.Data;

namespace TAApplication.Models
{
    public enum DayOfWeek
    {
        Monday, Tuesday, Wednesday, Thursday, Friday
    }
    public class Availability: ModificationTracking
    {
        public int ID { get; set; }
        public DateTime StartTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public bool Available { get; set; }
        public TAUser TAUser { get; set; } = new TAUser();
    }
}