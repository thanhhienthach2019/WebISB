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
    
    public partial class Get_TinTuc_Trash_Result
    {
        public Nullable<long> STT { get; set; }
        public int id { get; set; }
        public string TieuDe { get; set; }
        public string TieuDeKhongDau { get; set; }
        public string TomTat { get; set; }
        public string NoiDung { get; set; }
        public string ImagePath { get; set; }
        public Nullable<int> NoiBat { get; set; }
        public Nullable<int> SoLuotXem { get; set; }
        public int idLoaiTin { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public string User_Create { get; set; }
        public Nullable<int> status { get; set; }
    }
}
