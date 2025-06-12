using MVC_Practice.Repositories.Implementations;
using MVC_Practice.Repositories.Interfaces;
using MVC_Practice.Schema;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<TodoRepository>();
builder.Services.AddScoped<TodoXMLRepository>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

// Get to know
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<Query>();
//builder.Services.AddScoped<Mutation>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();
    //.AddMutationType<Mutation>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ToDo/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ToDo}/{action=Index}/");

app.MapGraphQL();

app.Run();
