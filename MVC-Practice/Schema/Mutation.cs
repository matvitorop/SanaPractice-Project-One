using MVC_Practice.Models;
using MVC_Practice.Repositories.Interfaces;

namespace MVC_Practice.Schema
{
    public class Mutation
    {
        private readonly ITodoRepository _repo;
        public Mutation(IHttpContextAccessor httpContextAccessor, IRepositoryFactory _factory)
        {
            var storageType = httpContextAccessor.HttpContext?.Request.Cookies["StorageType"] ?? "db";
            _repo = _factory.CreateTodoRepository(storageType);
        }

        public async Task<Tasks> AddTask(Tasks task)
        {
            await _repo.AddTaskAsync(task);
            return task;
        }

        public async Task<bool> CompleteTask(int id)
        {
            await _repo.CompleteTask(id);
            return true;
        }
    }
}
