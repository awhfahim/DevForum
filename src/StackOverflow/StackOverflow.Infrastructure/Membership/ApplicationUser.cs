using Microsoft.AspNetCore.Identity;
using System;

namespace StackOverflow.Infrastructure.Membership
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? ProfileImageKey { get; set; }
    }
}
