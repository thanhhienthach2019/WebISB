//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebISB.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class loaitin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public loaitin()
        {
            this.tintucs = new HashSet<tintuc>();
        }
    
        public int id { get; set; }
        public int idTheLoai { get; set; }
        public string Ten { get; set; }
        public string TenKhongDau { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public Nullable<short> status { get; set; }
    
        public virtual theloai theloai { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tintuc> tintucs { get; set; }
    }
}
