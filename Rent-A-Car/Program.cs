using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.DbContext;
using Rent_A_Car.Models;

namespace Rent_A_Car
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
			});

            builder.Services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = true;
					options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredLength = 6;
					options.Password.RequireDigit = true;

					options.User.RequireUniqueEmail = true;

					options.SignIn.RequireConfirmedAccount = false;
					options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
				}
                )
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                

                string email = "admin@gmail.com";
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User();
                    user.UserName = email;
                    user.Email = email;
                    user.Password = "Pass@1";
                    user.EGN = "1234567890";
                    user.PhoneNumber = "1234567890";
                    user.FirstName = "Admin";
                    user.LastName = "Account";
                    user.EmailConfirmed = true;

                    await userManager.CreateAsync(user, "Pass@1");

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }


            app.Run();
        }
    }
}
