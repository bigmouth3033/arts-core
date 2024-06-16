using arts_core.Data;

namespace arts_core.Interfaces
{
    //nhom tat ca repositories lại với nhau để đám bảo tính nhất quán của dữ liệu
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ITypeRepository TypeRepository { get; }

        IProductRepository ProductRepository { get; }
        ICartRepository CartRepository { get; }

        void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _dataContext;
        public IUserRepository UserRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }
        public ITypeRepository TypeRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public ICartRepository CartRepository { get; set; }


        public UnitOfWork(DataContext dataContext, IUserRepository userRepository, ICategoryRepository categoryRepository, ITypeRepository typeRepository, IProductRepository productRepository, ICartRepository cartRepository)
        {
            _dataContext = dataContext;
            UserRepository = userRepository;
            CategoryRepository = categoryRepository;
            TypeRepository = typeRepository;
            ProductRepository = productRepository;
            CartRepository = cartRepository;
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
