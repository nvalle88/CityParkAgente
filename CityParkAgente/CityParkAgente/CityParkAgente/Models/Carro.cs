﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityParkAgente.Models
{
   public class Carro
    {
        public int CarroId { get; set; }
        public int ModeloId { get; set; }
        public int UsuarioId { get; set; }
        public string Placa { get; set; }
        public string Color { get; set; }

        public virtual Modelo Modelo { get; set; }
        public virtual Usuario Usuario { get; set; }
       
    }
}
