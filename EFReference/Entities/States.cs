namespace EFReference.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reference.States")]
    public partial class States
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public States()
        {
            Countrys = new HashSet<Countrys>();
            InternalRailroad = new HashSet<InternalRailroad>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string state { get; set; }

        [Required]
        [StringLength(250)]
        public string name_network { get; set; }

        [Required]
        [StringLength(10)]
        public string abb_ru { get; set; }

        [Required]
        [StringLength(10)]
        public string abb_en { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Countrys> Countrys { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InternalRailroad> InternalRailroad { get; set; }
    }
}
