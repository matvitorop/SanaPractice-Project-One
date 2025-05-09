namespace MVC_Practice.Models.Todo_Models
{
    public class TodoListViewModel
    {
        public List<Categories> Categories { get; set; }
        public List<Tasks> ActiveTasks { get; set; }
        public List<Tasks> CompletedTasks { get; set; }
        public Tasks NewTask { get; set; }
    }
}
