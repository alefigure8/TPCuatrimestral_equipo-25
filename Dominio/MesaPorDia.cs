﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
	public class MesaPorDia
	{
        public int Id { get; set; }
        public int? Mesero { get; set; }
        public int Mesa { get; set; }
        public int? IDMeseroPorDia { get; set; }
		public DateTime Fecha { get; set; }
		public TimeSpan Apertura { get; set; }
		public TimeSpan? Cierre { get; set; }
	}
}
