using System;
using ServiceStack.ServiceHost;

namespace Zeitgeist.Web.Api.Model
{
    [Route("/LogEjercicio")]
    [Route("/LogEjercicio/{Usuario}/{FechaHora}/{Ubicacion}/{Conteo}/{Velocidad}/{Deporte}")]
    public class RequestLogEjercicio
    {
        public string Usuario { get; set; }
        public DateTime FechaHora { get; set; }
        public string Ubicacion { get; set; }
        public int Deporte { get; set; }
        //este campo esta sujeto a cambios por ejemplo cambiarlo por un entero y manejar m/h 
        public double Velocidad { get; set; }
        public int Conteo { get; set; }
        
    }

    public class ResponseLogEjercicio : ResponseService
    {
    }
}