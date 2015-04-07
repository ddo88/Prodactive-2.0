using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeitgeist.Web.Domain;

namespace Zeitgeist.Web.Data
{
    public class SampleData : DropCreateDatabaseIfModelChanges<Context>
    {
        public SampleData(Context context)
        {

            Persona p = new Persona();
            p.Nombre  = "Daniel";
            p.Identificacion = "1037587232";
            p.Peso = 40;
            p.FechaNacimiento=new DateTime(1988,3,17);
            p.Sexo=Sexo.Masculino;
            
            User user     = new User();
            user.UserName = "ddo88";
            user.Email    = "ddo88@hotmail.com";
            user.Persona  = p;

            context.Set<User>().Add(user);
            context.SaveChanges();
        }
    }
}
