using ServiceStack.ServiceHost;

namespace Zeitgeist.Web.Api.Model
{
    [Route("/status")]
    public class ServiceStatus
    {
        public string  Message { get; set; }
    }

    public class ResponsePeticion
    {
        public string Response { get; set; }
    }

    
}