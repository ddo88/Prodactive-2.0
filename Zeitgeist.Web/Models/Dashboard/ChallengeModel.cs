using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeitgeist.Web.Models.Dashboard
{
    public class ChallengeModel
    {
        public ChallengeModel()
        {
            DetailChallenges= new List<DetailChallenge>();
        }

        public List<DetailChallenge> DetailChallenges { get; set; }
    }

    public class DetailChallenge
    {
        public string   Name        { get; set; }
        public int      MyProgress  { get; set; }
        public int      Goal        { get; set; }
        public float Porcentage
        {
            get { return ((float)MyProgress / (float)Goal) * 100; }
        }

        public int Ranking { get; set; }
    }
}
