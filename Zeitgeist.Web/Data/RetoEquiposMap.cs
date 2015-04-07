using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class RetoEquiposMap : ZeitgeistEntityTypeConfiguration<RetoEquiposMapping>
    {
        public RetoEquiposMap()
        {
            ToTable("RetoEquiposMapping");
            HasKey(e => e.Id);
            
            HasRequired(e => e.Reto)
                .WithMany(f => f.RetoEquipoMapping)
                .HasForeignKey(e => e.RetoId)
                .WillCascadeOnDelete(false); 

            HasRequired(e => e.Equipo)
                .WithMany(f => f.RetosEquipoMapping)
                .HasForeignKey(e => e.EquipoId)
                .WillCascadeOnDelete(false);

            Property(e => e.RetoId).HasColumnAnnotation(
                "Index", new IndexAnnotation(new[] {
                    new IndexAttribute("idx_uq_reto_equipo",0) { IsUnique = true}
                })); ;

            Property(e => e.EquipoId).HasColumnAnnotation(
                 "Index", new IndexAnnotation(new[] {
                    new IndexAttribute("idx_uq_reto_equipo",1) {IsUnique = true}
                })); 
        }
    }
}