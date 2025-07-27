
using Core.Models;

namespace Core.IRepo
{
	public interface IInstructorCoursesRepository : IGenericrepo<Ins_Course>
	{
        public IQueryable<IGrouping<int, Ins_Course>> GetCourseWithInstructorGrouped();
		public IEnumerable<IGrouping<int, Ins_Course>> GetCourseWithInstructorGroupedSorted(string sortBy, bool IsDesc);

		public Task<IEnumerable<Ins_Course>> CreateInstructorCourses(List<Ins_Course> InsCourses);
		public Task<IEnumerable<Ins_Course>> UpdateInstructorCourse(List<Ins_Course> insCourses, int instructorId);
		public Task<IEnumerable<IGrouping<int, Ins_Course>>> GetCourseInstructorByInstructorIdGrouped(int? id);
	}

}
