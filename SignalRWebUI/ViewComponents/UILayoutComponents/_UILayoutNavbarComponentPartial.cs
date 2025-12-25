using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUI.ViewComponents.UILayoutComponents
{
    public class _UILayoutNavbarComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Cookie'den MenuTableId'yi oku (HttpContext üzerinden)
            var menuTableId = HttpContext.Request.Cookies["MenuTableId"];
            if (!string.IsNullOrEmpty(menuTableId))
            {
                ViewBag.MenuTableId = menuTableId;
            }
            return View();
        }
    }
}
