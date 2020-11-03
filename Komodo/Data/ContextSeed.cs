using Komodo.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Komodo.Data
{
    public enum Roles
    {
        Admin,
        ProjectManager,
        Developer,
        Submitter,
        NewUser
    }

    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.ProjectManager.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Submitter.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.NewUser.ToString()));
        }

        public static async Task SeedDefaultUsersAsync(UserManager<BTUser> userManager)
        {
            #region Admin
            // Admin
            var defaultAdmin = new BTUser
            {
                UserName = "m@w.com",
                Email = "m@w.com",
                FirstName = "Mackenzie",
                LastName = "Weaver",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultAdmin.Email);
                if(user == null)
                {
                    await userManager.CreateAsync(defaultAdmin, "Mweaver1!");
                    await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default Admin.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            #endregion

            #region PM
            // Project Manager
            var defaultPM = new BTUser
            {
                UserName = "j@w.com",
                Email = "j@w.com",
                FirstName = "Joseph",
                LastName = "Weaver",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultPM.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultPM, "Jweaver1!");
                    await userManager.AddToRoleAsync(defaultPM, Roles.ProjectManager.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default Project Manager.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            #endregion

            #region Dev
            // Developer
            var defaultDev = new BTUser
            {
                UserName = "d@w.com",
                Email = "d@w.com",
                FirstName = "Dorinda",
                LastName = "Weaver",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultDev.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultDev, "Dweaver1!");
                    await userManager.AddToRoleAsync(defaultDev, Roles.Developer.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default Developer.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            #endregion

            #region Sub
            // Submitter
            var defaultSub = new BTUser
            {
                UserName = "b@c.com",
                Email = "b@c.com",
                FirstName = "Baruch",
                LastName = "Carman",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultSub.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultSub, "Bcarman1!");
                    await userManager.AddToRoleAsync(defaultSub, Roles.Submitter.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default Submitter.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            #endregion

            #region New
            // New User
            var defaultNew = new BTUser
            {
                UserName = "a@c.com",
                Email = "a@c.com",
                FirstName = "Alexis",
                LastName = "Carman",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Acarman1!");
                    await userManager.AddToRoleAsync(defaultNew, Roles.NewUser.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default New User.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            #endregion
        }
    }
}
