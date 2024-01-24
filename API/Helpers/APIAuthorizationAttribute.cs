using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class APIAuthorizationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

        }

        public static bool CheckAccessRight(string Area, string Action, string Controller)
        {
            return true;
        }
    }
}
