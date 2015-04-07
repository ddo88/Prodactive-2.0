using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class RetoMap : ZeitgeistEntityTypeConfiguration<Reto>
    {
        public RetoMap()
        {
            ToTable("Reto");
            HasKey(e => e.Id);

            Property(e => e.Name);
            Property(e => e.FechaInicio);
            Property(e => e.FechaFin);
            Property(e => e.Meta);
            Property(e => e.IsActivo);

            HasRequired(e => e.Premio)
                .WithMany(f => f.Retos)
                .HasForeignKey(e => e.PremioId);


            HasRequired(e => e.TipoReto)
                .WithMany(f => f.Retos)
                .HasForeignKey(e => e.TipoRetoId);

            HasRequired(e => e.Coach)
                .WithMany(e => e.CoachRetos)
                .HasForeignKey(e => e.CoachId);


            HasRequired(e => e.Owner)
                .WithMany(f => f.OwnerRetos)
                .HasForeignKey(e => e.OwnerId);

            HasRequired(e => e.Liga)
                .WithMany(f => f.Retos)
                .HasForeignKey(e => e.LigaId);

            HasRequired(e => e.Division)
                .WithMany(f => f.Retos)
                .HasForeignKey(e => e.DivisionId);
        }

    }
}