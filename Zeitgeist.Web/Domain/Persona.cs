using System;
using Zeitgeist.Core.Data.Interfaces;

namespace Zeitgeist.Web.Domain
{
    public class Persona : IEntity
    {

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int SexoId { get; set; }
        public Sexo Sexo
        {
            get { return (Sexo)this.SexoId; }
            set { this.SexoId = (int)value; }
        }
        public int Peso { get; set; }
        public decimal Estatura { get; set; }
        //cuentas
        public virtual User User { get; set; }

    }
}