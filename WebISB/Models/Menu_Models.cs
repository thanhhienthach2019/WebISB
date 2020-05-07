using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class Menu_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<F_Get_Side_Menu_Result> GetMenu(string id)
        {
            return empdb.F_Get_Side_Menu(id).ToList();
        }
    }
}