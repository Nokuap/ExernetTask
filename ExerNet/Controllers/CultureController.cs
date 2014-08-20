using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exernet.Filters;
using Exernet.Models;

namespace Exernet.Controllers
{
    [Culture]
    public class CultureController : Controller
    {
       

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            // Сохраняем выбранную культуру в куки

            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }

        public ActionResult SetTheme(string theme)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            HttpCookie cookie = Request.Cookies["Theme"];
            if (cookie == null)
            {
                cookie = new HttpCookie("cookieValue");
                cookie.Name = "Theme";
                cookie.Value = theme;
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Value = theme;
                this.ControllerContext.HttpContext.Response.Cookies.Set(cookie);
            }
            return Redirect(returnUrl);
        }
    }
}