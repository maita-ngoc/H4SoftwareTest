using H4SoftwareTest.Code;
using H4SoftwareTest.Components;
using H4SoftwareTest.Components.Account;
using H4SoftwareTest.Data;
using H4SoftwareTest.Data.Manager;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddSingleton<HashingHandler>();
builder.Services.AddSingleton<EncryptionHandler>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
if (OperatingSystem.IsWindows())
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
    var todoConnectionString = builder.Configuration.GetConnectionString("TodoConnection") ?? throw new InvalidOperationException("Connection string 'TodoConnection' not found.");
    builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(todoConnectionString));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("MockConnection") ?? throw new InvalidOperationException("Connection string 'MockConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
    var todoConnectionString = builder.Configuration.GetConnectionString("TodoMockConnection") ?? throw new InvalidOperationException("Connection string 'TodoMockConnection' not found.");
    builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlite(todoConnectionString));
}
//var connectionString = builder.Configuration.GetConnectionString("MockConnection") ?? throw new InvalidOperationException("Connection string 'MockConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//options.UseSqlite(connectionString));
//var todoConnectionString = builder.Configuration.GetConnectionString("TodoMockConnection") ?? throw new InvalidOperationException("Connection string 'TodoMockConnection' not found.");
//builder.Services.AddDbContext<TodoDbContext>(options =>
//options.UseSqlite(todoConnectionString));
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Admin or not admin
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

//Add Policy Authorization
builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("AuthenticateUser",policy =>
    {
        policy.RequireAuthenticatedUser();
        
    });
    option.AddPolicy("RequireAdmin", policy =>
    {
        policy.RequireRole("Admin");

    });
});

//Add Singleton
builder.Services.AddSingleton<RoleHandler>();
builder.Services.AddScoped<ICprManager, CprManager>();
builder.Services.AddScoped<ITodoManager, TodoManager>();
builder.Services.AddHttpClient();

//Using certificate over cross platform
string userFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
userFolder = Path.Combine(userFolder, ".aspnet");
userFolder = Path.Combine(userFolder, "https");
userFolder = Path.Combine(userFolder, "TodoList2305.pfx");

string kestrelPassword = builder.Configuration.GetValue<string>("KestrelPassword");
builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate:Path").Value = userFolder;
builder.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate:Password").Value = kestrelPassword;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
