using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class PlanMap : ZeitgeistEntityTypeConfiguration<Plan>
    {
        public PlanMap()
        {

            ToTable("Plan");
            HasKey(e => e.Id);

            Property(e => e.Name);
            Property(e => e.UsuariosAdmitidos);
        }
    }
}