```
Author:     Isaac Kletzli
Partner:    Hyrum Schenk
Date:       3-Dec-2022
Course:     CS 4540, University of Utah, School of Computing
GitHub ID:  ikletzli and schenkhyrum
Repo:       https://github.com/uofu-cs4540-fall2022/taapplication-gitgud-2-0
Commit Tag: PS9FinalSubmission
Project:    TA Application
Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.
Isaac's Instance on AWS: N/A
Isaac's URL to access instance: N/A
Hyrum's Instance on AWS: N/A
Hyrum's URL to access instance: N/A
```
# Overview of the TA Application Functionality 

The TA Application has all the same functionality as the previous projects, but now has several additional features. The main
capability added to this project was adding an enrollment trends page so an admin can view enrollments over time data. This was
accomplished using HighCharts, and allows the admin to specify a start date, end date, and course to get data for. The admin can
also choose the type of chart they want to visualize the data in, and choose between a light and dark mode.

# Comments to Evaluators:

None specifically, the app should work as designed

# Assignment Specific Topics

Above and Beyond paragraph - The two extra items we chose to add to our enrollment trends page are the ability to switch the
                             type of chart, and the ability to switch to a dark mode (numbers 1 and 3 on the assignment). To
                             accomplish switching chart types, we added a dropdown to select between 5 different chart types
                             (Area Spline, Bar, Column, Scatter, and Spline). Then when the user changes the selection, a 
                             JavaScript function is called that changes the chart type. For changing between light and dark modes,
                             we added a toggle button that when clicked, calls a JavaScript function to change the lighting mode.
                             This function determines what type of mode the user selected, and updates the HighCharts chart to reflect
                             this selection (dark background with white text, or white background with dark text). This function also 
                             changes the background of the page itself and the text on the page to show this change.

# Consulted Peers:

We did not have outside help from other peers in the class, because we were able to figure out our questions on our own, and through
the help of Google.

# Peers Helped:

We did not help any peers as we were intently focused on finishing and polishing our assignment.

# Acknowledgements:

Special thanks to Stack Overflow for all our programming questions.

