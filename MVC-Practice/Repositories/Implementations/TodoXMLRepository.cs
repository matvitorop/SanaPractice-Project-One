using MVC_Practice.Models;
using MVC_Practice.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

public class TodoXMLRepository : ITodoRepository
{
    private readonly string _xmlFilePath;
    private XDocument _xmlDocument;
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public TodoXMLRepository(IConfiguration config)
    {
        var relativePath = config["XmlSettings:FilePath"];
        _xmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        LoadOrCreateXml().Wait();
    }

    private async Task LoadOrCreateXml()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (File.Exists(_xmlFilePath))
            {
                var fileInfo = new FileInfo(_xmlFilePath);
                if (fileInfo.Length > 0)
                {
                    try
                    {
                        using (var stream = File.OpenRead(_xmlFilePath))
                        {
                            _xmlDocument = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
                            return;
                        }
                    }
                    catch (XmlException)
                    {
                        // Якщо XML пошкоджено — створити новий
                    }
                }
            }
            await CreateNewXmlDocument();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task CreateNewXmlDocument()
    {
        _xmlDocument = new XDocument(
            new XElement("TodoData",
                new XElement("Categories"),
                new XElement("Tasks")
            ));
        await SaveXml();
    }

    private async Task SaveXml()
    {
        //await _semaphore.WaitAsync();
        try
        {
            using (var stream = new FileStream(_xmlFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await _xmlDocument.SaveAsync(stream, SaveOptions.None, CancellationToken.None);
            }
        }
        finally
        {
            //_semaphore.Release();
        }
    }

    public async Task AddCategoryAsync(Categories category)
    {
        await _semaphore.WaitAsync();
        try
        {
            var categoriesElement = _xmlDocument.Root.Element("Categories");

            if (categoriesElement.Elements("Category")
                .Any(x => (string)x.Element("Name") == category.Name))
            {
                throw new InvalidOperationException($"Category with name '{category.Name}' already exists");
            }

            categoriesElement.Add(
                new XElement("Category",
                    new XElement("Id", await GetNextCategoryId()),
                    new XElement("Name", category.Name)
                ));
            await SaveXml();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<int> GetNextCategoryId()
    {
        var lastId = _xmlDocument.Root.Element("Categories")
            .Elements("Category")
            .Select(x => (int)x.Element("Id"))
            .DefaultIfEmpty(0)
            .Max();
        return lastId + 1;
    }

    public async Task<List<Categories>> GetCategoriesAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            return _xmlDocument.Root.Element("Categories")
                .Elements("Category")
                .Select(x => new Categories
                {
                    Id = (int)x.Element("Id"),
                    Name = (string)x.Element("Name")
                }).ToList();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<Tasks> AddTaskAsync(Tasks task)
    {
        await _semaphore.WaitAsync();
        try
        {
            var tasksElement = _xmlDocument.Root.Element("Tasks");
            var newId = await GetNextTaskId();

            var newTaskElement = new XElement("Task",
                new XElement("Id", newId),
                new XElement("Title", task.Title),
                new XElement("DueDate", task.DueDate?.ToString("o")),
                new XElement("IsCompleted", task.IsCompleted),
                new XElement("CompletedDate", task.CompletedDate?.ToString("o")),
                new XElement("CategoryId", task.CategoryId)
            );

            tasksElement.Add(newTaskElement);
            await SaveXml();

            return new Tasks
            {
                Id = newId,
                Title = task.Title,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                CompletedDate = task.CompletedDate,
                CategoryId = task.CategoryId
            };
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<int> GetNextTaskId()
    {
        var lastId = _xmlDocument.Root.Element("Tasks")
            .Elements("Task")
            .Select(x => (int)x.Element("Id"))
            .DefaultIfEmpty(0)
            .Max();
        return lastId + 1;
    }

    public async Task<List<Tasks>> GetActiveTasksAsync()
    {
        var tasks = await GetTasksQueryAsync();
        return tasks
            .Where(t => !t.IsCompleted)
            .OrderBy(t => t.Id)
            .ToList();
    }

    public async Task<List<Tasks>> GetCompletedTasksAsync()
    {
        var tasks = await GetTasksQueryAsync();
        return tasks
            .Where(t => t.IsCompleted)
            .OrderByDescending(t => t.CompletedDate)
            .ToList();
    }

    private async Task<IEnumerable<Tasks>> GetTasksQueryAsync()
    {
        var categories = (await GetCategoriesAsync()).ToDictionary(c => c.Id);
    
        await _semaphore.WaitAsync();
        try
        {
            return _xmlDocument.Root.Element("Tasks")
                .Elements("Task")
                .Select(x => new Tasks
                {
                    Id = (int)x.Element("Id"),
                    Title = (string)x.Element("Title"),
                    DueDate = ParseXmlDateTime(x.Element("DueDate")),
                    IsCompleted = (bool)x.Element("IsCompleted"),
                    CompletedDate = ParseXmlDateTime(x.Element("CompletedDate")),
                    CategoryId = ParseNullableInt(x.Element("CategoryId")),
                    CategoryName = ParseNullableInt(x.Element("CategoryId")) != null 
                        && categories.TryGetValue((int)x.Element("CategoryId"), out var category) 
                        ? category.Name 
                        : null
                });
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private int? ParseNullableInt(XElement element)
    {
        if (element == null || string.IsNullOrEmpty(element.Value))
            return null;
    
        return int.Parse(element.Value);
    }

    private DateTime? ParseXmlDateTime(XElement element)
    {
        if (element == null || string.IsNullOrEmpty(element.Value))
            return null;

        return DateTime.Parse(element.Value, null, System.Globalization.DateTimeStyles.RoundtripKind);
    }

    public async Task<Tasks> CompleteTask(int taskId)
    {
        await _semaphore.WaitAsync();
        try
        {
            var taskElement = _xmlDocument.Root.Element("Tasks")
                .Elements("Task")
                .FirstOrDefault(x => (int)x.Element("Id") == taskId);

            if (taskElement != null)
            {
                taskElement.Element("IsCompleted").Value = "true";
                var completedDate = DateTime.UtcNow;
                taskElement.Element("CompletedDate").Value = completedDate.ToString("o");
                await SaveXml();

                return new Tasks
                {
                    Id = taskId,
                    Title = taskElement.Element("Title").Value,
                    DueDate = DateTime.TryParse(taskElement.Element("DueDate")?.Value, out var due) ? due : (DateTime?)null,
                    CategoryId = int.TryParse(taskElement.Element("CategoryId")?.Value, out var catId) ? catId : (int?)null,
                    IsCompleted = true,
                    CompletedDate = completedDate
                };
            }

            return null;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}