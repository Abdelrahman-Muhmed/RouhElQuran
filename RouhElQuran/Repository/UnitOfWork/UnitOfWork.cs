using Core.IRepo;
using Core.IUnitOfWork;
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

        #endregion

    }
}
