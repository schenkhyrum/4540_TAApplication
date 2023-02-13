/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 22-Oct-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Javascript file used on Course List page. Contains the script for initializing our data table
    and for when the admin wants to edit a note for a particular course.
*/

$(document).ready(function () {
    $('#courseTable').DataTable();
});

function buttonClick(rowID) {
    //first line extracts the last character, second line removes it
    var lastCharacter = rowID.charAt(rowID.length - 1);
    var courseID = rowID.slice(0, -1);

    // toggle the buttons
    $("#" + courseID + "1").toggle();
    $("#" + courseID + "2").toggle();

    // last character of 1 corresponds to the edit button
    if (lastCharacter == "1")
    {
        // last character of 3 corresponds to note's text, trimming to remove extra whitespace
        var htmlString = $("#" + courseID + "3").html().trim();

        // changes the html of the note from plain text to a text area that can be edited
        $("#" + courseID + "3").html('<textarea class="form-control" id="' + courseID + '4" rows="4" cols="25" >' + htmlString + "</textarea>");
    }

    // last character of 2 corresponds to the save button
    if (lastCharacter == "2")
    {
        // last character of 4 corresponds to the text area, get it's newly edited value
        var newNote = $("#" + courseID + "4").val();

        $.post({
            url: "EditNote",
            data: { courseID: courseID, newNote: newNote }
        });

        // reset html of the note from text area back to plain text
        $("#" + courseID + "3").html(newNote);
    }
}
