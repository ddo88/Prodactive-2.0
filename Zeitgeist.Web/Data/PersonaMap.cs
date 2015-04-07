using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class PersonaMap : ZeitgeistEntityTypeConfiguration<Persona>
    {
        public PersonaMap()
        {
            ToTable("Persona");
            HasKey(e => e.Id);

            Property(e => e.Nombre);
            Property(e => e.Apellido);
            Property(e => e.Identificacion)
                .HasColumnAnnotation(
                 "Index", new IndexAnnotation(new[] {
                    new IndexAttribute("idx_uq_identificacion") {IsUnique = true}
                }));
            Property(e => e.FechaNacimiento);
            Property(e => e.SexoId);
            Property(e => e.Peso);
            Property(e => e.Estatura);
            
            Ignore(e => e.Sexo);

            

        }
    }
}