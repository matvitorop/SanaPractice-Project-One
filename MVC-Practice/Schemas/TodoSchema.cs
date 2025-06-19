using GraphQL.Types;
using MVC_Practice.Schemas;
using System.Diagnostics.CodeAnalysis;

namespace MVC_Practice.Schemas
{
    public class TodoSchema : Schema
    {
        public TodoSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<TodoQuery>();
            Mutation = provider.GetRequiredService<TodoMutation>();
        }
    }

}
