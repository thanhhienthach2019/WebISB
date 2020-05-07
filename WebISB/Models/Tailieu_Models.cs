using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class Tailieu_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<FN_Get_Tailieu_Result> GetTaiLieu(string maloai,string donvi)
        {
            return empdb.FN_Get_Tailieu(maloai, donvi).ToList();
        }
        
        public List<FN_Get_Tailieu_Trash_Result> GetTaiLieuTrash(string ms_dv)
        {
            return empdb.FN_Get_Tailieu_Trash(ms_dv).ToList();
        }
        public List<Get_tailieu_admin_Result> GetTaiLieuAdmin(string ms_dv)
        {
            return empdb.Get_tailieu_admin(ms_dv).ToList();
        }
    }
}