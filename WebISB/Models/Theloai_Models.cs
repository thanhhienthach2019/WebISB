using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class Theloai_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<FN_Get_TheLoai_Result> GetTheLoai()
        {
            return empdb.FN_Get_TheLoai().ToList();
        }
        public int Update_Status_LT(int? idTheLoai,int up)
        {
            int res = empdb.FN_Update_Loaitin_Status(idTheLoai, up);
            return res;
        }
        public List<FN_Get_TheLoai_Trash_Result> GetTheLoaiTrash()
        {
            return empdb.FN_Get_TheLoai_Trash().ToList();
        }
        public int Delete_Loaitin_Tintuc(int? idTheLoai)
        {
            int res = empdb.FN_Delete_Loaitin_TinTuc(idTheLoai);
            return res;
        }

    }
}