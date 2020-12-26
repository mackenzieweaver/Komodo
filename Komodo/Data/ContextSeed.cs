using Komodo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        NewUser,
        Demo
    }

    public enum TicketTypes
    {
        UI,
        Calculation,
        Logic,
        Security
    }

    public enum TicketPriorities
    {
        Low,
        Moderate,
        Major,
        Critical
    }

    public enum TicketStatuses
    {
        Opened,
        Testing,
        Development,
        QA,
        FinalPass,
        Closed
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
            await roleManager.CreateAsync(new IdentityRole(Roles.Demo.ToString()));
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
                if (user == null)
                {
                    await userManager.CreateAsync(defaultAdmin, "Mweaver1!");
                    await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin.ToString());
                }
            }
            catch (Exception ex)
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
                UserName = "a@r.com",
                Email = "a@r.com",
                FirstName = "Antonio",
                LastName = "Raynor",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultPM.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultPM, "Araynor1!");
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
                UserName = "j@t.com",
                Email = "j@t.com",
                FirstName = "Jason",
                LastName = "Twitchell",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultDev.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultDev, "Jtwitchell1!");
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
                UserName = "d@r.com",
                Email = "d@r.com",
                FirstName = "Drew",
                LastName = "Russel",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultSub.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultSub, "Drussell1!");
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
                UserName = "a@b.com",
                Email = "a@b.com",
                FirstName = "Adam",
                LastName = "Brooks",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Abrooks1!");
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
            defaultNew = new BTUser
            {
                UserName = "a@d.com",
                Email = "a@d.com",
                FirstName = "Anthony",
                LastName = "Duval",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Aduval1!");
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
            defaultNew = new BTUser
            {
                UserName = "j@h.com",
                Email = "j@h.com",
                FirstName = "Jackson",
                LastName = "Holliday",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jholliday1!");
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
            defaultNew = new BTUser
            {
                UserName = "j@s.com",
                Email = "j@s.com",
                FirstName = "Josh",
                LastName = "Scott",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jscott1!");
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
            defaultNew = new BTUser
            {
                UserName = "k@b.com",
                Email = "k@b.com",
                FirstName = "Kenan",
                LastName = "Bjelosevic",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Kbjelosevic1!");
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
            defaultNew = new BTUser
            {
                UserName = "m@n.com",
                Email = "m@n.com",
                FirstName = "MacColl",
                LastName = "Nicolson",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Mnicolson1!");
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
            defaultNew = new BTUser
            {
                UserName = "a@e.com",
                Email = "a@e.com",
                FirstName = "Adrian",
                LastName = "Edelen",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Aedelen1!");
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
            defaultNew = new BTUser
            {
                UserName = "b@c.com",
                Email = "b@c.com",
                FirstName = "Beau",
                LastName = "Cunningham",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Bcunningham1!");
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
            defaultNew = new BTUser
            {
                UserName = "j@h.com",
                Email = "j@h.com",
                FirstName = "Jessica",
                LastName = "Hedenskog",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jhedenskog1!");
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
            defaultNew = new BTUser
            {
                UserName = "k@c.com",
                Email = "k@c.com",
                FirstName = "Kit",
                LastName = "Chau",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Kchau1!");
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
            defaultNew = new BTUser
            {
                UserName = "n@w.com",
                Email = "n@w.com",
                FirstName = "Nick",
                LastName = "Webb",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Nwebb1!");
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
            defaultNew = new BTUser
            {
                UserName = "t@b.com",
                Email = "t@b.com",
                FirstName = "Tony",
                LastName = "Beavers",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Tbeavers1!");
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
            defaultNew = new BTUser
            {
                UserName = "andy@rivera.com",
                Email = "andy@rivera.com",
                FirstName = "Andres",
                LastName = "Rivera",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Arivera1!");
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
            defaultNew = new BTUser
            {
                UserName = "c@t.com",
                Email = "c@t.com",
                FirstName = "Charles",
                LastName = "Tincher",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Ctincher1!");
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
            defaultNew = new BTUser
            {
                UserName = "e@j.com",
                Email = "e@j.com",
                FirstName = "Ethan",
                LastName = "Jones",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Ejones1!");
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
            defaultNew = new BTUser
            {
                UserName = "eli@jones.com",
                Email = "eli@jones.com",
                FirstName = "Eli",
                LastName = "Jones",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Ejones1!");
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
            defaultNew = new BTUser
            {
                UserName = "j@g.com",
                Email = "j@g.com",
                FirstName = "Jonathan",
                LastName = "Green",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jgreen1!");
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
            defaultNew = new BTUser
            {
                UserName = "joseph@green.com",
                Email = "joseph@green.com",
                FirstName = "Joseph",
                LastName = "Green",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jgreen1!");
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
            defaultNew = new BTUser
            {
                UserName = "j@s.com",
                Email = "j@s.com",
                FirstName = "Julio",
                LastName = "Segarra",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Jsegarra1!");
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
            defaultNew = new BTUser
            {
                UserName = "l@a.com",
                Email = "l@a.com",
                FirstName = "Larry",
                LastName = "Ashton",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Lashton1!");
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
            defaultNew = new BTUser
            {
                UserName = "o@o.com",
                Email = "o@o.com",
                FirstName = "Orlando",
                LastName = "Olmo",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Oolmo1!");
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
            defaultNew = new BTUser
            {
                UserName = "a@a.com",
                Email = "a@a.com",
                FirstName = "Andrew",
                LastName = "Albanese",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Aalbanese1!");
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
            defaultNew = new BTUser
            {
                UserName = "d@j.com",
                Email = "d@j.com",
                FirstName = "Denis",
                LastName = "Jojot",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Djojot1!");
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
            defaultNew = new BTUser
            {
                UserName = "f@s.com",
                Email = "f@s.com",
                FirstName = "Fred",
                LastName = "Smith",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Fsmith1!");
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
            defaultNew = new BTUser
            {
                UserName = "d@c.com",
                Email = "d@c.com",
                FirstName = "Danny",
                LastName = "Carroll",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Dcarroll1!");
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
            defaultNew = new BTUser
            {
                UserName = "m@j.com",
                Email = "m@j.com",
                FirstName = "Mark",
                LastName = "Janicki",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Mjanicki1!");
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
            defaultNew = new BTUser
            {
                UserName = "s@j.com",
                Email = "s@j.com",
                FirstName = "Shyann",
                LastName = "Jobe",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(defaultNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultNew, "Sjobe1!");
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

            var demoPassword = "Abc&123!";
            #region DEMO Seed Admin 
            // Admin
            var demoAdmin = new BTUser
            {
                UserName = "garretreynolds@mailinator.com",
                Email = "garretreynolds@mailinator.com",
                FirstName = "Garret",
                LastName = "Reynolds",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoAdmin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoAdmin, demoPassword);
                    await userManager.AddToRoleAsync(demoAdmin, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(demoAdmin, Roles.Demo.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Default Admin.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
            #endregion

            #region DEMO Seed PM
            // Project Manager
            var demoPM = new BTUser
            {
                UserName = "alexheim@mailinator.com",
                Email = "alexheim@mailinator.com",
                FirstName = "Alex",
                LastName = "Heim",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoPM.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoPM, demoPassword);
                    await userManager.AddToRoleAsync(demoPM, Roles.ProjectManager.ToString());
                    await userManager.AddToRoleAsync(demoPM, Roles.Demo.ToString());
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

            #region DEMO Seed Dev
            // Developer
            var demoDev = new BTUser
            {
                UserName = "dennisenerson@mailinator.com",
                Email = "dennisenerson@mailinator.com",
                FirstName = "Dennis",
                LastName = "Enerson",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoDev.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoDev, demoPassword);
                    await userManager.AddToRoleAsync(demoDev, Roles.Developer.ToString());
                    await userManager.AddToRoleAsync(demoDev, Roles.Demo.ToString());
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

            #region DEMO Seed Sub
            // Submitter
            var demoSub = new BTUser
            {
                UserName = "larryedwards@mailinator.com",
                Email = "larryedwards@mailinator.com",
                FirstName = "Larry",
                LastName = "Edwards",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoSub.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoSub, demoPassword);
                    await userManager.AddToRoleAsync(demoSub, Roles.Submitter.ToString());
                    await userManager.AddToRoleAsync(demoSub, Roles.Demo.ToString());
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

            #region DEMO Seed New
            // New User
            var demoNew = new BTUser
            {
                UserName = "steviechurchhill@mailinator.com",
                Email = "steviechurchhill@mailinator.com",
                FirstName = "Stevie",
                LastName = "Churchhill",
                EmailConfirmed = true
            };
            try
            {
                var user = await userManager.FindByEmailAsync(demoNew.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(demoNew, demoPassword);
                    await userManager.AddToRoleAsync(demoNew, Roles.NewUser.ToString());
                    await userManager.AddToRoleAsync(demoNew, Roles.Demo.ToString());
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
            #endregion new user
        }

        public static async Task SeedTicketListsAsync(ApplicationDbContext context)
        {
            try
            {
                // Types
                var types = context.TicketTypes.Any();
                if (types == false)
                {
                    var type = new TicketType{ Name = TicketTypes.UI.ToString() };
                    context.TicketTypes.Add(type);
                    type = new TicketType { Name = TicketTypes.Calculation.ToString() };
                    context.TicketTypes.Add(type);
                    type = new TicketType { Name = TicketTypes.Logic.ToString() };
                    context.TicketTypes.Add(type);
                    type = new TicketType { Name = TicketTypes.Security.ToString() };
                    context.TicketTypes.Add(type);
                    await context.SaveChangesAsync();
                }

                // Priority
                var priorites = context.TicketPriorities.Any();
                if (priorites == false)
                {
                    var priority = new TicketPriority { Name = TicketPriorities.Low.ToString() };
                    context.TicketPriorities.Add(priority);
                    priority = new TicketPriority { Name = TicketPriorities.Moderate.ToString() };
                    context.TicketPriorities.Add(priority);
                    priority = new TicketPriority { Name = TicketPriorities.Major.ToString() };
                    context.TicketPriorities.Add(priority);
                    priority = new TicketPriority { Name = TicketPriorities.Critical.ToString() };
                    context.TicketPriorities.Add(priority);
                    await context.SaveChangesAsync();
                }

                // Status
                var statuses = context.TicketStatuses.Any();
                if (statuses == false)
                {
                    var status = new TicketStatus { Name = TicketStatuses.Opened.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.Testing.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.Development.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.QA.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.FinalPass.ToString() };
                    context.TicketStatuses.Add(status);
                    status = new TicketStatus { Name = TicketStatuses.Closed.ToString() };
                    context.TicketStatuses.Add(status);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("*********** ERROR **********");
                Debug.WriteLine("Error Seeding Ticket Priorities.");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("****************************");
                throw;
            }
        }

        public static async Task SeedProjectAsync(ApplicationDbContext context)
        {
            List<string> projectNames = new List<string> { "Blog", "Bug Tracker", "Financial Portal" };
            foreach(var projectName in projectNames)
            {
                Project project = new Project();
                project.Name = projectName;
                try
                {                    
                    if (context.Projects.FirstOrDefault(p => p.Name == projectName) == null)
                    {
                        await context.Projects.AddAsync(project);
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("*********** ERROR **********");
                    Debug.WriteLine($"Error Seeding Project: {projectName}");
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine("****************************");
                    throw;
                }
            }            
        }

        public static async Task SeedProjectUsersAsync(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            string adminId = (await userManager.FindByEmailAsync("garretreynolds@mailinator.com")).Id;
            string pmId = (await userManager.FindByEmailAsync("alexheim@mailinator.com")).Id;
            string devId = (await userManager.FindByEmailAsync("dennisenerson@mailinator.com")).Id;
            string subId = (await userManager.FindByEmailAsync("larryedwards@mailinator.com")).Id;

            int project1Id = context.Projects.FirstOrDefault(p => p.Name == "Blog").Id;
            int project2Id = context.Projects.FirstOrDefault(p => p.Name == "Bug Tracker").Id;
            int project3Id = context.Projects.FirstOrDefault(p => p.Name == "Financial Portal").Id;

            List<string> userIds = new List<string> { adminId, pmId, devId, subId };
            List<int> projectIds = new List<int> { project1Id, project2Id, project3Id };

            ProjectUser projectUser = new ProjectUser();
            foreach(var userId in userIds)
            {
                foreach(var projectId in projectIds)
                {
                    projectUser.UserId = userId;
                    projectUser.ProjectId = projectId;
                    try
                    {
                        var record = context.ProjectUsers.FirstOrDefault(p => p.UserId == userId && p.ProjectId == projectId);
                        if (record == null)
                        {
                            await context.ProjectUsers.AddAsync(projectUser);
                            await context.SaveChangesAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("*********** ERROR **********");
                        Debug.WriteLine($"Error Seeding {userId} for {projectId}");
                        Debug.WriteLine(ex.Message);
                        Debug.WriteLine("****************************");
                        throw;
                    }
                }
            }
        }

        public static async Task SeedTicketsAsync(ApplicationDbContext context, UserManager<BTUser> userManager)
        {
            string devId = (await userManager.FindByEmailAsync("dennisenerson@mailinator.com")).Id;
            string subId = (await userManager.FindByEmailAsync("larryedwards@mailinator.com")).Id;
            int project1Id = context.Projects.FirstOrDefault(p => p.Name == "Blog").Id;
            int project2Id = context.Projects.FirstOrDefault(p => p.Name == "Bug Tracker").Id;
            int project3Id = context.Projects.FirstOrDefault(p => p.Name == "Financial Portal").Id;
            List<int> projects = new List<int> { project1Id, project2Id, project3Id };
            int numTickets = 10;
            var statuses = context.TicketStatuses.ToList();
            var types = context.TicketTypes.ToList();
            var priorities = context.TicketPriorities.ToList();
            for (var i = 0; i < numTickets; i++)
            {
                int random = new Random().Next(numTickets);
                bool hasDeveloper = random > (numTickets / 2) ? true : false;
                Ticket ticket = new Ticket
                {
                    Title = $"Random: {random}",
                    Description = $"{random} uniquely identifies this ticket",
                    Created = DateTimeOffset.Now.AddDays(-(random * random)),
                    Updated = DateTimeOffset.Now.AddHours(-(random * random)),
                    ProjectId = projects[new Random().Next(projects.Count)],
                    TicketPriorityId = priorities[new Random().Next(priorities.Count)].Id,
                    TicketStatusId = statuses[new Random().Next(statuses.Count)].Id,
                    TicketTypeId = types[new Random().Next(types.Count)].Id,
                    DeveloperUserId = hasDeveloper ? devId : null,
                    OwnerUserId = subId
                };
                try
                {
                    var myticket = context.Tickets.FirstOrDefault(t => t.Title == ticket.Title);
                    if (myticket == null)
                    {
                        await context.Tickets.AddAsync(ticket);
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("*********** ERROR **********");
                    Debug.WriteLine($"Error Seeding ticket {random}");
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine("****************************");
                    throw;
                }
            }
        }
    }
}
