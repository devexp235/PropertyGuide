using Microsoft.AspNet.Identity.EntityFramework;
using PropertyGuide.Helpers;

namespace PropertyGuide.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PropertyGuide.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(PropertyGuide.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //Add default user roles
            //context.Roles.AddOrUpdate(new IdentityRole(Constants.UserType.Buyer));
            //context.Roles.AddOrUpdate(new IdentityRole(Constants.UserType.Seller));
        }
    }
}
