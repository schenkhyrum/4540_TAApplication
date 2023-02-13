/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 24-Sep-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Javascript file used on Admin Role Manager page. Contains the script for initializing our data table
    and for when the admin wants to add a remove a role from a user
*/


$(document).ready(function () {
    $('#roleManagerTable').DataTable();
});

function addRemoveRole(role, rowID) {
    //removes the last character from the string
    var userId = rowID.slice(0, -1);

    var addOrRemove;
    if ($("#" + rowID).is(':checked')) { addOrRemove = "add"; }
    else { addOrRemove = "remove"; }

    $.post({
        url: "Admin/EditRole",
        data: { role: role, userId: userId, addOrRemove: addOrRemove }
    });
}
