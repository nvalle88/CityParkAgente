using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityParkAgente.Models
{
   public class Parqueo
    {
        public int ParqueoId { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public int? TarjetaCreditoId { get; set; }

        public virtual TarjetaCredito TarjetaCredito { get; set; }
        public virtual Usuario Usuario { get; set; }

    }
}
