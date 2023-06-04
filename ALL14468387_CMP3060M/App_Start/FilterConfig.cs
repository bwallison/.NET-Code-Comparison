using System.Web;
using System.Web.Mvc;

namespace ALL14468387_CMP3060M
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
