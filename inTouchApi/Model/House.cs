using System;
using System.Collections.Generic;

#nullable disable

namespace inTouchApi.Model
{
    public partial class House
    {
        public House()
        {
            Guests = new HashSet<Guest>();
        }

        public int HouseId { get; set; }
        public int UrbanizationId { get; set; }
        public int UserId { get; set; }
        public string Mz { get; set; }
        public string Villa { get; set; }
        public string PhoneNumber { get; set; }
        public string Notes { get; set; }
        public string FullAddress { get; set; }

        public virtual Urbanization Urbanization { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Guest> Guests { get; set; }
    }
}


