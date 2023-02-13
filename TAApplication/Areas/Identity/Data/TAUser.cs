/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 24-Sep-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    TAUser inherits from the IdentityUser to determine authentication and authorization. Basic information
    such as passwords and emails come from IdentityUser, while this adds on Unid, Name, and ReferredTo on top.
    Unid is required and must be unique, Name is required but not unique, and ReferredTo is not required.
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TAApplication.Areas.Data
{
    [Index(nameof(Unid), IsUnique = true)]
    public class TAUser : IdentityUser
    {
        public string Unid { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? RefferedTo { get; set; }
    }
}
