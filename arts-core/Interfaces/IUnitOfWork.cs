using arts_core.Data;

namespace arts_core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ITypeRepository TypeRepository { get; }

        IProductRepository ProductRepository { get; }

        void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _dataContext;
        public IUserRepository UserRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public ITypeRepository TypeRepository { get; set; }

        public IProductRepository ProductRepository { get; set; }


        public UnitOfWork(DataContext dataContext, IUserRepository userRepository, ICategoryRepository categoryRepository, ITypeRepository typeRepository, IProductRepository productRepository)
        {
            _dataContext = dataContext;
            UserRepository = userRepository;
            CategoryRepository = categoryRepository;
            TypeRepository = typeRepository;
            ProductRepository = productRepository;
            
        }        

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }
    }
}
