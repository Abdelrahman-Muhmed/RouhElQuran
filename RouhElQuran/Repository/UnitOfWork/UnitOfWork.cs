using Core.IRepo;
using Core.IUnitOfWork;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Models;
using Repository.Repos;

namespace Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly RouhElQuranContext _Context;
        private IDbContextTransaction? _Transaction;

        public UnitOfWork(RouhElQuranContext context)
        {
            _Context = context;
        }

        public async Task<int> SaveChangesAsync()
            => await _Context.SaveChangesAsync();

        public void Dispose()
            => _Context.Dispose();

        public async Task BeginTransactionAsync()
        {
            if (_Transaction == null)
                _Transaction = await _Context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_Transaction != null)
            {
                await _Transaction.CommitAsync();
                await _Transaction.DisposeAsync();
                _Transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_Transaction != null)
            {
                await _Transaction.RollbackAsync();
                await _Transaction.DisposeAsync();
                _Transaction = null;
            }
        }



        #region Repositories


        private ICourseRepository? _CourseRepository;
        public ICourseRepository CourseRepository
            => _CourseRepository ??= new CourseRepository(_Context);

        private IFreeClassRepository? _FreeClassRepository;
        public IFreeClassRepository FreeClassRepository
            => _FreeClassRepository ??= new FreeClassRepository(_Context);

        private IGenericRepository<Student>? _StudentRepository;
        public IGenericRepository<Student> StudentRepository
            => _StudentRepository ??= new GenericRepository<Student>(_Context);

        private ICoursePlanRepository? _CoursePlanRepository;
        public ICoursePlanRepository CoursePlanRepository
            => _CoursePlanRepository ??= new CoursePlanRepository(_Context);

        private IInstructorCoursesRepository? _InstructorCoursesRepository;
        public IInstructorCoursesRepository InstructorCoursesRepository
            => _InstructorCoursesRepository ??= new InstructorCoursesRepository(_Context);

        private IGenericRepository<Files>? _FilesRepository;
        public IGenericRepository<Files> FilesRepository
            => _FilesRepository ??= new GenericRepository<Files>(_Context);

        private IGenericRepository<Instructor>? _InstructorRepository;
        public IGenericRepository<Instructor> InstructorRepository
            => _InstructorRepository ??= new GenericRepository<Instructor>(_Context);

        private IGenericRepository<UserPayments>? _UserPaymentsRepository;
        public IGenericRepository<UserPayments> UserPaymentsRepository
            => _UserPaymentsRepository ??= new GenericRepository<UserPayments>(_Context);

        private IGenericRepository<Reviews>? _ReviewRepository;
        public IGenericRepository<Reviews> ReviewRepository
            => _ReviewRepository ??= new GenericRepository<Reviews>(_Context);

        private IGenericRepository<AppUser>? _UserRepository;
        public IGenericRepository<AppUser> UserRepository
            => _UserRepository ??= new GenericRepository<AppUser>(_Context);
        #endregion

    }
}
