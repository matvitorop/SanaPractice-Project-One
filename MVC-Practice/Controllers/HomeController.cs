using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC_Practice.Models;
using MVC_Practice.Models.Todo_Models;
using MVC_Practice.Repositories.Interfaces;

namespace MVC_Practice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITodoRepository _todoRepository;

        public HomeController(ILogger<HomeController> logger, ITodoRepository todoRepository)
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
        public async Task<IActionResult> AddTask(TodoListViewModel model)
        {
            var task = model.NewTask;

            if (string.IsNullOrWhiteSpace(task.Title))
            {
                ModelState.AddModelError("NewTask.Title", "Назва обов’язкова.");
            }

            await _todoRepository.AddTaskAsync(task);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<ActionResult> CompleteTask(int id)
        {
            await _todoRepository.CompleteTask(id);
            return RedirectToAction("Index");
        }



        // DELETE
        [HttpPost]
        public IActionResult Greeting(NameModel model)
        {
            var name = string.IsNullOrWhiteSpace(model.Name) ? "Unknown" : model.Name;
            ViewData["Username"] = name;
            return View();
        }

        // REVIEW
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
