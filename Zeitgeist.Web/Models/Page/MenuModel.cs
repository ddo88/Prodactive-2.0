using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeitgeist.Web.Models.Page
{
    public class MenuModel
    {
        public MenuModel()
        {
            MenuItems= new List<MenuItem>();
        }
        public List<MenuItem> MenuItems { get; set; }   
    }


    public class MenuItem
    {
        public MenuItem()
        {
            SubMenuItems= new List<MenuItem>();
        }

        public string Href { get; set; }
        public string Name { get; set; }

        public string IconClass { get; set; }

        public List<MenuItem> SubMenuItems { get; set; }
        public bool Active { get; set; }
    }

    public class MenuIcon
    {
        public const string About        = "fa-exclamation";
        public const string Calendar     = "fa-calendar";
        public const string Galery       = "fa-picture-o";
        public const string Questions    = "fa-question";
        public const string Tips         = "fa-pencil-square-o";
        public const string Achievements = "fa-check-square-o";
        public const string Statistics   = "fa-flag-checkered";
        public const string Challenge    = "fa-pencil-square";
        public const string Dashboard    = "fa-desktop";
    }
}
