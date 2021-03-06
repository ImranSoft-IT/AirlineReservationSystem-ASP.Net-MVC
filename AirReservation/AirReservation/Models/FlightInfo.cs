//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirReservation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class FlightInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FlightInfo()
        {
            this.BookingFlights = new HashSet<BookingFlight>();
        }
    
        public int FlightID { get; set; }
        [DisplayName("Flight Number")]
        [Required(ErrorMessage = "You must provide a Flight number")]
        public string FlightNunber { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public string TakeOff { get; set; }
        [Required]
        [DataType(DataType.Time)]
        public string Landing { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookingFlight> BookingFlights { get; set; }
    }
}
