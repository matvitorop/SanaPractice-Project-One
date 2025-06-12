using MVC_Practice.Models;
using MVC_Practice.Repositories.Interfaces;
using System.Xml;
using System.Xml.Linq;

namespace MVC_Practice.Repositories.Implementations
{
    public class TodoXMLRepository : ITodoRepository
    {
        private readonly string _xmlFilePath;
        private XDocument _xmlDocument;

        public TodoXMLRepository(IConfiguration config)
        {
            var relativePath = config["XmlSettings:FilePath"];
            _xmlFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            LoadOrCreateXml();
        }

        private void LoadOrCreateXml()
        {
            if (File.Exists(_xmlFilePath) && new FileInfo(_xmlFilePath).Length > 0)
            {
                try
                {
                    _xmlDocument = XDocument.Load(_xmlFilePath);
                }
                catch (XmlException)
                {
                    // Якщо XML пошкоджено — створити новий
                    CreateNewXmlDocument();
                }
            }
            else
            {
                CreateNewXmlDocument();
            }
        }

        private void CreateNewXmlDocument()
        {
            _xmlDocument = new XDocument(
                new XElement("TodoData",
                    new XElement("Categories"),
                    new XElement("Tasks")
                ));
            SaveXml();
        }

        private void SaveXml()
        {
            _xmlDocument.Save(_xmlFilePath);
        }

        public async Task AddCategoryAsync(Categories category)
        {
            var categoriesElement = _xmlDocument.Root.Element("Categories");
            categoriesElement.Add(
                new XElement("Category",
                    new XElement("Id", GetNextCategoryId()),
                    new XElement("Name", category.Name)
                ));
            SaveXml();
            await Task.CompletedTask;
        }

        private int GetNextCategoryId()
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
            return await Task.FromResult(
                _xmlDocument.Root.Element("Categories")
                    .Elements("Category")
                    .Select(x => new Categories
                    {
                        Id = (int)x.Element("Id"),
                        Name = (string)x.Element("Name")
                    }).ToList());
        }

        public async Task AddTaskAsync(Tasks task)
        {
            var tasksElement = _xmlDocument.Root.Element("Tasks");
            tasksElement.Add(
                new XElement("Task",
                    new XElement("Id", GetNextTaskId()),
                    new XElement("Title", task.Title),
                    new XElement("DueDate", task.DueDate.HasValue ? task.DueDate.ToString() : null),
                    new XElement("IsCompleted", task.IsCompleted),
                    new XElement("CompletedDate", task.CompletedDate.HasValue ? task.CompletedDate.ToString() : null),
                    new XElement("CategoryId", task.CategoryId.HasValue ? task.CategoryId.ToString() : null)
                ));
            SaveXml();
            await Task.CompletedTask;
        }

        private int GetNextTaskId()
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
            return await Task.FromResult(GetTasksQuery()
                .Where(t => !t.IsCompleted)
                .OrderBy(t => t.Id)
                .ToList());
        }

        public async Task<List<Tasks>> GetCompletedTasksAsync()
        {
            return await Task.FromResult(GetTasksQuery()
                .Where(t => t.IsCompleted)
                .OrderByDescending(t => t.CompletedDate)
                .ToList());
        }

        private IEnumerable<Tasks> GetTasksQuery()
        {
            var categories = GetCategoriesAsync().Result.ToDictionary(c => c.Id);

            return _xmlDocument.Root.Element("Tasks")
                .Elements("Task")
                .Select(x => new Tasks
                {
                    Id = (int)x.Element("Id"),
                    Title = (string)x.Element("Title"),
                    DueDate = string.IsNullOrEmpty((string)x.Element("DueDate")) ? null : DateTime.Parse((string)x.Element("DueDate")),
                    IsCompleted = (bool)x.Element("IsCompleted"),
                    CompletedDate = string.IsNullOrEmpty((string)x.Element("CompletedDate")) ? null : DateTime.Parse((string)x.Element("CompletedDate")),
                    CategoryId = string.IsNullOrEmpty((string)x.Element("CategoryId")) ? null : (int?)int.Parse((string)x.Element("CategoryId")),
                    CategoryName = string.IsNullOrEmpty((string)x.Element("CategoryId")) ? null : categories[(int)x.Element("CategoryId")].Name
                });
        }

        public async Task CompleteTask(int taskId)
        {
            var taskElement = _xmlDocument.Root.Element("Tasks")
                .Elements("Task")
                .FirstOrDefault(x => (int)x.Element("Id") == taskId);

            if (taskElement != null)
            {
                taskElement.Element("IsCompleted").Value = "true";
                taskElement.Element("CompletedDate").Value = DateTime.Now.ToString();
                SaveXml();
            }
            await Task.CompletedTask;
        }
    }
}
