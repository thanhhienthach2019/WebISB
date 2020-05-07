using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebISB.Models
{
    public class SlidePage_Models
    {
        ISBEntities empdb = new ISBEntities();
        public List<slide> GetSlidePage()
        {
            return empdb.FN_Get_Slide_Page().ToList();
        }
    }
}