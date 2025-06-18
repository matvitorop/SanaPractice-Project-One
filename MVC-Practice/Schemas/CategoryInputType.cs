using GraphQL.Types;

namespace MVC_Practice.Schemas
{
    public class CategoryInputType : InputObjectGraphType<Models.Categories>
    {
        public CategoryInputType()
        {
            Name = "CategoryInput";
            Field<NonNullGraphType<StringGraphType>>("name")
                .Description("Name of the category");
        }
    }
}
