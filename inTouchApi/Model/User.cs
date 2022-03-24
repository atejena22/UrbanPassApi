using System;
using System.Collections.Generic;

#nullable disable

namespace inTouchApi.Model
{
    public partial class User
    {
        public User()
        {
            Guestacces = new HashSet<Guestacce>();
            Guests = new HashSet<Guest>();
            Houses = new HashSet<House>();
        }

        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Active { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public DateTime lastLogin { get; set; }

        public bool? Deleted { get; set; }
        public bool? EmailConfirmation { get; set; }
        public int? UserParentId { get; set; }
        public string Image { get; set; }
        public int? UrbanizationId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Urbanization Urbanization { get; set; }
        public virtual ICollection<Guestacce> Guestacces { get; set; }
        public virtual ICollection<Guest> Guests { get; set; }
        public virtual ICollection<House> Houses { get; set; }
    }
}

/*
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Active { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public DateTime lastLogin { get; set; }

        public bool? Deleted { get; set; }
        public bool? EmailConfirmation { get; set; }
        public int? UserParentId { get; set; }
        public string Image { get; set; }
 */