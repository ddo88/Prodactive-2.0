using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class LogEjercicioMap : ZeitgeistEntityTypeConfiguration<LogEjercicio>
    {

        public LogEjercicioMap()
        {
            ToTable("LogEjercicios");
            HasKey(e => e.Id);

            Property(e => e.Conteo);
            Property(e => e.Fecha);
            Property(e => e.Ubicacion);
            Property(e => e.Velocidad);
            
            HasRequired(e => e.User)
                .WithMany(f => f.LogEjercicios)
                .HasForeignKey(e => e.UserId);

            HasRequired(e => e.Deporte)
                .WithMany(f => f.LogEjercicios)
                .HasForeignKey(e => e.DeporteId);


        }
    }
}