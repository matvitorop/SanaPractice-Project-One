using MVC_Practice.Repositories.Interfaces;

namespace MVC_Practice.Schema
{
    public class Query
    {
        private readonly ITodoRepository _repo;
        public Query(IHttpContextAccessor httpContextAccessor, IRepositoryFactory _factory) 
        {
            var storageType = httpContextAccessor.HttpContext?.Request.Cookies["StorageType"] ?? "db";
            _repo = _factory.CreateTodoRepository(storageType);
        }

        public async Task<IEnumerable<Models.Categories>> GetCategoriesAsync() => await _repo.GetCategoriesAsync();
        public async Task<IEnumerable<Models.Tasks>> GetActiveTasksAsync() => await _repo.GetActiveTasksAsync();
        public async Task<IEnumerable<Models.Tasks>> GetCompletedTasksAsync() => await _repo.GetCompletedTasksAsync();

        [GraphQLDeprecated("Test query")]
        public string Hello => "Hello, world!";
    }
}
