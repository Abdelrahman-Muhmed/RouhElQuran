using Core.IRepo;
using Core.Models;
using Repository.Models;

namespace Core.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();


        IFreeClassRepository FreeClassRepository { get; }
        ICourseRepository CourseRepository { get; }
        ICoursePlanRepository CoursePlanRepository { get; }
        IInstructorCoursesRepository InstructorCoursesRepository { get; }
        IGenericRepository<Student> StudentRepository { get; }
        IGenericRepository<Files> FilesRepository { get; }
        IGenericRepository<Instructor> InstructorRepository { get; }
        IGenericRepository<UserPayments> UserPaymentsRepository { get; }
    }

}
