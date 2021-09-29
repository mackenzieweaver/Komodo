using Komodo.Data;
using Komodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Utilities
{
    public static class DataHelper
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            // the default connection string will come from appsettings.json like usual
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // It will be automatically overwritten if we are running on Heroku
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        public static string BuildConnectionString(string databaseUrl)
        {
            // Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            //Provides a simple way to create and manage the contents of connection strings used by the NpgsqlConnection class.
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }

        public static async Task ManageData(IHost host)
        {
            try
            {
                //This technique is used to obtain references to services
                // normally I would just inject these services but you cant use a constructor in a static class
                using var svcScope = host.Services.CreateScope();
                var svcProvider = svcScope.ServiceProvider;

                //The service will run your migrations
                var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
                await dbContextSvc.Database.MigrateAsync();

                // Seed Data
                var services = svcScope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<BTUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                if (!context.Roles.Any())
                {
                    await ContextSeed.SeedRolesAsync(roleManager);
                }

                if (context.Users.FirstOrDefault(u => u.Email == "m@w.com") == null)
                {
                    await ContextSeed.SeedDefaultUsersAsync(userManager);
                }

                await ContextSeed.SeedTicketListsAsync(context);

                // projects, users, tickets
                if (!context.Projects.Any())
                {
                    await ContextSeed.SeedProjectAsync(context);
                }

                if (!context.ProjectUsers.Any())
                {
                    await ContextSeed.SeedProjectUsersAsync(context, userManager);
                }

                int numTickets = 10;
                if (context.Tickets.ToList().Count < numTickets)
                {
                    await ContextSeed.SeedTicketsAsync(context, userManager, numTickets);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while running Manage Data => {ex}");
            }
        }
    }
}
