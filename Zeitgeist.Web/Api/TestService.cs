using System.Web.Security;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Api.Model;
using ServiceStack.ServiceInterface;

using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Services;

namespace Zeitgeist.Web.Api
{
    public class TestService : Service
    {
        private readonly IUserService               _userService;
        private readonly IRepository<LogEjercicio> _logsRepository;

        public TestService(IUserService userService, IRepository<LogEjercicio> logsRepository)
        {
            _userService    = userService;
            _logsRepository = logsRepository;
        }

        //public ResponsePrueba Any(Prueba peticion)
        //{
        //    return new ResponsePrueba() {Fecha = DateTime.Now, Message = "", State = true};
        //}

        public ResponsePeticion Any(ServiceStatus serviceStatus)
        {
            return new ResponsePeticion() { Response = "ok" };
        }
        [Authenticate]
        public LoginResponse Any(Login login)
        {
            bool rt = Membership.ValidateUser(login.User, login.Pass);
            if (rt)
            {
                var result = _userService.GetCustomerByUsername(login.User);
                return new LoginResponse()
                {
                    Message = "Verifique los Datos",
                    State = false,
                    Nombre = result.Persona.Nombre,
                    Apellido = result.Persona.Apellido,
                    Identificacion = result.Persona.Identificacion,
                    Peso = result.Persona.Peso,
                    Estatura = result.Persona.Estatura
                };
            }

            return new LoginResponse()
            {

                Message = "Verifique los datos",
                State   = false
                
            };
        }

        public ResponseLogEjercicio Any(RequestLogEjercicio reg)
        {
            //Manager m = Manager.Instance;
            LogEjercicio l  = new LogEjercicio();
            l.User          = _userService.GetCustomerByUsername(reg.Usuario);
            l.Velocidad     = 32;
            l.Ubicacion     = "";
            l.Fecha         = reg.FechaHora;
            l.Conteo        = reg.Conteo;
            l.DeporteId     = reg.Deporte;

            _logsRepository.Insert(l);

            ResponseLogEjercicio res = new ResponseLogEjercicio();

            res.State = true;
            res.Message = "Se ha guardado con exito el registro.";
            return res;
        }
    }
}
