using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebISB.Models
{
    public class Tintuc_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<FN_Get_TinTuc_Result> GetTintucs()
        {
            return empdb.FN_Get_TinTuc().ToList();
        }
        
        public List<FN_Get_TinTuc_Trash_Result> GetTintucTrash()
        {
            return empdb.FN_Get_TinTuc_Trash().ToList();
        }
        public int Count_Status_Lt(int? idLoaitin, int tt_s)
        {
            int res = empdb.FN_Count_status_Lt(idLoaitin, tt_s);
            return res;
        }
        public List<FN_Get_TinTuc_TC_Result> GetTinTuc_TC()
        {
            return empdb.FN_Get_TinTuc_TC().ToList();
        }
        public List<FN_Get_TinTuc_Loai_TC_Result> GetTinTucLoai_TC(string loai,int? id)
        {
            return empdb.FN_Get_TinTuc_Loai_TC(loai, id).ToList();
        }
        public List<FN_Get_ChiTietTin_Result> Get_Chitiettin(int? id)
        {
            return empdb.FN_Get_ChiTietTin(id).ToList();
        }
        public List<FN_Get_ChiTietTin_LT_Result> Get_Chitiettin_LT(int? idLT)
        {
            return empdb.FN_Get_ChiTietTin_LT(idLT).ToList();
        }
        public List<FN_Get_ChiTietTin_Noibat_Result> Get_Chitiettin_Noibat()
        {
            return empdb.FN_Get_ChiTietTin_Noibat().ToList();
        }
    }
}