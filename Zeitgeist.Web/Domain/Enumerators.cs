using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls.Expressions;
using System.Web.UI.WebControls.WebParts;


namespace Zeitgeist.Web.Domain
{
    public enum Sexo
    {
        Femenino,
        Masculino

    }


    public enum TipoConteo
    {
        Pasos      = 0,
        Metros     = 1,
        Repeticion = 2,
        Saltos     = 3
    }


    public enum TipoTip
    {
        Alimentacion,
        Salud,
        Deporte
    }
}
