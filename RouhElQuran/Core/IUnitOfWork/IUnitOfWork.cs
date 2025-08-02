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

        // Courses
        ICourseRepository CourseRepository { get; }
        ICoursePlanRepository CoursePlanRepository { get; }
        IFreeClassRepository FreeClassRepository { get; }

        // Users
        IGenericRepository<Student> StudentRepository { get; }
        IGenericRepository<Instructor> InstructorRepository { get; }
        IGenericRepository<AppUser> UserRepository { get; }
        // Payments
        IGenericRepository<UserPayments> UserPaymentsRepository { get; }

        // Files
        IGenericRepository<Files> FilesRepository { get; }

        // Reviews
        IGenericRepository<Reviews> ReviewRepository { get; }

        // Instructor Mapping
        IInstructorCoursesRepository InstructorCoursesRepository { get; }
    }


}
