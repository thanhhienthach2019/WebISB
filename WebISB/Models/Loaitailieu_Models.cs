using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class Loaitailieu_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<Get_LoaiTaiLieu_Result> GetLoaiTaiLieu()
        {
            return empdb.Get_LoaiTaiLieu().ToList();
        }
        public List<Get_Tailieu_PhongBan_Result> Get_Tailieu_Phongban(string maloai)
        {
            return empdb.Get_Tailieu_PhongBan(maloai).ToList();
        }
    }
}