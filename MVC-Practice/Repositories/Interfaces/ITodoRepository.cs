﻿using MVC_Practice.Models;

namespace MVC_Practice.Repositories.Interfaces
{
    public interface ITodoRepository
    {
        Task AddCategoryAsync(Categories category);
        Task<List<Categories>> GetCategoriesAsync();
        Task<Tasks> AddTaskAsync(Tasks task);
        Task<List<Tasks>> GetActiveTasksAsync();
        Task<List<Tasks>> GetCompletedTasksAsync();
        Task<Tasks> CompleteTask(int taskId);

    }
}
