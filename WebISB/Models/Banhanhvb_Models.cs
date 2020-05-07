using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class Banhanhvb_Models
    {
        ISBEntities empdb = new ISBEntities();
        
        public List<FN_Get_Banhanhvb_Result> GetBanhanhvb()
        {
            return empdb.FN_Get_Banhanhvb().ToList();
        }
        public List<Get_Banhanhvb_vb_Result> GetBanhanhvb_vb()
        {
            return empdb.Get_Banhanhvb_vb().ToList();
        }
        public List<FN_Get_vanbandi_Result> Get_vanbandi(string dv)
        {
            return empdb.FN_Get_vanbandi(dv).ToList();
        }
        public List<FN_Get_Banhanhvb_Tim_Result> Get_vanbanden_find(string sovb,string noiphathanh,string loaivanban,string trichyeu)
        {
            return empdb.FN_Get_Banhanhvb_Tim(sovb,noiphathanh,loaivanban,trichyeu).ToList();
        }
        public List<FN_Get_Banhanhvb_Tim_vbdi_Result> Get_vanbandi_find(string madv, string sovb, string noiphathanh, string loaivanban, string trichyeu)
        {
            return empdb.FN_Get_Banhanhvb_Tim_vbdi(madv, sovb, noiphathanh, loaivanban, trichyeu).ToList();
        }
        public List<FN_Get_Noibo_vbden_Result> Get_vanbannbden(string donvi)
        {
            return empdb.FN_Get_Noibo_vbden(donvi).ToList();
        }
        public List<Get_Banhanhvb_NB_Den_Result> Get_vanbannb_den_find(string madv, string sovb, string noiphathanh, string loaivanban, string trichyeu)
        {
            return empdb.Get_Banhanhvb_NB_Den(madv, sovb, noiphathanh, loaivanban, trichyeu).ToList();
        }
        public List<Get_vanbandi_NoiBo_Result> Get_vanbandi_noibo(string dv)
        {
            return empdb.Get_vanbandi_NoiBo(dv).ToList();
        }
        public List<Get_vanbandi_NoiBo_search_Result> Get_vanbandi_noibo_search(string sovb,string loaivanban,string trichyeu,string donvigui,string donvinhan)
        {
            return empdb.Get_vanbandi_NoiBo_search(sovb,loaivanban,trichyeu,donvigui,donvinhan).ToList();
        }
        public List<Get_Chitietvb_Result> Get_chitietvb(string id)
        {
            return empdb.Get_Chitietvb(id).ToList();
        }
        public List<Get_Chitietvb_vb_Result> Get_chitietvb_vb(string id)
        {
            return empdb.Get_Chitietvb_vb(id).ToList();
        }
        public List<Get_vanban_dashboard_Result> Get_vanban_dashboard(string madv)
        {
            return empdb.Get_vanban_dashboard(madv).ToList();
        }
        public List<Get_banhanhvanban_dashboard_Result> Get_banhanhvanban_dashboard(string madv)
        {
            return empdb.Get_banhanhvanban_dashboard(madv).ToList();
        }
        public int sp_delete_vanban(Guid? idvanban)
        {
            return empdb.sp_delVanban(idvanban);
        }
    }
}