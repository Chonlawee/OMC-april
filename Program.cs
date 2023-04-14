using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OMC.Data;
using OMC.Models;
using OMC.Hubs;
using OMC.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AuthorizePage("/Index");
    });
builder.Services.AddDbContext<OMCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OMCContext") ?? throw new InvalidOperationException("Connection string 'OMCContext' not found.")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 4;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.SignIn.RequireConfirmedAccount = false;
        })

    .AddEntityFrameworkStores<OMCContext>();
builder.Services.AddSession();
builder.Services.AddLogging();
builder.Services.AddSignalR();
builder.Services.AddScoped<OrderHub>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<MqttMessageService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
using (var scope = app.Services.CreateScope())
{
   var services = scope.ServiceProvider;
   
   var context = services.GetRequiredService<OMCContext>();
    SeedData.Initialize(services);
}


app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseAuthentication();;

app.UseRouting();



app.UseAuthorization();

app.MapHub<OrderHub>("/orderHub");


app.MapRazorPages();

app.Run();
