using GraphQL.Types;

namespace MVC_Practice.Schema
{
    public class CategoryType : ObjectGraphType<Models.Categories>
    {
        public CategoryType()
        {
            Field(x => x.Id)
                .Description("The unique identifier of the category.");

            Field(x => x.Name)
                .Description("The name of the category.");
        }
    }
}
