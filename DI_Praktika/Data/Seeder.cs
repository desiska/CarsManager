using DI_Praktika.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI_Praktika.Data
{
    public static class Seeder
    {
        public static void Seeding()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            if (db.Users.Count() == 0)
            {
                string administratorRoleID = Guid.NewGuid().ToString();
                db.Roles.Add(new IdentityRole()
                {
                    Id = administratorRoleID,
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });

                db.Roles.Add(new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Client",
                    NormalizedName = "CLIENT",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                });

                db.Statuses.Add(new Status()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "Waiting"
                });

                db.Statuses.Add(new Status()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "Canceled"
                });

                db.Statuses.Add(new Status()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "Active"
                });

                db.Statuses.Add(new Status()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "Used"
                });

                db.Statuses.Add(new Status()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "Overdue"
                });

                string userID = Guid.NewGuid().ToString();

                User user = new User()
                {
                    Id = userID,
                    UserName = "administrator",
                    NormalizedUserName = "ADMINISTRATOR"
                };

                IPasswordHasher<User> hasher = new PasswordHasher<User>();
                string hash = hasher.HashPassword(user, "Admin123!");

                user.PasswordHash = hash;

                db.Users.Add(user);

                db.UserRoles.Add(new IdentityUserRole<string>()
                {
                    RoleId = administratorRoleID,
                    UserId = userID
                });

                db.SaveChangesAsync();
            }
        }
    }
}
