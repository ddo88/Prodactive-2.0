using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Tip : IEntity
    {
        public int      TipoTipId { get; set; }
        public TipoTip  TipoTip
        {
            get
            {
                return (TipoTip)this.TipoTipId;
            }
            set
            {
                this.TipoTipId = (int)value;
            }
        }
        public string   Titulo { get; set; }
        public string   Descripcion { get; set; }
        public int?      PictureId { get; set; }
        public Picture  Picture { get; set; }

    }
}