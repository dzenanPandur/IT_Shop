using ITShop.API.Entities;
using Microsoft.AspNetCore.Identity;

namespace ITShop.API.Helper
{
    public static class CreateRolesHelper
    {
        public static async Task CreateRoles(RoleManager<Role> RoleManager)
        {
            //initializing custom roles 

            string[] roleNames = { "Admin", "Zaposlenik", "Kupac" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    roleResult = await RoleManager.CreateAsync(new Role() { Name = roleName});
                }
            }

        }
    }
}
