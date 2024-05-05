using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Domain.Entities
{
    public class Member : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string? DisplayName { get; set; }
        public string? FullName { get; set; }
        public string? Location { get; set; }
        public string? AboutMe { get; set; }
        public string? WebsiteLink { get; set; }
        public string? TwitterUsername { get; set; }
        public string? GitHubUsername { get; set; }
        public int Reputation { get; set; }

        private Member()
        {
            
        }
        public Member(Guid applicationUserId, string displayName)
        {
            Id = applicationUserId; 
            ApplicationUserId = applicationUserId;
            DisplayName = displayName;
        }

    }
}
