using MVC_Practice.Models;

namespace MVC_Practice.Schema
{
    public class TaskType : ObjectType<Tasks>
    {
        protected override void Configure(IObjectTypeDescriptor<Tasks> descriptor)
        {
            descriptor.Description("Represents a task in the to-do list.");
            
            descriptor.Field(t => t.Id).
                Description("The unique identifier of the task.");
            
            descriptor.Field(t => t.Title)
                .Description("The title of the task.")
                .Type<NonNullType<StringType>>();

            descriptor.Field(t => t.DueDate)
                .Description("The due date of the task.")
                .Type<DateTimeType>();
            
            descriptor.Field(t => t.IsCompleted)
                .Description("Indicates whether the task is completed.")
                .Type<NonNullType<BooleanType>>();
            
            descriptor.Field(t => t.CompletedDate)
                .Description("The date when the task was completed.")
                .Type<DateTimeType>();
            
            descriptor.Field(t => t.CategoryId)
                .Description("The ID of the category to which the task belongs.")
                .Type<IntType>();
            
            descriptor.Field(t => t.CategoryName)
                .Description("The name of the category to which the task belongs.")
                .Type<StringType>();
        }
    }
}
