using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class RetoDeporteMap : ZeitgeistEntityTypeConfiguration<RetoDeportesMapping>
    {
        public RetoDeporteMap()
        {
            ToTable("RetoDeporteMapping");
            HasKey(e => e.Id);

            HasRequired(e => e.Reto)
                .WithMany(f => f.RetoDeporteMapping)
                .HasForeignKey(e => e.RetoId)
                .WillCascadeOnDelete(false);

            HasRequired(e => e.Deporte)
                .WithMany(f => f.RetoDeporteMapping)
                .HasForeignKey(e => e.DeporteId)
                .WillCascadeOnDelete(false);

            Property(e => e.RetoId).HasColumnAnnotation(
                 "Index", new IndexAnnotation(new[] {
                    new IndexAttribute("idx_uq_reto_deporte",0) { IsUnique = true}
                })); ;

            Property(e => e.DeporteId).HasColumnAnnotation(
                 "Index", new IndexAnnotation(new[] {
                    new IndexAttribute("idx_uq_reto_deporte",1) {IsUnique = true}
                })); 

        }
    }
}