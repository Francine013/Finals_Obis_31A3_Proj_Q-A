using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QABoard.Infrastructure.Data;
using QABoard.Infrastructure.Entities;
using QABoard.Services.Implementations;
using QABoard.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure DbContext with In-Memory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("QABoardDb"));

// Configure Identity with custom password requirements
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredUniqueChars = 1;

    // Custom validation for exactly 2 uppercase, 3 numbers, 3 symbols
    // This will be handled in registration validation
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Register services
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();
builder.Services.AddScoped<IVoteService, VoteService>();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var context = services.GetRequiredService<AppDbContext>();

    await DbSeeder.SeedData(userManager, roleManager, context);
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();