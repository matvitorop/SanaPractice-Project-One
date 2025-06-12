namespace MVC_Practice.Schema
{
    public class CategoryType : ObjectType<Models.Categories>
    {
        protected override void Configure(IObjectTypeDescriptor<Models.Categories> descriptor)
        {
            descriptor.Description("Represents a category in the to-do list.");

            descriptor.Field(c => c.Id)
                .Description("The unique identifier of the category.");
            
            descriptor.Field(c => c.Name)
                .Description("The name of the category.")
                .Type<NonNullType<StringType>>();
        }
    }
}
