using System;
using System.Linq;
using UdemyCarBook.Domain.Entities;
using UdemyCarBook.Persistence.Context;

namespace UdemyCarBook.Persistence.Seed
{
    public static class SeedData
    {
        public static void Initialize(CarBookContext context)
        {
            // =========================
            // 🔥 ROLE SEED
            // =========================
            if (!context.AppRoles.Any())
            {
                context.AppRoles.Add(new AppRole
                {
                    AppRoleName = "Admin"
                });

                context.AppRoles.Add(new AppRole
                {
                    AppRoleName = "User"
                });

                context.SaveChanges();
            }

            // =========================
            // 🔥 ADMIN USER SEED
            // =========================
            if (!context.AppUsers.Any())
            {
                var adminRole = context.AppRoles.FirstOrDefault(x => x.AppRoleName == "Admin");

                context.AppUsers.Add(new AppUser
                {
                    Username = "admin",
                    Password = "1234", // ⚠ şimdilik plain (sonra hash önerilir)
                    Name = "System",
                    Surname = "Admin",
                    Email = "admin@carbook.com",
                    AppRoleId = adminRole.AppRoleId
                });

                context.SaveChanges();
            }
        }
    }
}