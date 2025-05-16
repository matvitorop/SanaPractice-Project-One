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
        private readonly ITodoRepository _todoRepository;

        public ToDoController(ILogger<ToDoController> logger, ITodoRepository todoRepository)
        {
            _logger = logger;
            _todoRepository = todoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _todoRepository.GetCategoriesAsync();
            var activeTasks = await _todoRepository.GetActiveTasksAsync();
            var completedTasks = await _todoRepository.GetCompletedTasksAsync();

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

            try
            {
                await _todoRepository.AddTaskAsync(model);
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
            try
            {
                await _todoRepository.CompleteTask(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when completing a task.");
                TempData["ErrorMessage"] = "The task could not be completed. Please try again later.";
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
