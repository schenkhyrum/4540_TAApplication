/*
    Author: Isaac Kletzli
    Partner: Hyrum Schenk
    Date: 24-Sep-2022
    Course: CS 4540, University of Utah, School of Computing
    Copyright:  CS 4540, Isaac Kletzli, and Hyrum Schenk - This work may not be copied for use in Academic Coursework.

    We, Isaac Kletzli and Hyrum Schenk, certify that we wrote this code from scratch and did not copy it in part or whole from
    another source. Any references used in the completion of the assignment are cited in our README file.

    File Contents

    Context that represents the database in C# code, mainly contanins a seeding method to create new users and roles
    if these users and roles do not exist yet, also contains seeding method for applications, courses, availability,
    and enrollments over time. This file also contains code to track creation and modification dates.
*/

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TAApplication.Areas.Data;
using System.Security.Claims;
using TAApplication.Models;
using TAApplication.Controllers;

namespace TAApplication.Data
{
    public class TAApplicationContext : IdentityDbContext<TAUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TAApplicationContext(DbContextOptions<TAApplicationContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Application> Applications { get; set; } = default!;
        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<Availability> Availability { get; set; } = default!;
        public DbSet<OverTimeEnrollment> OverTimeEnrollments { get; set; } = default!;

        public async Task InitializeUsers(UserManager<TAUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.Database.Migrate();

            // add roles if non-existent
            if (roleManager.Roles.Count() != 3)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("Professor"));
                await roleManager.CreateAsync(new IdentityRole("Applicant"));
            }

