using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Humanizer;
using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class TipMap : ZeitgeistEntityTypeConfiguration<Tip>
    {
        public TipMap()
        {
            ToTable("Tip");
            HasKey(e => e.Id);

            Property(e => e.TipoTipId);
            Property(e => e.Titulo);
            Property(e => e.Descripcion);
            
            Ignore(e => e.TipoTip);

            HasOptional(e => e.Picture)
                .WithMany(f => f.Tips)
                .HasForeignKey(e => e.PictureId);
        }
    }
}