# References:

    1. HTML Tutorial - https://www.w3schools.com/html/default.asp
    2. CSS Tutorial - https://www.w3schools.com/css/
    3. Html / CSS : image taking half of the screen - https://stackoverflow.com/questions/22321466/html-css-image-taking-half-of-the-screen
    5. Right div on float:right appears a line below the left one - https://stackoverflow.com/questions/17525371/right-div-on-floatright-appears-a-line-below-the-left-one
    5. How to set the default value for an HTML <select> element ? - GeeksforGeeks - https://www.geeksforgeeks.org/how-to-set-the-default-value-for-an-html-select-element/
    6. Introduction Bootstrap v5.0 - https://getbootstrap.com/docs/5.0/getting-started/introduction/
    7. Bootstrap 5 Tutorial - https://www.w3schools.com/bootstrap5/index.php
    8. Entity Framework error: There is already an open DataReader associated with this Command - https://stackoverflow.com/questions/66690147/entity-framework-error-there-is-already-an-open-datareader-associated-with-this
    9. Check if checkbox is NOT checked on click - jQuery - https://stackoverflow.com/questions/11159221/check-if-checkbox-is-not-checked-on-click-jquery 
    10. How to remove the last character of a string in JavaScript - https://flaviocopes.com/how-to-remove-last-char-string-js/
    11. Access HttpContextAccessor from startup.cs in .net Core WebApi - https://stackoverflow.com/questions/52927463/access-httpcontextaccessor-from-startup-cs-in-net-core-webapi
    12. RedirectToAction with parameter - https://stackoverflow.com/questions/1257482/redirecttoaction-with-parameter
    13. keep viewdata on RedirectToAction - https://stackoverflow.com/questions/1226329/keep-viewdata-on-redirecttoaction
    14. Pass values of checkBox to controller action in asp.net mvc4 - https://stackoverflow.com/questions/18862712/pass-values-of-checkbox-to-controller-action-in-asp-net-mvc4
    15. How to get only time from date-time C# - https://stackoverflow.com/questions/1026841/how-to-get-only-time-from-date-time-c-sharp
    16. How to get the last character of a string in JavaScript? - https://www.geeksforgeeks.org/how-to-get-the-last-character-of-a-string-in-javascript/#:~:text=length%20function.,the%20last%20character%20of%20string
    17. How to get the value of a textarea in jQuery - https://www.tutorialrepublic.com/faq/how-to-get-the-value-of-a-textarea-in-jquery.php
    18. Simple lines with Pixi.js - https://www.html5gamedevs.com/topic/28098-simple-lines-with-pixijs/
    19. Asp.net mvc passing a C# object to Javascript - https://stackoverflow.com/questions/8145716/asp-net-mvc-passing-a-c-sharp-object-to-javascript
    20. Where are @Json.Encode or @Json.Decode methods in MVC 6? - https://stackoverflow.com/questions/30301930/where-are-json-encode-or-json-decode-methods-in-mvc-6
    21. How can I get last characters of a string - https://stackoverflow.com/questions/5873810/how-can-i-get-last-characters-of-a-string
    22. How to pass a javascript object to a C# MVC 4 controller - https://stackoverflow.com/questions/27829340/how-to-pass-a-javascript-object-to-a-c-sharp-mvc-4-controller
    23. Jquery - How to make $.post() use contentType=application/json? - https://stackoverflow.com/questions/2845459/jquery-how-to-make-post-use-contenttype-application-json
    24. How to send a list of objects to controller via Ajax using asp net core 3.1? - https://stackoverflow.com/questions/65081966/how-to-send-a-list-of-objects-to-controller-via-ajax-using-asp-net-core-3-1
    25. Find object by id in an array of JavaScript objects - https://stackoverflow.com/questions/7364150/find-object-by-id-in-an-array-of-javascript-objects
    26. Reading CSV file and storing values into an array - https://stackoverflow.com/questions/5282999/reading-csv-file-and-storing-values-into-an-array
    27. Unable to get right file path through StreamReader - https://stackoverflow.com/questions/10672818/unable-to-get-right-file-path-through-streamreader
    28. Select Lists in a Razor Pages Form - https://www.learnrazorpages.com/razor-pages/forms/select-lists
    29. How do I get the current date in JavaScript? - https://stackoverflow.com/questions/1531093/how-do-i-get-the-current-date-in-javascript
    30. how to add specific x and y data? - https://www.highcharts.com/forum/viewtopic.php?t=20964
    31. JSFiddle for datetime with HighCharts - http://jsfiddle.net/BGurung/grVFk/12704/
    32. JSFiddle for updating series in HighCharts - http://jsfiddle.net/phpdeveloperrahul/5bA5R/
    33. JSFiddle for updating chart type in HighCharts - https://jsfiddle.net/gh/get/library/pure/highcharts/highcharts/tree/master/samples/highcharts/demo/chart-update
    34. Incrementing a date in JavaScript - https://stackoverflow.com/questions/3674539/incrementing-a-date-in-javascript
    35. Convert UTC Epoch to local date - https://stackoverflow.com/questions/4631928/convert-utc-epoch-to-local-date
    36. Highcharts Custom tooltips for multiple series - https://stackoverflow.com/questions/39208289/highcharts-custom-tooltips-for-multiple-series

    Assignment 1 - Consulted W3schools CSS and HTML websites extensively
    Assignment 2 - Consulted W3schools Bootstrap tutorial extensively as well as the Bootstrap docs
    Assignment 5 - Consulted the Contoso Tutorial extensively
    Assignment 6 - Consulted Mozilla Docs and Microsoft Docs occassionally
    Assignment 8 - Consulted Mozilla Docs, W3Schools, and Stack Overflow to look at documentation for various JavaScript methods
    Assignment 9 - Consulted JQuery Docs, JQuery UI Docs, and HighCharts docs to look at documentation
# Time Expenditures:

    1. Assignment One: Predicted Hours: 10 Actual Hours: 9
    2. Assignment Two: Predicted Hours: 10 Actual Hours: 15
    3. Assignment Four: Predicted Hours: 12 Actual Hours: 22
    4. Assignment Five: Predicted Hours: 12 Actual Hours: 16
    5. Assignment Six: Predicted Hours: 10 Actual Hours: 13
    6. Assignment Eight: Predicted Hours: 15 Actual Hours: 20
    7. Assignment Nine: Predicted Hours: 15 Actual Hours: 13
