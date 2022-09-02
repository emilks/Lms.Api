using Bogus;
using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data
{
    public class SeedData
    {
        private static LmsApiContext db = default!;

        public static async Task InitAsync(LmsApiContext context, IServiceProvider services)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            db = context;

            ArgumentNullException.ThrowIfNull(nameof(services));
            if (services is null) throw new ArgumentNullException(nameof(services));

            if (db.Course.Any()) return;

            //SeedModulesAndCoursesAsync();

            var courses = SeedCoursesAsync();
            await db.AddRangeAsync(courses);

            await db.SaveChangesAsync();

            //var modules = SeedModulesAsync(courses);
            //await db.AddRangeAsync(modules);

            //await db.SaveChangesAsync();
        }

        private static IEnumerable<Course> SeedCoursesAsync()
        {
            var faker = new Faker("sv");
            var courses = new List<Course>();

            for (int i = 0; i < 10; i++)
            {
                var temp = new Course
                {
                    Title = faker.Company.CatchPhrase(),
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5)),
                    Modules = new Module[]
                    {
                        new Module
                        {
                            Title = $"Course ${i} mdolue 1",//faker.Finance.AccountName(),
                            StartDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5)),
                            //CourseId = course.Id,
                            //Course = course
                        },
                        new Module
                        {
                            Title = $"Course ${i} mdolue 2",//faker.Finance.AccountName(),
                            StartDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5)),
                            //CourseId = course.Id,
                            //Course = course
                        }
                    }
                };

                courses.Add(temp);
            }

            return courses;
        }

        private static IEnumerable<Module> SeedModulesAsync(IEnumerable<Course> courses)
        {
            var faker = new Faker("sv");
            var modules = new List<Module>();

            foreach(Course course in courses)
            {
                var temp = new Module
                {
                    Title = faker.Finance.AccountName(),
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5)),
                    CourseId = course.Id,
                    //Course = course
                };

                modules.Add(temp);
            }

            return modules;
        }


        /*private static async Task<ApplicationUser> AddAdminAsync(string adminEmail, string adminPW)
        {
            var found = await userManager.FindByEmailAsync(adminEmail);

            if (found != null) return null!;

            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                TimeOfRegistration = DateTime.Now
            };

            var result = await userManager.CreateAsync(admin, adminPW);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return admin;
        }

        /*
        private static async Task AddToRolesAsync(ApplicationUser admin, string[] roleNames)
        {
            foreach (var role in roleNames)
            {
                if (await userManager.IsInRoleAsync(admin, role)) continue;
                var result = await userManager.AddToRoleAsync(admin, role);
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task<ApplicationUser> AddAdminAsync(string adminEmail, string adminPW)
        {
            var found = await userManager.FindByEmailAsync(adminEmail);

            if (found != null) return null!;

            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                TimeOfRegistration = DateTime.Now
            };

            var result = await userManager.CreateAsync(admin, adminPW);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

            return admin;
        }

        private static async Task AddRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole { Name = roleName };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static IEnumerable<GymClass> GetGymClasses()
        {
            var faker = new Faker("sv");

            var gymClasses = new List<GymClass>();

            for (int i = 0; i < 20; i++)
            {
                var temp = new GymClass
                {
                    Name = faker.Company.CatchPhrase(),
                    Description = faker.Hacker.Verb(),
                    Duration = new TimeSpan(0, 55, 0),
                    StartDate = DateTime.Now.AddDays(faker.Random.Int(-5, 5))
                };

                gymClasses.Add(temp);
            }

            return gymClasses;
        */
    }
}
