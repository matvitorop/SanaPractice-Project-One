using GraphQL.Reflection;
using GraphQL.Types;
using MVC_Practice.Repositories.Interfaces;

namespace MVC_Practice.Schemas
{
    public class TodoQuery : ObjectGraphType
    {
        public TodoQuery(IHttpContextAccessor accessor, IRepositoryFactory _factory) 
        {
            Field<ListGraphType<TaskType>>("activeTasks")
            .ResolveAsync(async context =>
            {
                var storageType = accessor.HttpContext?.Request.Headers["StorageType"].FirstOrDefault() ?? "db";
                var repo = _factory.CreateTodoRepository(storageType);

                return await repo.GetActiveTasksAsync();
            });

            Field<ListGraphType<TaskType>>("completedTasks")
            .ResolveAsync(async context =>
            {
                var storageType = accessor.HttpContext?.Request.Headers["StorageType"].FirstOrDefault() ?? "db";
                var repo = _factory.CreateTodoRepository(storageType);

                return await repo.GetCompletedTasksAsync();
            });

            Field<ListGraphType<CategoryType>>("categories")
            .ResolveAsync(async context =>
            {
                var storageType = accessor.HttpContext?.Request.Headers["StorageType"].FirstOrDefault() ?? "db";
                var repo = _factory.CreateTodoRepository(storageType);

                return await repo.GetCategoriesAsync();
            });
        }

        
    }
}
