using Zeitgeist.Core.Data.Configuration;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class PictureMap : ZeitgeistEntityTypeConfiguration<Picture>
    {
        public PictureMap()
        {
            ToTable("Picture");

            HasKey(e => e.Id);

            Property(e => e.Bytes);
            Property(e => e.MimeType);
            Property(e => e.FileName);


        }
    }
}