/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 21-Nov-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Javascript file used on Availability page. Creates a GUI using Pixi.js to display a user's available hours.
    Applicants can edit their hours by dragging and clicking their mouse on the GUI. Admins and professors can
    also see a user's hours, but cannot edit them.
*/

mouse_down = false;
slotWidth = 80;
slotHeight = 10;
allAvailability = [];
dataNotSaved = false;

class AvailabilityTracker extends PIXI.Graphics {
    constructor() {
        super();
        this.xOffset = 30;
        this.topOffset = 100;
        this.timeSlotList = [];

        this.buildAvailability();
        this.buildGUI();
        this.getUserAvailability();
    }

    // builds a list of time slots all initialized to unavailable
    buildAvailability() {
        for (let i = 0; i < 240; i++) {

            var positionInColumn = i % 48;
            var time = 8 + 0.25 * positionInColumn;
            var hour = Math.floor(time);
            var minute = 60 * (time - hour);

            var hourString = hour;
            if (hour < 10) {
                hourString = `0${hour}`
            }

            var minuteString = "00";
            if (minute > 0) {
                minuteString = `${minute}`;
            }

            allAvailability.push({
                available: false,
                dayOfWeek: Math.floor(i / 48),
                id: i,
                startTime: `2022-11-20T${hourString}:${minuteString}:00`,
                taUser: null
            });
        }
    }

    // builds the availability GUI
    buildGUI() {
        this.buildTimeSlots();
        this.drawLines();
        this.drawDayText();
        this.drawTimeText();
        this.drawFooterText();
    }

    // builds the list of slots needed to draw on the GUI
    buildTimeSlots() {
        for (let i = 0; i < 5; i++) {
            var offset = 0;
            for (let j = 0; j < 48; j++) {

                // add one each 4 slots to account for the lines
                if (j % 4 == 0) {
                    offset++;
                }

                var timeSlot = new PIXI.Graphics();
                timeSlot.color = 0xfedcba;
                timeSlot.x = (slotWidth + this.xOffset) * i + this.xOffset;
                timeSlot.y = slotHeight * j + this.topOffset + offset;
                timeSlot.id = i * 48 + j;
                timeSlot.available = false;

                this.timeSlotList.push(timeSlot);
            }
        }
    }

    // draws the time slots on the GUI, after we got the user's current availability
    drawTimeSlots(obj) {
        for (let i = 0; i < obj.timeSlotList.length; i++) {

            var timeSlot = obj.timeSlotList[i];
            timeSlot.beginFill(timeSlot.color);
            timeSlot.drawRect(0, 0, slotWidth, slotHeight);
            timeSlot.interactive = interactive;

            obj.addChild(timeSlot);

            timeSlot.on('mousedown', obj.pointer_down);
            timeSlot.on('mouseover', obj.pointer_over);
            timeSlot.on('mouseup', obj.pointer_up);
        }
    }

    // draw the lines separating hours on the GUI, lines are white and extend
    // from left to right
    drawLines() {
        for (let i = 0; i < 13; i++) {

            var line = new PIXI.Graphics();
            line.lineStyle(1, 0xffffff);

            line.moveTo(this.xOffset - 25, this.topOffset + (4 * slotHeight + 1) * i);
            line.lineTo((this.xOffset + slotWidth) * 5 + 25, this.topOffset + (4 * slotHeight + 1) * i);

            this.addChild(line);
        }

        return line;
    }

    // draws the days of the week on the GUI
    drawDayText() {
        let daysArray = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
        for (let i = 0; i < daysArray.length; i++) {

            let text_sprite = new PIXI.Text(daysArray[i], { fill: 0xffffff, fontSize: 18 });
            text_sprite.x = this.xOffset + i * (slotWidth + this.xOffset);
            text_sprite.y = this.topOffset - 30;

            this.addChild(text_sprite);
        }
    }

    // draws the time intervals on the GUI
    drawTimeText() {
        let init_time = 8;
        let ampm = "am";

        for (let i = 0; i < 13; i++) {
            if (init_time == 12) { ampm = "pm"; }

            let text_string = `${init_time}:00 ${ampm}`;
            let text_sprite = new PIXI.Text(text_string, { fill: 0xffffff, fontSize: 18 });

            text_sprite.x = (this.xOffset + slotWidth) * 5 + 35;
            text_sprite.y = this.topOffset - 10 + (4 * slotHeight + 1) * i;

            if (init_time == 12) { init_time = 0; }
            init_time++;

            this.addChild(text_sprite);
        }
    }

