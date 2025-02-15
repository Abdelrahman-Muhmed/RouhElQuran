using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Add All Roles Needed in the Application Heres
    public sealed class RolesNames
    {
        public readonly List<IdentityRole<int>> RoleStore = new List<IdentityRole<int>>();
        public RolesNames()
        {
            RoleStore.Add(new IdentityRole<int> { Name = Admin, NormalizedName = Admin.ToUpper() });
            RoleStore.Add(new IdentityRole<int> { Name = Student, NormalizedName = Student.ToUpper() });
        }

        public const string Admin = nameof(Admin);
        public const string Student = nameof(Student);
    }
}
