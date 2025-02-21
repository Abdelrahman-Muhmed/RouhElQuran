using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    // Add All Roles Needed in the Application Heres
    public static class RolesNames
    {
        public const string Admin = nameof(Admin);
        public const string Student = nameof(Student);
    }
}