            // add users if non-existent
            if (userManager.Users.Count() == 0)
            {
                var user1 = new TAUser() { Email = "admin@utah.edu", Unid = "u1234567", EmailConfirmed = true, UserName = "admin@utah.edu", Name = "Admin" };
                await userManager.CreateAsync(user1);
                bool test = userManager.Users.Count() == 0;
                await userManager.AddPasswordAsync(user1, "123ABC!@#def");
                await userManager.AddToRoleAsync(user1, "Admin");

                var user2 = new TAUser() { Email = "professor@utah.edu", Unid = "u7654321", EmailConfirmed = true, UserName = "professor@utah.edu", Name = "Professor" };
                await userManager.CreateAsync(user2);
                await userManager.AddPasswordAsync(user2, "123ABC!@#def");
                await userManager.AddToRoleAsync(user2, "Professor");

                var user3 = new TAUser() { Email = "u0000000@utah.edu", Unid = "u0000000", EmailConfirmed = true, UserName = "u0000000@utah.edu", Name = "Applicant0" };
                await userManager.CreateAsync(user3);
                await userManager.AddPasswordAsync(user3, "123ABC!@#def");
                await userManager.AddToRoleAsync(user3, "Applicant");

                var user4 = new TAUser() { Email = "u0000001@utah.edu", Unid = "u0000001", EmailConfirmed = true, UserName = "u0000001@utah.edu", Name = "Applicant1" };
                await userManager.CreateAsync(user4);
                await userManager.AddPasswordAsync(user4, "123ABC!@#def");
                await userManager.AddToRoleAsync(user4, "Applicant");

                var user5 = new TAUser() { Email = "u0000002@utah.edu", Unid = "u0000002", EmailConfirmed = true, UserName = "u0000002@utah.edu", Name = "Applicant2" };
                await userManager.CreateAsync(user5);
                await userManager.AddPasswordAsync(user5, "123ABC!@#def");
                await userManager.AddToRoleAsync(user5, "Applicant");
            }
        }

        public async Task InitializeUserApplications(UserManager<TAUser> userManager)
        {
            // Look for any applications.
            if (this.Applications.Any())
            {
                return;   // DB has been seeded
            }

            var user1 = await userManager.FindByEmailAsync("u0000000@utah.edu");
            var user1Application = new Application()
            {
                AvailableHours = 20,
                AvailableWeekBefore = true,
                DegreePursuing = DegreePursuing.PhD,
                Department = "CS",
                GPA = 4.0,
                LinkedInURL = "https://www.linkedin.com/u0000000",
                PersonalStatement = "I am smart",
                SemestersCompleted = 6,
                TAUser = user1,
                TransferSchool = "Weber State"
            };

            var user2 = await userManager.FindByEmailAsync("u0000001@utah.edu");
            var user2Application = new Application()
            {
                AvailableHours = 5,
                AvailableWeekBefore = false,
                DegreePursuing = DegreePursuing.AS,
                Department = "None",
                GPA = 0.0,
                SemestersCompleted = 0,
                TAUser = user2
            };

            this.Applications.Add(user1Application);
            this.Applications.Add(user2Application);
            this.SaveChanges();
        }

        public async Task InitializeCourses()
        {
            // Look for any course.
            if (this.Courses.Any())
            {
                return;   // DB has been seeded
            }

            var course1 = new Course()
            {
                CourseNumber = 1400,
                CreditHours = 3,
                DaysOffered = "M/W/F",
                Department = "CS",
                Description = "Good Class",
                EndTime = DateTime.Parse("5:00 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note",
                ProfessorName = "John Smith",
                ProfessorUnid = "u0000007",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("3:30 PM").ToUniversalTime(),
                Title = "Intro Comp Programming",
                YearOffered = 2023
            };

            var course2 = new Course()
            {
                CourseNumber = 2420,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Not so good class",
                EndTime = DateTime.Parse("7:00 PM").ToUniversalTime(),
                EnrollmentCount = 175,
                Location = "WEB L 103",
                ProfessorName = "Ben Johnson",
                ProfessorUnid = "u0000009",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("5:15 PM").ToUniversalTime(),
                Title = "Data Structures",
                YearOffered = 2023
            };

            var course3 = new Course()
            {
                CourseNumber = 3500,
                CreditHours = 3,
                DaysOffered = "M/Th/F",
                Department = "CS",
                Description = "Improve software skills",
                EndTime = DateTime.Parse("3:00 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                ProfessorName = "John Smith",
                ProfessorUnid = "u0000007",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("2:15 PM").ToUniversalTime(),
                Title = "Software Practice",
                YearOffered = 2023
            };

            var course4 = new Course()
            {
                CourseNumber = 4150,
                CreditHours = 3,
                DaysOffered = "M/W",
                Department = "CS",
                Description = "Learn to write algorithms",
                EndTime = DateTime.Parse("10:50 AM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 105",
                Note = "This is another note",
                ProfessorName = "Jeff Jones",
                ProfessorUnid = "u0000123",
                Section = "002",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("9:30 AM").ToUniversalTime(),
                Title = "Algorithms",
                YearOffered = 2023
            };

            var course5 = new Course()
            {
                CourseNumber = 4400,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Hard Class",
                EndTime = DateTime.Parse("1:45 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note for CS 4400",
                ProfessorName = "Steve Smith",
                ProfessorUnid = "u0000014",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("12:25 PM").ToUniversalTime(),
                Title = "Computer Systems",
                YearOffered = 2023
            };

            var course6 = new Course()
            {
                CourseNumber = 1410,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Hard Class",
                EndTime = DateTime.Parse("1:45 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note for CS 4400",
                ProfessorName = "Steve Smith",
                ProfessorUnid = "u0000014",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("12:25 PM").ToUniversalTime(),
                Title = "Computer Systems",
                YearOffered = 2023
            };

            var course7 = new Course()
            {
                CourseNumber = 1420,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Hard Class",
                EndTime = DateTime.Parse("1:45 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note for CS 4400",
                ProfessorName = "Steve Smith",
                ProfessorUnid = "u0000014",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("12:25 PM").ToUniversalTime(),
                Title = "Computer Systems",
                YearOffered = 2023
            };

            var course8 = new Course()
            {
                CourseNumber = 2100,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Hard Class",
                EndTime = DateTime.Parse("1:45 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note for CS 4400",
                ProfessorName = "Steve Smith",
                ProfessorUnid = "u0000014",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("12:25 PM").ToUniversalTime(),
                Title = "Computer Systems",
                YearOffered = 2023
            };

            var course9 = new Course()
            {
                CourseNumber = 3100,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Hard Class",
                EndTime = DateTime.Parse("1:45 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note for CS 4400",
                ProfessorName = "Steve Smith",
                ProfessorUnid = "u0000014",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("12:25 PM").ToUniversalTime(),
                Title = "Computer Systems",
                YearOffered = 2023
            };

            var course10 = new Course()
            {
                CourseNumber = 4480,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Hard Class",
                EndTime = DateTime.Parse("1:45 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note for CS 4400",
                ProfessorName = "Steve Smith",
                ProfessorUnid = "u0000014",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("12:25 PM").ToUniversalTime(),
                Title = "Computer Systems",
                YearOffered = 2023
            };

            var course11 = new Course()
            {
                CourseNumber = 4500,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Hard Class",
                EndTime = DateTime.Parse("1:45 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note for CS 4400",
                ProfessorName = "Steve Smith",
                ProfessorUnid = "u0000014",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("12:25 PM").ToUniversalTime(),
                Title = "Computer Systems",
                YearOffered = 2023
            };

            var course12 = new Course()
            {
                CourseNumber = 4530,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Hard Class",
                EndTime = DateTime.Parse("1:45 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note for CS 4400",
                ProfessorName = "Steve Smith",
                ProfessorUnid = "u0000014",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("12:25 PM").ToUniversalTime(),
                Title = "Computer Systems",
                YearOffered = 2023
            };

            var course13 = new Course()
            {
                CourseNumber = 3200,
                CreditHours = 3,
                DaysOffered = "Tu/Th",
                Department = "CS",
                Description = "Hard Class",
                EndTime = DateTime.Parse("1:45 PM").ToUniversalTime(),
                EnrollmentCount = 100,
                Location = "WEB L 104",
                Note = "This is a note for CS 4400",
                ProfessorName = "Steve Smith",
                ProfessorUnid = "u0000014",
                Section = "001",
                SemesterOffered = SemesterOffered.Spring,
                StartTime = DateTime.Parse("12:25 PM").ToUniversalTime(),
                Title = "Computer Systems",
                YearOffered = 2023
            };

            this.Courses.Add(course1);
            this.Courses.Add(course2);
            this.Courses.Add(course3);
            this.Courses.Add(course4);
            this.Courses.Add(course5);
            this.Courses.Add(course6);
            this.Courses.Add(course7);
            this.Courses.Add(course8);
            this.Courses.Add(course9);
            this.Courses.Add(course10);
            this.Courses.Add(course11);
            this.Courses.Add(course12);
            this.Courses.Add(course13);
            await this.SaveChangesAsync();
        }

        public async Task InitializeAvailability(UserManager<TAUser> userManager)
        {
            // Look for any availability.
            if (this.Availability.Any())
            {
                return;   // DB has been seeded
            }

            var user1 = await userManager.FindByEmailAsync("u0000001@utah.edu");

            for (int i = 0; i < 240; i++)
            {
                var time = 8 + 0.25 * (i % 48);
                int hour = (int)time;
                var minute = 60 * (time - hour);

                var hourString = hour.ToString();
                if (hour < 10)
                {
                    hourString = $"0{hour}";
                }

                var minuteString = "00";
                if (minute > 0)
                {
                    minuteString = $"{minute}";
                }

                var availabilty = new Availability()
                {
                    Available = i % 48 == 0, // first slot on each day is available
                    DayOfWeek = (Models.DayOfWeek)(i / 48),
                    StartTime = DateTime.Parse($"{hourString}:{minuteString}"),
                    TAUser = user1
                };
                this.Availability.Add(availabilty);
            }

            var user2 = await userManager.FindByEmailAsync("u0000000@utah.edu");

            // add monday, friday
            for (int j = 0; j <= 4; j += 4)
            {
                for (int i = 0; i < 48; i++)
                {
                    var time = 8 + 0.25 * i;
                    int hour = (int)time;
                    var minute = 60 * (time - hour);

                    var hourString = hour.ToString();
                    if (hour < 10)
                    {
                        hourString = $"0{hour}";
                    }

                    var minuteString = "00";
                    if (minute > 0)
                    {
                        minuteString = $"{minute}";
                    }

                    var availabilty = new Availability()
                    {
                        Available = i < 16 ? true : false, // adds from 8 - noon
                        DayOfWeek = (Models.DayOfWeek)(j),
                        StartTime = DateTime.Parse($"{hourString}:{minuteString}"),
                        TAUser = user2
                    };
                    this.Availability.Add(availabilty);
                }
            }

            // add tuesday, thursday
            for (int j = 1; j <= 3; j += 2)
            {
                for (int i = 0; i < 48; i++)
                {
                    var time = 8 + 0.25 * i;
                    int hour = (int)time;
                    var minute = 60 * (time - hour);

                    var hourString = hour.ToString();
                    if (hour < 10)
                    {
                        hourString = $"0{hour}";
                    }

                    var minuteString = "00";
                    if (minute > 0)
                    {
                        minuteString = $"{minute}";
                    }

                    var availabilty = new Availability()
                    {
                        Available = i > 15 && i < 36 ? true : false, // adds from noon - 5
                        DayOfWeek = (Models.DayOfWeek)(j),
                        StartTime = DateTime.Parse($"{hourString}:{minuteString}"),
                        TAUser = user2
                    };
                    this.Availability.Add(availabilty);
                }
            }

            // add wednesday
            for (int i = 0; i < 48; i++)
            {
                var time = 8 + 0.25 * i;
                int hour = (int)time;
                var minute = 60 * (time - hour);

                var hourString = hour.ToString();
                if (hour < 10)
                {
                    hourString = $"0{hour}";
                }

                var minuteString = "00";
                if (minute > 0)
                {
                    minuteString = $"{minute}";
                }

                var availabilty = new Availability()
                {
                    Available = false,
                    DayOfWeek = Models.DayOfWeek.Wednesday,
                    StartTime = DateTime.Parse($"{hourString}:{minuteString}"),
                    TAUser = user2
                };
                this.Availability.Add(availabilty);
            }

            await this.SaveChangesAsync();
        }

        public async Task InitializeEnrollmentsOverTime()
        {
            //// Look for any OverTimeEnrollments entry.
            if (this.OverTimeEnrollments.Any())
            {
                return;   // DB has been seeded
            }

            // read the csv data
            using (var reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Data", "EnrollmentSeedData.csv")))
            {
                // read header line
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var course = values[0].Split(' ');

                    // add the enrollment over time entry
                    for (int i = 1; i < values.Length; i++)
                    {
                        var enrollment = new OverTimeEnrollment()
                        {
                            EnrollmentCount = int.Parse(values[i]),
                            Course = this.Courses.Where(x => x.CourseNumber == int.Parse(course[1]) && x.Department == course[0]).First(),
                            DateTaken = DateTime.Parse("11/01/2022").AddDays(i - 1)
                        };
                        this.OverTimeEnrollments.Add(enrollment);
                    }
                }
            }
            await this.SaveChangesAsync();
        }

        /// <summary>
        /// Every time Save Changes is called, add timestamps
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()  // JIM: Override save changes to add timestamps
        {
            AddTimestamps();
            return base.SaveChanges();
        }
        /// <summary>
        /// Every time Save Changes (Async) is called, add timestamps
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            AddTimestamps();   // JIM: Override save changes async to add timestamps
            return await base.SaveChangesAsync(cancellationToken);
        }
        /// <summary>
        /// JIM: this code adds time/user to DB entry
        /// 
        /// Check the DB tracker to see what has been modified, and add timestamps/names as appropriate.
        /// 
        /// </summary>
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is ModificationTracking
                        && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var currentUsername = "";

            if (_httpContextAccessor.HttpContext == null) // happens during startup/initialization code
            {
                currentUsername = "DBSeeder";
            }
            else
            {
                currentUsername = _httpContextAccessor.HttpContext.User.Identity?.Name;
            }

            currentUsername ??= "Sadness"; // JIM: compound assignment magic... test for null, and if so, assign value

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((ModificationTracking)entity.Entity).CreationDate = DateTime.UtcNow;
                    ((ModificationTracking)entity.Entity).CreatedBy = currentUsername;
                }
                ((ModificationTracking)entity.Entity).ModificationDate = DateTime.UtcNow;
                ((ModificationTracking)entity.Entity).ModifiedBy = currentUsername;
            }
        }
    }
}