using MVC_Practice.Models;
using MVC_Practice.Repositories.Interfaces;

namespace MVC_Practice.Repositories.Implementations
{
    public class TodoXMLRepository : ITodoRepository
    {
        public Task AddCategoryAsync(Categories category)
        {
            throw new NotImplementedException();
        }

        public Task AddTaskAsync(Tasks task)
        {
            throw new NotImplementedException();
        }

        public Task CompleteTask(int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tasks>> GetActiveTasksAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Categories>> GetCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Tasks>> GetCompletedTasksAsync()
        {
            throw new NotImplementedException();
        }
    }
}
