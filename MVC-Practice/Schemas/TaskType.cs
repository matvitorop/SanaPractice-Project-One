using MVC_Practice.Models;
using GraphQL.Types;

namespace MVC_Practice.Schemas
{
    public class TaskType : ObjectGraphType<Tasks>
    {
        public TaskType() 
        {
            Field(x => x.Id)
                .Description("The unique identifier of the task.");
            
            Field(x => x.Title)
                .Description("The title of the task.");

            Field(x => x.IsCompleted)
                .Description("Mark of task status");

            Field(x => x.DueDate, nullable: true)
                .Description("Time for completing task");
            
            Field(x => x.CompletedDate, nullable: true)
                .Description("Date when task was completed");
            
            Field(x => x.CategoryId, nullable: true)
                .Description("Id of task category");
            
            Field(x => x.CategoryName, nullable: true);
        }
    }
}
