using dotenv.net;
using Blog.Context;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
// Add services to the container.
builder.Services.AddRazorPages();

// connection to mysql database
builder.Services.AddDbContext<BlogDbContext>((opt) =>
{
    opt.UseMySql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"), new MySqlServerVersion(new Version(8, 0, 21)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapGet("/", () => "Hello Ladies and Gentlemen!");


app.Run();
