using MVC_Practice.Repositories.Interfaces;

namespace MVC_Practice.Repositories.Implementations
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ITodoRepository _dbRepo;
        private readonly ITodoRepository _xmlRepo;
        public RepositoryFactory(ITodoRepository dbRepo, ITodoRepository xmlRepo)
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
