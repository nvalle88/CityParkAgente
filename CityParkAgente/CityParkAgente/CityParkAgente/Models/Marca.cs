using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityParkAgente.Models
{
  public  class Marca
    {


        public int MarcaId { get; set; }
        public string Nombre { get; set; }

       
        public virtual List<Modelo> Modelo { get; set; }
    }
}
