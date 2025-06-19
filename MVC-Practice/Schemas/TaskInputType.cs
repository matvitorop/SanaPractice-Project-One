using GraphQL.Types;

namespace MVC_Practice.Schemas
{
    public class TaskInputType : InputObjectGraphType<Models.Tasks>
    {
        public TaskInputType()
        {
            Name = "TaskInput";

            Field<NonNullGraphType<StringGraphType>>("title")
                .Description("Task`s description");

            Field<DateTimeGraphType>("duedate")
                .Description("Time for completing a task");
            
            Field<IntGraphType>("categoryId")
                .Description("Category ID for the task");
            
            Field<BooleanGraphType>("isCompleted")
                .Description("Is the task completed?")
                .DefaultValue(false);

            Field<DateTimeGraphType>("completedDate")
                .Description("Date when the task was completed")
                .DefaultValue(null);
        }
    }
}
