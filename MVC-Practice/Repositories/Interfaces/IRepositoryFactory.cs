namespace MVC_Practice.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        ITodoRepository CreateTodoRepository(string storageType);
    }
}
