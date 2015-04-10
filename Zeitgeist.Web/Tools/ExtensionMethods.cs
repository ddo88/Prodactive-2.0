using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Models.Page;

namespace Zeitgeist.Web.Tools
{
    public static class ExtensionMethods
    {
        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            // because "[" and "]" aren't replaced with "_" in GetFullHtmlFieldId
            return id.Replace('[', '_').Replace(']', '_');
        }

        public static string GetFullName(this Persona person)
        {
            return String.Format("{0} {1}", person.Nombre, person.Apellido);
        }

        public static List<MenuItem> ToSubMenuItems(this List<Reto> retos)
        {
            List<MenuItem> list= new List<MenuItem>();
            foreach (var reto in retos)
            {
                var item = new MenuItem();
                item.Name = reto.Name;
                item.Href = String.Format("/Reto/Details/{0}", reto.Id);
                list.Add(item);
            }
            return list;
        }

        private static UserSettingsMapping GetSettingMapping (User user, string name)
        {
            if (user.UserSettings != null || user.UserSettings.Count > 0)
            {
                var r = user.UserSettings.Where(x => x.Setting.Name.Contains(SettingBase.League));
                if (r.Count() > 0)
                    return r.First();

            }
            return null;
        }

        public static object GetSetting(this User user, string name)
        {
            var setting = GetSettingMapping(user, name);
            if (setting == null)
                return null;

            switch (setting.Setting.TypeValue)
            {
                case 0:
                    return Convert.ToInt32(setting.Value);
                case 1:
                    return setting.Value;
            }
            return null;
        }


    }


    public static class HtmlExtensions
    {
        public static MvcHtmlString Script(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return MvcHtmlString.Empty;
        }
    }
}
