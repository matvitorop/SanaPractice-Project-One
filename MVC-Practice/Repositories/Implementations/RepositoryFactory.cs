using MVC_Practice.Repositories.Interfaces;

namespace MVC_Practice.Repositories.Implementations
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly TodoRepository _dbRepo;
        private readonly TodoXMLRepository _xmlRepo;
        public RepositoryFactory(TodoRepository dbRepo, TodoXMLRepository xmlRepo)
        {
            _dbRepo = dbRepo;
            _xmlRepo = xmlRepo;
        }
        public ITodoRepository CreateTodoRepository(string storageType)
        {
            return storageType == "xml" ? _xmlRepo : _dbRepo;
        }
    }
}
