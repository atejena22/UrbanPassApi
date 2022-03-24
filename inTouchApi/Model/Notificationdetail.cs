using System;
using System.Collections.Generic;

#nullable disable

namespace inTouchApi.Model
{
    public partial class Notificationdetail
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaEstablecida { get; set; }
        public string Notas { get; set; }
        public int? UrbanizationId { get; set; }
       // public TimeSpan? tiempo { get; set; }
        public string tiempo { get; set; }


        public virtual Urbanization Urbanization { get; set; }
    }
}
/*
    public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaEstablecida { get; set; }
        public string Notas { get; set; }
        public int? UrbanizationId { get; set; }
        public string tiempo { get; set; }

        public virtual Urbanization Urbanization { get; set; }*/