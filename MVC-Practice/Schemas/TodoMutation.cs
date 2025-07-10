using GraphQL;
using GraphQL.Reflection;
using GraphQL.Types;
using MVC_Practice.Models;
using MVC_Practice.Repositories.Interfaces;

namespace MVC_Practice.Schemas
{
    public class TodoMutation : ObjectGraphType
    {
        public TodoMutation(IHttpContextAccessor httpContextAccessor, IRepositoryFactory _factory)
        {
            Field<TaskType>("addTask")
                .Argument<TaskInputType>("task", "The task to add")
                .ResolveAsync(async context =>
                {
                    var storageType = httpContextAccessor.HttpContext?.Request.Headers["StorageType"].FirstOrDefault() ?? "db";
                    var repo = _factory.CreateTodoRepository(storageType);

                    var task = context.GetArgument<Models.Tasks>("task");
                    
                    try
                    {
                        var createdTask = await repo.AddTaskAsync(task);
                        return createdTask;

                    }
                    catch (Exception ex)
                    {
                        context.Errors.Add(new ExecutionError("Failed to add task", ex));
                        return null;
                    }
                });

            Field<BooleanGraphType>("completeTask")
                .Argument<IntGraphType>("id", "task id")
                .ResolveAsync(async context =>
                {
                    var storageType = httpContextAccessor.HttpContext?.Request.Headers["StorageType"].FirstOrDefault() ?? "db";
                    var repo = _factory.CreateTodoRepository(storageType);
                    var id = context.GetArgument<int>("id");
                    
                    try
                    {
                        await repo.CompleteTask(id);

                    }catch (Exception ex)
                    {
                        return false;
                    }
                    return true;
                });

            Field<BooleanGraphType>("addCategory")
                .Argument<CategoryInputType>("category", "Category name")
                .ResolveAsync(async context =>
                {
                    var storageType = httpContextAccessor.HttpContext?.Request.Headers["StorageType"].FirstOrDefault() ?? "db";
                    var repo = _factory.CreateTodoRepository(storageType);
                    var name = context.GetArgument<Categories>("category");
                                        
                    try
                    {
                        await repo.AddCategoryAsync(name);

                    }catch (Exception ex)
                    {
                        return false;
                    }
                    return true;
                });
        }
    }
}
