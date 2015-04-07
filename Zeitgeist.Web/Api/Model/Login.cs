using ServiceStack.ServiceHost;
using Zeitgeist.Web.Domain;


namespace Zeitgeist.Web.Api.Model
{
    [Route("/login")]
    [Route("/login/{User}/{Pass}")]
    public class Login
    {
        public string User { get; set; }
        public string Pass { get; set; }
    }


    public class LoginResponse : ResponseService
    {
        public string  User           { get; set; }
        public string  Nombre         { get; set; }
        public string  Apellido       { get; set; }
        public string  Identificacion { get; set; }
        public int     Peso           { get; set; }
        public decimal Estatura       { get; set; }
    }

}