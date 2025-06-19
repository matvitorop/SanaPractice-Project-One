using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC_Practice.Models;
using MVC_Practice.Models.Todo_Models;
using MVC_Practice.Repositories.Interfaces;

namespace MVC_Practice.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly IRepositoryFactory _factory;

        public ToDoController(ILogger<ToDoController> logger, IRepositoryFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }
        private ITodoRepository GetRepo()
        {
            return _factory.CreateTodoRepository(Request.Cookies["StorageType"] ?? "db");
        }
        public async Task<IActionResult> Index()
        {
            var repo = GetRepo();

            var categories = await repo.GetCategoriesAsync();
            var activeTasks = await repo.GetActiveTasksAsync();
            var completedTasks = await repo.GetCompletedTasksAsync();

            var viewModel = new TodoListViewModel
            {
                Categories = categories,
                ActiveTasks = activeTasks,
                CompletedTasks = completedTasks,
                NewTask = new Tasks()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(Models.Tasks model)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("NewTask.Title", "Назва обов’язкова.");
            }

            var repo = GetRepo();
            try
            {
                await repo.AddTaskAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when adding a task.");
                ModelState.AddModelError(string.Empty, "Error adding a task.An error occurred while saving. Please try again later.");

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> CompleteTask(int id)
        {
            var repo = GetRepo();

            try
            {
                await repo.CompleteTask(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when completing a task.");
                TempData["ErrorMessage"] = "The task could not be completed. Please try again later.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> SetStorage(string storageType)
        {
            if(string.IsNullOrWhiteSpace(storageType) || 
                !(storageType.Equals("db", StringComparison.OrdinalIgnoreCase) ||
                storageType.Equals("xml", StringComparison.OrdinalIgnoreCase)))
            {
                TempData["ErrorMessage"] = "Invalid storage type selected.";
                return RedirectToAction("Index");
            }
            else
            {
                Response.Cookies.Append("StorageType", storageType, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    //IsEssential = true,
                });
                return RedirectToAction("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