    // draws the footer on the GUI, will only show up if the user is an applicant
    drawFooterText() {
        let text_string = "Click and drag to set/un-set available times.";
        if (!interactive) {
            text_string = "";
        }

        let text_sprite = new PIXI.Text(text_string, { fill: 0xffffff, fontSize: 12 });
        text_sprite.x = 10;
        text_sprite.y = this.topOffset + 13 + slotHeight * 48 + 20;

        this.addChild(text_sprite);
    }

    // method called when the mouse is clicked, updates the color and 
    // if the slot is available or not
    pointer_down() {
        this.clear();
        var color;

        if (this.available) {
            color = 0xfedcba;
            this.available = false;
        }

        else {
            color = 0x567899;
            this.available = true;
        }

        this.beginFill(color);
        this.drawRect(0, 0, slotWidth, slotHeight);

        allAvailability.find(x => x.id === this.id).available = this.available;
        mouse_down = true;

        $('#saveWarning').text("Warning: Data Not Saved");
        dataNotSaved = true;
    }

    // method called when mouse is released
    pointer_up() {
        mouse_down = false;
    }

    // method called when pointer is over a time slot, if the mouse
    // is clicked, then we will change the availability
    pointer_over() {

        if (mouse_down) {

            this.clear();
            var color;

            if (this.available) {
                color = 0xfedcba;
                this.available = false;
            }

            else {
                color = 0x567899;
                this.available = true;
            }

            this.beginFill(color);
            this.drawRect(0, 0, slotWidth, slotHeight);

            allAvailability.find(x => x.id === this.id).available = this.available;
        }
    }

    // saves the changes the user has made
    saveChanges() {
        $('#saveButton').hide();
        $('#spinner').show();

        if (dataNotSaved) {
            $.post({
                contentType: "application/json; charset=utf-8",
                url: '/Availability/SetSchedule',
                data: JSON.stringify(allAvailability)

            }).always(function () {
                $('#spinner').hide();
                $('#saveButton').show();
                $('#saveWarning').text("");
                dataNotSaved = false;
            });
        }
    }

    // gets the user's current availability and updates/draws time slots accordingly
    getUserAvailability() {
        var obj = this;
        $.get({
            url: `/Availability/GetSchedule/${userId}`
        }).done(function (response) {
            obj.updateAvailability(obj, response);
            obj.drawTimeSlots(obj);
        });
    }

    updateAvailability(obj, userAvailability) {
        var availableSlots = userAvailability.filter(x => x.available == true);
        for (let i = 0; i < availableSlots.length; i++) {

            // last 8 characters are relevant for time
            var time = availableSlots[i].startTime.slice(-8);
            var timeInfo = time.split(':');

            // get id based on hour, minute, and day
            var hourOffset = (parseInt(timeInfo[0]) - 8) * 4;
            var minuteOffset = parseInt(timeInfo[1]) / 15;
            var id = availableSlots[i].dayOfWeek * 48 + hourOffset + minuteOffset;

            var square = obj.timeSlotList.find(x => x.id === id);
            square.color = 0x567899;
            square.available = true;

            allAvailability.find(x => x.id === id).available = true;
        }
    }
}

setup();

function setup() {

    // build app and put into html
    app = new PIXI.Application({ backgroundColor: 0x111111 });
    app.renderer.resize(700, 700);
    $("#canvas_div").append(app.view);

    // background that fills canvas so when a user lets go of mouse anywhere on 
    // background, mouse down gets set to false
    var background = new PIXI.Graphics();
    background.beginFill(0x111111);
    background.drawRect(0, 0, 700, 700);
    background.interactive = interactive;
    background.available = false;
    app.stage.addChild(background);
    
    // create new availability tracker to set up GUI and add to stage
    availabilityTracker = new AvailabilityTracker(app);
    app.stage.addChild(availabilityTracker);

    background.on('mouseup', availabilityTracker.pointer_up);
}