using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class Loaitin_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<FN_Get_LoaiTin_Result> GetLoaiTin()
        {
            return empdb.FN_Get_LoaiTin().ToList();
        }
        public List<FN_Get_LoaiTin_Trash_Result> GetLoaiTinTrash()
        {
            return empdb.FN_Get_LoaiTin_Trash().ToList();
        }
        public int Update_Status_TT(int? idLoaitin, int up)
        {
            int res = empdb.FN_Update_Tintuc_Status(idLoaitin, up);
            return res;
        }
        public int Count_Status_Tl(int? idTheLoai, int tt_s)
        {
            int res = empdb.FN_Count_status(idTheLoai, tt_s);
            return res;
        }
        public int Delete_Tintuc(int? idLoaitin)
        {
            int res = empdb.FN_Delete_TinTuc(idLoaitin);
            return res;
        }
    }
}