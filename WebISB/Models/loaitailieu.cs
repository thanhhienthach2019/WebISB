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
    
    public partial class loaitailieu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public loaitailieu()
        {
            this.tailieux = new HashSet<tailieu>();
        }
    
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public Nullable<System.DateTime> Create_at { get; set; }
        public string User_create { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tailieu> tailieux { get; set; }
    }
}
