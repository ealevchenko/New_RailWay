namespace EFReference.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reference.InternalRailroad")]
    public partial class InternalRailroad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InternalRailroad()
        {
            Stations = new HashSet<Stations>();
        }

        public int id { get; set; }

        public int id_state { get; set; }

        [Required]
        [StringLength(250)]
        public string internal_railroad { get; set; }

        public int code { get; set; }

        [Required]
        [StringLength(10)]
        public string abbr { get; set; }

        [Required]
        [StringLength(300)]
        public string list_code_station { get; set; }

        public virtual States States { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stations> Stations { get; set; }
    }
}
