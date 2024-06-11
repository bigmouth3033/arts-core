using arts_core.Data;

namespace arts_core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private DataContext _dataContext;
        public IUserRepository UserRepository { get; set; }
        public UnitOfWork(DataContext dataContext, IUserRepository userRepository)
        {
            _dataContext = dataContext;
            UserRepository = userRepository;
        }        

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }
    }
}
