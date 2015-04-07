using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
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
    }
}
