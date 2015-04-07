using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class UserMap : ZeitgeistEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("User");
            HasKey(e => e.Id);

            Property(e => e.UserName)
                .HasColumnAnnotation(
                 "Index", new IndexAnnotation(new[] {
                    new IndexAttribute("idx_uq_username") {IsUnique = true}
                }));

            Property(e => e.Email);

            HasOptional(e => e.Avatar).WithMany(f => f.Users).HasForeignKey(e => e.AvatarId);
            
            HasRequired(e => e.Persona)
            .WithRequiredPrincipal(f => f.User);
        }
    }
}