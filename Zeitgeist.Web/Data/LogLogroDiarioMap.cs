using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class LogLogroDiarioMap : ZeitgeistEntityTypeConfiguration<LogLogroDiario>
    {

        public LogLogroDiarioMap()
        {
            ToTable("LogLogroDiario");
            HasKey(e => e.Id);

            Property(e => e.Fecha);

            HasRequired(e => e.User)
                .WithMany(f => f.LogLogroDiarios)
                .HasForeignKey(e => e.UserId);

            HasRequired(e => e.Reto)
                .WithMany(f => f.LogLogrosDiarios)
                .HasForeignKey(e => e.RetoId);
        }
    }
}