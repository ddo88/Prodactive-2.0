using System.Collections.Generic;

namespace Zeitgeist.Web.Models.Page
{
    public class NavbarModel
    {
        public NavbarModel()
        {
            User= new PersonalData();
            Leagues= new LeagueData();
        }

        public PersonalData User    { get; set; }

        public LeagueData   Leagues { get; set; }

        public class PersonalData
        {
            public string Name { get; set; }
            public string AvatarUrl { get; set; }
            public bool AccessToAdmin { get; set; }
        }

        public class LeagueData
        {
            public LeagueData()
            {
                Leagues= new List<League>();
            }

            public List<League> Leagues { get; set; }

        }


    }

    public class League
    {
        public string Name { get; set; }
        public int IdLeague { get; set; }
        public bool Selected { get; set; }
    }


    
}