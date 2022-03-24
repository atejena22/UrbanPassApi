using System;
using System.Collections.Generic;

#nullable disable

namespace inTouchApi.Model
{
    public partial class Guest
    {
        public Guest()
        {
            Guestacces = new HashSet<Guestacce>();
        }

        public int GuestId { get; set; }
        public int UserId { get; set; }
        public int UrbanizationId { get; set; }
        public int HouseId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Identification { get; set; }
        public string Plate { get; set; }
        public bool? Delivery { get; set; }
        public string Restaurant { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateCreation { get; set; }


        public virtual House House { get; set; }
        public virtual Urbanization Urbanization { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Guestacce> Guestacces { get; set; }
    }
}


