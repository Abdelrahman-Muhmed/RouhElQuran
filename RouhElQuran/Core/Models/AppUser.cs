using Core.Models;
using Microsoft.AspNetCore.Identity;


namespace Repository.Models;

public class AppUser : IdentityUser<int>
{
    public string? Country { get; set; }
    public string? Language { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PersonalImage { get; set; }
    public virtual Employee? Emp { get; set; }
    public virtual Instructor? Ins { get; set; }
    public virtual Student? Std { get; set; }

    public ICollection<Files> files { get; set; } = new List<Files>();
}