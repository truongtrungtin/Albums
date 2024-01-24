using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity
{
	public class ApplicationIdentityDbContextSeed
	{
		public static async Task SeedAsync(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope()) {
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

				await SeedUserAsync(userManager, dbContext);
			};
		}

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, ApplicationIdentityDbContext dbContext)
        {
			if (!userManager.Users.Any())
			{
				var user = new ApplicationUser
				{
					UserName = "trungtin",
					Email = "trungtin.230599@gmail.com",
					DisplayName = "Trung Tin",
					//Add other properties as needed
					Address = new Address
					{
						Id = Guid.NewGuid().ToString(),
						Fname = "Trung",
						Lname = "Tin",
						Street = "123 Main St",
						City = "Example City",
						State = "Example State",
						ZipCode = "123456",
					}
				};

				var result = await userManager.CreateAsync(user, "Trungtin@30599");
				if (result.Succeeded)
				{
					//Optionally, you can do additional or customization here
					//For example, add user roles, claims, etc.
				}
				else
				{
                    throw new Exception($"User creation failed: {string.Join(",", result.Errors)}");
                }
            }
        }

	}
}

