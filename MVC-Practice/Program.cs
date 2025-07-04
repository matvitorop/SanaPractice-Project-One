using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Types;
using MVC_Practice.Repositories.Implementations;
using MVC_Practice.Repositories.Interfaces;
using MVC_Practice.Schemas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:49574")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<TodoRepository>();
builder.Services.AddScoped<TodoXMLRepository>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<TaskType>();
builder.Services.AddScoped<CategoryType>();
builder.Services.AddScoped<TodoQuery>();
builder.Services.AddScoped<TodoMutation>();

builder.Services.AddScoped<ISchema, TodoSchema>();

builder.Services.AddGraphQL(b => b
    .AddSystemTextJson()
    .AddGraphTypes()
    .AddAuthorizationRule()
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/ToDo/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowReactApp");

app.UseRouting();
app.UseAuthorization();

app.UseGraphQL("/graphql");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=ToDo}/{action=Index}/");
});

app.UseGraphQLGraphiQL("/ui/graphiql", new GraphiQLOptions
{
    Headers = new Dictionary<string, string>
    {
        { "StorageType", "db" }
    }
});

app.Run();