using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsCore.Areas.Products.Helpers
{
    public static class GeneralHelper
    {
        public static string ToDateMDY_DMY(this string input)
        {
            string[] dat = input.Split(Convert.ToChar("/"));
            return string.Format("{0}/{1}/{2}", Convert.ToInt32(dat[2]), Convert.ToInt32(dat[0]), Convert.ToInt32(dat[1]));
        }
    }
}
