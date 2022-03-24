using System;
using System.Collections.Generic;

#nullable disable

namespace inTouchApi.Model
{
    public partial class Guestacce
    {
        public int GuestAccessId { get; set; }
        public int? GuestId { get; set; }
        public int? UserId { get; set; }
        public int? UrbanizationId { get; set; }
        public DateTime accessFrom { get; set; }
        public DateTime accessTo { get; set; }
        public string AccessCode { get; set; }
        public string AccessImage { get; set; }

        public virtual Guest Guest { get; set; }
        public virtual Urbanization Urbanization { get; set; }
        public virtual User User { get; set; }
    }
}


