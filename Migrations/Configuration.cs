namespace OnBoardingIdentity.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using OnBoardingIdentity.Infrastructure;
    using OnBoardingIdentity.Infrastructure.Data;

    internal sealed class Configuration : DbMigrationsConfiguration<OnBoardingIdentity.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OnBoardingIdentity.Infrastructure.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            //  This method will be called after migrating to the latest version


            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "ExampleUser",
                Email = "taiseer.joudeh@gmail.com",
                EmailConfirmed = true,
                FirstName = "Taiseer",
                LastName = "Joudeh",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

            manager.Create(user, "passwordexemplo");


            //create the 2 roles we want our system to have
            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Gestor Projeto" });
                roleManager.Create(new IdentityRole { Name = "Programador" });
            }

            var adminUser = manager.FindByName("ExampleUser");

            manager.AddToRoles(adminUser.Id, new string[] { "Gestor Projeto"});
        }
    }
}
