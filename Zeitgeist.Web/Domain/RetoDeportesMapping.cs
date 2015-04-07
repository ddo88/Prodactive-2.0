namespace Zeitgeist.Web.Domain
{
    public class RetoDeportesMapping
    {
        public int Id { get; set; }
        public int RetoId { get; set; }
        public Reto Reto { get; set; }
        public int DeporteId { get; set; }
        public Deporte Deporte { get; set; }

    }
}