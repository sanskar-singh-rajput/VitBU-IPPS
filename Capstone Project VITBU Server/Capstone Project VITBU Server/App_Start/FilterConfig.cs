using System.Web;
using System.Web.Mvc;

namespace Capstone_Project_VITBU_Server
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
