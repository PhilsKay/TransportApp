using Microsoft.AspNetCore.Identity;
using TransportApp.Domain.Enum;

namespace TransportApp.Persistence.Seed;

public class SeedRoles
{
    public static async Task Add(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var roleNames = Enum.GetNames(typeof(Roles));

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}

