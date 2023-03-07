using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zemoga_Test.Domain.Entities;
using Zemoga_Test.Infrastructure.Identity;

namespace Zemoga_Test.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var roles = new IdentityRole[]
        {
            new IdentityRole("administrator"),
            new IdentityRole("writer"),
            new IdentityRole("editor"),
            new IdentityRole("public"),
        };
        foreach (var role in roles)
        {
            if (_roleManager.Roles.All(r => r.Name != role.Name))
            {
                await _roleManager.CreateAsync(role);
            }
        }

        // Default users
        var users = new ApplicationUser[]{
            new ApplicationUser { UserName = "administrator", Email = "administrator@localhost.com" },
             new ApplicationUser { UserName = "writer", Email = "writer@localhost.com" },
             new ApplicationUser { UserName = "editor", Email = "editor@localhost.com" },
             new ApplicationUser { UserName = "public", Email = "publicUser@localhost.com" }
        };

        foreach (var user in users)
        {
            if (_userManager.Users.All(u => u.UserName != user.UserName))
            {
                await _userManager.CreateAsync(user, "Password1!");
                if (!string.IsNullOrWhiteSpace(user.UserName))
                {
                    var rol = roles.Where(x => x.Name == user.UserName).Select(x => x.Name);
                    if (rol.Any()) { 
                        await _userManager.AddToRolesAsync(user, rol!);
                    }
                }
            }
        }


        // Default data
        // Seed, if necessary
        if (!_context.Posts.Any())
        {
            var post1 = new Post { Title = "Exploring the Benefits of Meditation", Content = "Meditation is an ancient practice that has been used for centuries to help reduce stress, improve mental clarity, and create a better sense of overall wellbeing. In this article, I will discuss the physical and mental benefits of meditation and how it can be used as part of a healthy lifestyle.", Status = Domain.Enums.PostStatus.Approved };
            var post2 = new Post { Title = "The Importance of Exercise", Content = "Exercise is an important part of a healthy lifestyle. Regular physical activity can help reduce the risk of chronic diseases, improve mental health, and increase energy levels. In this article, I will discuss the importance of incorporating exercise into your daily routine and some tips for getting started.", Status = Domain.Enums.PostStatus.Approved };
            var post3 = new Post { Title = "The Benefits of Healthy Eating", Content = "Healthy eating is an important part of leading a healthy lifestyle. Eating a balanced diet that includes plenty of fruits and vegetables can help support a healthy weight, reduce the risk of chronic diseases, and improve overall wellbeing. In this article, I will discuss the benefits of healthy eating and some tips for making healthier food choices.", Status = Domain.Enums.PostStatus.Approved };
            var post4 = new Post { Title = "The Benefits of a Good Night's Sleep", Content = "Getting a good night's sleep is essential for physical and mental health. Sleep allows the body to repair and recharge, and a lack of sleep can have serious consequences on your health. In this article, I will discuss the importance of getting enough restful sleep and some tips for improving your sleep quality.", Status = Domain.Enums.PostStatus.Approved };
            var post5 = new Post { Title = "The Benefits of Stress Management", Content = "Stress is a normal part of life, but too much stress can have a negative impact on your physical and mental health. Learning how to manage stress can help you live a happier and healthier life. In this article, I will discuss the benefits of stress management and some tips for reducing stress in your life.", Status = Domain.Enums.PostStatus.Approved };

            post1.Comments.Add(new Comment { Content = "this is a great post!", Author = "John Doe" });
            post1.Comments.Add(new Comment { Content = "I agree, this is a great post!", Author = "Jane Doe" });
            post1.Comments.Add(new Comment { Content = "I love this post!", Author = "John Smith" });

            post2.Comments.Add(new Comment { Content = "this is a great post!", Author = "John Doe" });
            post2.Comments.Add(new Comment { Content = "I agree, this is a great post!", Author = "Jane Doe" });
            post2.Comments.Add(new Comment { Content = "I love this post!", Author = "John Smith" });

            post3.Comments.Add(new Comment { Content = "this is a great post!", Author = "John Doe" });
            post3.Comments.Add(new Comment { Content = "I agree, this is a great post!", Author = "Jane Doe" });
            post3.Comments.Add(new Comment { Content = "I love this post!", Author = "John Smith" });

            _context.Posts.AddRange(post1, post2, post3, post4, post5);

            await _context.SaveChangesAsync();
        }
    }
}