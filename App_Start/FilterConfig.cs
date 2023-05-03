using System.Web;
using System.Web.Mvc;

namespace B2C_Ecomerce_Website
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
