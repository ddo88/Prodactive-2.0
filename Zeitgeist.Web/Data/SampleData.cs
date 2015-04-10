using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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

            //Persona p = new Persona();
            //p.Nombre  = "Daniel";
            //p.Identificacion = "1037587232";
            //p.Peso = 40;
            //p.FechaNacimiento=new DateTime(1988,3,17);
            //p.Sexo=Sexo.Masculino;
            
            //User user     = new User();
            //user.UserName = "ddo88";
            //user.Email    = "ddo88@hotmail.com";
            //user.Persona  = p;
            Setting s= new Setting();
            s.Name = SettingBase.League;
            s.TypeValue = SettingType.IntValue;
            context.Set<Setting>().Add(s);
            //context.Set<User>().Add(user);

            string script = File.ReadAllText(@"C:\Users\D\Source\Repos\Prodactive-2.0\Zeitgeist.Web\DatabaseSchema.sql");
            context.ExecuteSqlCommand(script);
            context.SaveChanges();
        }
    }
}
