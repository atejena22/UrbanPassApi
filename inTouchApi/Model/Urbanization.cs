using System;
using System.Collections.Generic;

#nullable disable

namespace inTouchApi.Model
{
    public partial class Urbanization
    {
        public Urbanization()
        {
            Guestacces = new HashSet<Guestacce>();
            Guests = new HashSet<Guest>();
            Houses = new HashSet<House>();
            Notificationdetails = new HashSet<Notificationdetail>();
            Users = new HashSet<User>();
        }

        public int UrbanizationId { get; set; }
        public string Urbanization1 { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public bool? Active { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public string Ruc { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }

        public virtual ICollection<Guestacce> Guestacces { get; set; }
        public virtual ICollection<Guest> Guests { get; set; }
        public virtual ICollection<House> Houses { get; set; }
        public virtual ICollection<Notificationdetail> Notificationdetails { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
/*
        public int UrbanizationId { get; set; }
        public string Urbanization1 { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactName { get; set; }
        public bool? Active { get; set; }
        public DateTime activeFrom { get; set; }
        public DateTime activeTo { get; set; }
        public string Ruc { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }
 */