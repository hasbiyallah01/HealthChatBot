using HealthChatBox.Core.Application.Interfaces.Repositories;
using HealthChatBox.Core.Application.Interfaces.Services;
using HealthChatBox.Core.Application.Services;
using HealthChatBox.Infrastructure.Context;
using HealthChatBox.Infrastructure.Repositories;
using HealthChatBox.Services;
using KonsumeTestRun.Core.Application.Interfaces.Repositories;
using KonsumeTestRun.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.Models.Entities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure CORS
builder.Services.AddCors(cors =>
{
    cors.AddPolicy("konsume", pol =>
    {
        pol.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Configure Email
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("SMTPConfig"));

// Configure Database Context
builder.Services.AddDbContext<KonsumeContext>(opt => opt.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IProfileRepository, ProfileRepository>();
builder.Services.AddTransient<IRoleRepository, RoleRepository>();
builder.Services.AddTransient<IVerificationCodeRepository, VerificationCodeRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();

// Register Services
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IVerificationCodeService, VerificationCodeService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable CORS
app.UseCors("konsume");

// Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
