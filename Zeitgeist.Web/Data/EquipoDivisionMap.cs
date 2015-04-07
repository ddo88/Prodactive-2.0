using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class EquipoDivisionMap : ZeitgeistEntityTypeConfiguration<EquipoDivisionMapping>
    {
        public EquipoDivisionMap()
        {
            ToTable("EquipoDivisionMapping");
            HasKey(e => e.Id);


            HasRequired(e => e.Division).WithMany(f => f.EquiposDivisionMapping)
                .HasForeignKey(e => e.DivisionId)
                .WillCascadeOnDelete(false);

            HasRequired(e => e.Equipo)
                .WithMany(f => f.EquipoDivisionMapping)
                .HasForeignKey(e => e.EquipoId).WillCascadeOnDelete(false);;

            Property(e => e.DivisionId).HasColumnAnnotation(
                 "Index", new IndexAnnotation(new[] {
                    new IndexAttribute("idx_uq_equipo_division",0) { IsUnique = true}
                })); ;

            Property(e => e.EquipoId).HasColumnAnnotation(
                 "Index", new IndexAnnotation(new[] {
                    new IndexAttribute("idx_uq_equipo_division",1) {IsUnique = true}
                })); 

        }
    }
}