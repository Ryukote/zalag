using System;

namespace Domain.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public UserAccount? User { get; set; }

        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
