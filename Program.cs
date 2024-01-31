using dotenv.net;
using Blog.Context;
using Microsoft.EntityFrameworkCore;
using AI;
using Blog.Services;
using Blog.Middleware;
using Blog;
using Blog.Services.FileService;
using Blog.Services.Comments;
using LikeService;

using Blog.Blog;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();
// Add services to the container.

// connection to mysql database
builder.Services.AddDbContext<BlogDbContext>((opt) =>
{
    opt.UseMySql(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"), new MySqlServerVersion(new Version(8, 0, 21)));
});


builder.Services.AddHttpClient("OpenAI", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/v1/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    string openAiApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new Exception("OPENAI_API_KEY environment variable is not set.");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
});


builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileValidationService, FileValidationService>();




builder.Services.AddTransient<IOpenAIService, OpenAIService>();
builder.Services.AddTransient<IHashingService, HashingService>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddScoped<AuthMiddleware>();
builder.Services.AddScoped<AdminMiddleware>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ILikeService, LikeService.LikeService>();

builder.Services.AddScoped<IBlogService, BlogService>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new BadRequestExceptionFilter());
});
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
    {
    	policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseCors();
app.Use(async (context, next) =>
{
    var req = context.Request;
    var res = context.Response;
    res.Headers.Add("Access-Control-Allow-Origin", "*");
    res.Headers.Add(
      "Access-Control-Allow-Headers",
      "Origin, X-Requested-With, Content-Type, Accept, Authorization"
    );
    if (req.Method == "OPTIONS")
    {
       res.Headers.Add("Access-Control-Allow-Methods", "PUT, POST, PATCH, DELETE, GET");
       res.StatusCode = 200;
        await res.WriteAsync("");
        return;
    }
    await next();
});

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
if(app.Environment.IsDevelopment()){
    app.MapSwagger();
}



app.MapGet("/", () => "Hello Ladies and Gentlemen!");


app.Run();
